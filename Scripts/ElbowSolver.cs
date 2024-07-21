using Godot;
using System;
using System.Collections.Generic;

public partial class ElbowSolver : BodyPartSolver, ILElbowSolver, IRElbowSolver, ILShoulderSolver, IRShoulderSolver
{
    [Export] public BodyPartSolver ShoulderSolver;
    [Export] public bool UseExternalHint = false;
    [Export] public bool Lefthanded = false;

    [Export] private Vector3 ShoulderWristMeanHintOffset;


    private Vector3 _ExternalHint;

    private Vector3 _ElbowPos;
    private Basis _ElbowBas;
    private Vector3 _ShoulderPos;
    private Basis _ShoulderBas;


    public override void Update(BodySolver Solver)
    {
        Vector3 elbowHint = GetHint(Solver);

        SolveElbow(Solver, elbowHint);
        SolveShoulder();
    }

    private void SolveElbow(BodySolver Solver, Vector3 Hint)
    {
        GetWristPose(Solver, out Vector3 wristPos, out Basis wristBas);
        GetShoulderPose(Solver, out Vector3 shoulderPos, out Basis shoulderBas);

        //cosine law. we know all sides of the triangle, so we know all angles.
        //from there, we calculate a point - because of the hint, there's only one
        //valid solution
        float shoulderWristDist = wristPos.DistanceTo(shoulderPos);
        float shoulderAngle = CosineLawAngle(VRUserMeasurements.Arm, shoulderWristDist, VRUserMeasurements.Forearm, out _);
        Vector3 armPlaneNormal = CalculateArmPlaneNormal(wristPos, shoulderPos, Hint);
        Vector3 elbowDir = (wristPos - shoulderPos).Normalized().Rotated(armPlaneNormal, shoulderAngle).Normalized();

        _ElbowPos = shoulderPos + (elbowDir * VRUserMeasurements.Arm);
        _ElbowBas = Basis.LookingAt(wristPos - _ElbowPos);
    }
    private Vector3 CalculateArmPlaneNormal(Vector3 Wrist, Vector3 Shoulder, Vector3 Hint)
    {
        Vector3 normal = new Plane(Shoulder, Hint, Wrist).Normal;

        return normal;
    }
    //calculates the angle made at the vertex C, neighboring sides a and b
    private float CosineLawAngle(float a, float b, float c, out bool GotNan)
    {
        float Angle = Mathf.Acos(((a * a) + (b * b) - (c * c)) / (2 * a * b));
        if (float.IsNaN(Angle))
        {
            //returning NaN causes all sorts of problems
            GotNan = true;
            return 0;
        }
        GotNan = false;
        return Angle;
    }


    private Vector3 GetHint(BodySolver Solver)
    {
        if (UseExternalHint)
        {
            return _ExternalHint;
        }
        else
        {
            return CalculateHint(Solver);
        }
    }

    //only to be used when UseExternalHint == True.
    //hint will be used for all calls of Update after SetHint is called until replaced,
    //but may be a frame behind if Update has already been called on the same frame.
    //a solution could be to inherit BodyPartSolver to set your hint before the 
    //elbow solver runs, only technically existing as a solver in the BodySolver 
    public void SetHint(Vector3 hint)
    {
        _ExternalHint = hint;
    }

    delegate Tuple<Vector3, float> HintGenerator(BodySolver Solver);
    private Vector3 CalculateHint(BodySolver Solver)
    {
        List<Vector3> potentialHints = new();
        List<float> hintConfidences = new();
        List<HintGenerator> hintGenerators = new(){
            WristHint,
            ShoulderWristMeanHint
        };

        foreach (HintGenerator g in hintGenerators)
        {
            Tuple<Vector3, float> hint = g(Solver);
            potentialHints.Add(hint.Item1);
            hintConfidences.Add(hint.Item2);
        }

        return WeightedMeanV3(potentialHints, hintConfidences);
    }

    //https://en.wikipedia.org/wiki/Weighted_arithmetic_mean
    private Vector3 WeightedMeanV3(List<Vector3> points, List<float> weights)
    {
        if (points.Count == 0)
        {
            throw new ArgumentException("Zero (0) points given to WeightedMeanV3");
        }
        if (weights.Count == 0)
        {
            throw new ArgumentException("Zero (0) weights given to WeightedMeanV3");
        }
        if (points.Count != weights.Count)
        {
            throw new ArgumentException("points.Count != weights.Count in WeightedMeanV3");
        }


        Vector3 result = Vector3.Zero;
        float totalWeight = 0.0f;

        for (int i = 0; i < points.Count; i++)
        {
            totalWeight += weights[i];
            result += points[i] * weights[i];
        }

        return result / totalWeight;
    }


    #region Hint Generators
    private Tuple<Vector3, float> WristHint(BodySolver Solver)
    {
        GetWristPose(Solver, out Vector3 wristPos, out Basis wristBas);
        Vector3 hint = wristPos + (wristBas * Vector3.Back * VRUserMeasurements.Forearm);

        //TODO confidence estimation
        return new(hint, 2);
    }

    private Tuple<Vector3, float> ShoulderWristMeanHint(BodySolver Solver)
    {
        GetWristPose(Solver, out Vector3 wristPos, out Basis wristBas);
        GetShoulderPose(Solver, out Vector3 shoulderPos, out Basis shoulderBas);
        Vector3 hint = (wristPos + shoulderPos) / 2;
        hint += Solver.GetBodyDirection() * ShoulderWristMeanHintOffset;

        //TODO confidence estimation
        return new(hint, 1);
    }
    #endregion


    //helper functions. handedness makes some calls ugly, but these are nice
    private void GetWristPose(BodySolver Solver, out Vector3 wristPos, out Basis wristBas)
    {
        if (Lefthanded)
        {
            wristPos = Solver.GetLWristPos();
            wristBas = Solver.GetLWristBas();
        }
        else
        {
            wristPos = Solver.GetRWristPos();
            wristBas = Solver.GetRWristBas();
        }
    }
    private void GetShoulderPose(BodySolver Solver, out Vector3 shoulderPos, out Basis shoulderBas)
    {
        if (Lefthanded)
        {
            shoulderPos = ((ILShoulderSolver)ShoulderSolver).GetLShoulderPos();
            shoulderBas = ((ILShoulderSolver)ShoulderSolver).GetLShoulderBas();
        }
        else
        {
            shoulderPos = ((IRShoulderSolver)ShoulderSolver).GetRShoulderPos();
            shoulderBas = ((IRShoulderSolver)ShoulderSolver).GetRShoulderBas();
        }
    }


    private void SolveShoulder()
    {
        if (Lefthanded)
        {
            _ShoulderPos = ((ILShoulderSolver)ShoulderSolver).GetLShoulderPos();
        }
        else
        {
            _ShoulderPos = ((IRShoulderSolver)ShoulderSolver).GetRShoulderPos();
        }
        _ShoulderBas = CalculateShoulderBasis();
    }
    private Basis CalculateShoulderBasis()
    {
        return Basis.LookingAt(_ElbowPos - _ShoulderPos);
    }

    #region Getters
    public Vector3 GetLElbowPos()
    {
        return _ElbowPos;
    }
    public Basis GetLElbowBas()
    {
        return _ElbowBas;
    }
    public Vector3 GetRElbowPos()
    {
        return _ElbowPos;
    }
    public Basis GetRElbowBas()
    {
        return _ElbowBas;
    }


    public Vector3 GetLShoulderPos()
    {
        return _ShoulderPos;
    }
    public Basis GetLShoulderBas()
    {
        return _ShoulderBas;
    }

    public Vector3 GetRShoulderPos()
    {
        return _ShoulderPos;
    }
    public Basis GetRShoulderBas()
    {
        return _ShoulderBas;
    }
    #endregion
}
