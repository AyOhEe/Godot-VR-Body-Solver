using Godot;
using System;

[GlobalClass]
public partial class OneShoulderSolver : Resource
{
    [Export] private Vector3 _ShoulderDirection = Vector3.Left;

    //the input range of the curve, in multiples of VRUserMeasurements.Arm
    [Export] private Vector2 YDeltaRange;
    [Export] private Curve ThetaZFromY;

    //the input range of the curve, in multiples of VRUserMeasurements.Arm
    [Export] private Vector2 ZDeltaRange;
    [Export] private Curve ThetaYFromZ;

    public Vector3 ShoulderPos { get; private set; } = Vector3.Zero;
    public Basis ShoulderBas { get; private set; } = Basis.Identity;

    public void Start()
    {
        if (YDeltaRange.X >= YDeltaRange.Y)
        {
            throw new Exception("YDeltaRange start (X) must be lesser than end (Y)!");
        }
        if (ZDeltaRange.X >= ZDeltaRange.Y)
        {
            throw new Exception("ZDeltaRange start (X) must be lesser than end (Y)!");
        }

        //ensure the shoulder direction upon initialisation is normalised. other
        //calculations depend on this fact.
        _ShoulderDirection = _ShoulderDirection.Normalized();
    }

    //prepares the basic shoulder position for use with elbow estimation
    public void PreUpdate(BodySolver Solver)
    {
        ShoulderPos = Solver.GetChestPos() + (Solver.GetBodyDirection() * _ShoulderDirection * VRUserMeasurements.Clavicle);
        ShoulderBas = Solver.GetBodyDirection();
    }

    //calculates the actual shoulder position
    public void Update(BodySolver Solver, Transform3D Wrist, Transform3D Elbow)
    {
        Vector3 baseElbowPos = ShoulderPos + (Vector3.Down * VRUserMeasurements.Arm);
        Vector3 delta = Solver.GetBodyDirection().Inverse() * (Elbow.Origin - baseElbowPos);

        float thetaY = ThetaYFromZ.Sample(RemapDelta(delta.Z, ZDeltaRange));
        float thetaZ = ThetaZFromY.Sample(RemapDelta(delta.Y, YDeltaRange));

        Basis shoulderRotation = Solver.GetBodyDirection() * new Basis(Vector3.Forward, thetaZ) * new Basis(Vector3.Up, thetaY);
        ShoulderPos = Solver.GetChestPos() + (shoulderRotation * _ShoulderDirection * VRUserMeasurements.Clavicle);
    }

    public float RemapDelta(float delta, Vector2 deltaRange)
    {
        //delta ranges are provided in multiples of VRUserMeasurements.Arm
        //to account for variances in user size
        deltaRange.X *= VRUserMeasurements.Arm;
        deltaRange.Y *= VRUserMeasurements.Arm;

        float remappedDelta = (delta - deltaRange.X) / (deltaRange.Y - deltaRange.X);
        return (float)Mathf.Clamp(remappedDelta, 0.0, 1.0);
    }
}