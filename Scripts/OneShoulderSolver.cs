using Godot;

[GlobalClass]
public partial class OneShoulderSolver : Resource
{
    [Export] private Vector3 _ShoulderDirection = Vector3.Left;
    [Export] private float _ShoulderLength = 0.17f; //TODO move this to body definition

    public Vector3 ShoulderPos { get; private set; } = Vector3.Zero;
    public Basis ShoulderBas { get; private set; } = Basis.Identity;


    public void Update(BodySolver Solver, Vector3 WristPos, Basis WristBas)
    {
        ShoulderPos = Solver.GetChestPos() + (_ShoulderDirection * _ShoulderLength);
    }
}