using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class ElbowSolver : BodyPartSolver, ILElbowSolver, IRElbowSolver
{
    [Export] public bool UseExternalHint = false;
    [Export] public bool Lefthanded = false;

    [Export] public float ArmLength;
    [Export] public float ForearmLength;

    [Export] private Vector3 _HintOffset;


    private Vector3 _ExternalHint;

    private Vector3 _ElbowPos;
    private Basis _ElbowBas;


    public override void Update(BodySolver Solver)
    {
        Vector3 elbowHint = GetHint(Solver);

        SolveElbow(Solver, elbowHint);
    }

    private void SolveElbow(BodySolver Solver, Vector3 Hint)
    {
        GetWristPose(Solver, out Vector3 wristPos, out Basis wristBas);
        GetShoulderPose(Solver, out Vector3 shoulderPos, out Basis shoulderBas);

        //cosine law. we know all sides of the triangle, so we know all angles.
        //from there, we calculate a point - because of the hint, there's only one
        //valid solution
        float shoulderWristDist = wristPos.DistanceTo(shoulderPos);
        float shoulderAngle = CosineLawAngle(ArmLength, shoulderWristDist, ForearmLength, out _);
        Vector3 armPlaneNormal = CalculateArmPlaneNormal(wristPos, shoulderPos, Hint);
        Vector3 elbowDir = (wristPos - shoulderPos).Normalized().Rotated(armPlaneNormal, shoulderAngle);

        _ElbowPos = shoulderPos + (elbowDir * ArmLength);
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
        if(float.IsNaN(Angle)) 
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

    private Vector3 CalculateHint(BodySolver Solver)
    {
        GetWristPose(Solver, out Vector3 wristPos, out Basis wristBas);
        GetShoulderPose(Solver, out Vector3 shoulderPos, out Basis shoulderBas);

        //TODO this sucks
        Vector3 bodyForward = CalculateBodyForward(Solver, out Vector3 bodyRight);
        Basis bodyOrientation = new Basis(bodyRight, Vector3.Up, bodyForward);
        Vector3 bodySideVector = (Lefthanded ? Vector3.Left : Vector3.Right) + _HintOffset;
        return (bodyOrientation * bodySideVector) + ((wristPos + shoulderPos) / 2);
    }
    //calculates the direction the player's body is facing
    private Vector3 CalculateBodyForward(BodySolver Solver, out Vector3 bodyRight)
    {
        //get a vector3 of length 1 in the direction of the eyes' right vector
        //the right vector doesn't change if the player looks beyond straight up, 
        //and usually stays on the correct side of straight up/down to be a good
        //indicator of the body's forward direction
        bodyRight = Solver.GetEyesBas() * Vector3.Right;
        bodyRight.Y = 0;
        bodyRight = bodyRight.Normalized();
        Vector3 bodyForward = bodyRight.Cross(Vector3.Up).Normalized();

        return bodyForward;
    }


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
            shoulderPos = Solver.GetLShoulderPos();
            shoulderBas = Solver.GetLShoulderBas();
        }
        else
        {
            shoulderPos = Solver.GetRShoulderPos();
            shoulderBas = Solver.GetRShoulderBas();
        }
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
    #endregion
}
