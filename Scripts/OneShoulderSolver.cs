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
        Vector3 bodyForward = CalculateBodyForward(Solver, out Vector3 bodyRight);
        Basis bodyOrientation = new Basis(bodyRight, Vector3.Up, bodyForward);
        ShoulderPos = Solver.GetChestPos() + (bodyOrientation * _ShoulderDirection * _ShoulderLength);
        ShoulderBas = bodyOrientation;
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
}