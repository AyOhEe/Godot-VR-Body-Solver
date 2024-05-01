using Godot;

[GlobalClass]
public partial class OneShoulderSolver : Resource
{
    [Export] private Vector3 _ShoulderDirection = Vector3.Left;

    public Vector3 ShoulderPos { get; private set; } = Vector3.Zero;
    public Basis ShoulderBas { get; private set; } = Basis.Identity;

    public void Start()
    {
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

    }
}