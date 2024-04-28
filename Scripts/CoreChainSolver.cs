using Godot;
using System;

interface ICoreChainSolver :
	INeckSolver,
	IChestSolver,
	ISpineSolver,
    IBodyDirectionSolver
{ }

[GlobalClass]
public partial class CoreChainSolver : BodyPartSolver, ICoreChainSolver
{
	//the offset between the neck and the eyes, relative to the eyes' position and basis
	[Export] private Vector3 _NeckEyesOffset;
    [Export] private Vector3 _ChestNeckOffset;
    [Export] private Vector3 _SpineChestDirection;

    [ExportCategory("Neck Bend settings")]
    [Export] private Curve EyeAngleNeckBend;
    [Export] private Curve CrouchPercentageNeckBend;

    [ExportCategory("Chest Bend settings")]
    [Export] private Curve EyeAngleChestBend;
    [Export] private Curve CrouchPercentageChestBend;

    //the direction which the body is facing expressed as a basis
    private Basis _BodyDirection;

    //the position and basis of the neck relative to the camera rig
    private Vector3 _NeckPos;
	private Basis _NeckBas;

    //the position and basis of the chest relative to the camera rig
    private Vector3 _ChestPos;
    private Basis _ChestBas;

    //the position and basis of the spine relative to the camera rig
    private Vector3 _SpinePos;
    private Basis _SpineBas;


    public override void Update(BodySolver Solver)
	{
        //solve for each part of the chain in turn
        SolveBodyDirection(Solver);
		SolveNeck(Solver);
        SolveChest(Solver);
        SolveSpine(Solver);
    }

    private void SolveBodyDirection(BodySolver Solver)
    {   
        //get a vector3 of length 1 in the direction of the eyes' right vector
        //the right vector doesn't change if the player looks beyond straight up, 
        //and usually stays on the correct side of straight up/down to be a good
        //indicator of the body's forward direction
        Vector3 bodyRight = Solver.GetEyesBas() * Vector3.Right;
        bodyRight.Y = 0;
        bodyRight = bodyRight.Normalized();
        Vector3 bodyForward = Vector3.Up.Cross(bodyRight).Normalized();

        _BodyDirection = Basis.LookingAt(bodyForward, Vector3.Up);
    }

	//solves for the neck's position and basis
	private void SolveNeck(BodySolver Solver)
	{
        //get the eyes' position and basis
        Vector3 eyePos = Solver.GetEyesPos();
        Basis eyeBas = Solver.GetEyesBas();

        //move back and keep the rotation
        _NeckPos = eyePos + (eyeBas * _NeckEyesOffset);
        _NeckBas = Basis.LookingAt(eyePos - _NeckPos, Vector3.Up);
    }

    //solves for the chest's position and basis
    private void SolveChest(BodySolver Solver)
    {
        Vector3 neckPos = Solver.GetNeckPos();
        Basis neckBas = Solver.GetNeckBas();

        Vector3 bodyForward = Solver.GetBodyDirection() * Vector3.Forward;
        Basis neckEyesBendBasis = CalculateEyeBend(Solver, EyeAngleNeckBend);
        Basis neckCrouchBendbasis = CalculateCrouchBend(Solver, CrouchPercentageNeckBend);


        //calculate the final position and basis of the chest
        Basis chestBasisOffset = neckEyesBendBasis * neckCrouchBendbasis;
        _ChestBas = Basis.LookingAt(Vector3.Up, bodyForward) * chestBasisOffset;
        _ChestPos = neckPos + (_ChestBas * _ChestNeckOffset);
    }

    private void SolveSpine(BodySolver Solver)
    {
        Vector3 chestPos = Solver.GetChestPos();
        Basis chestBas = Solver.GetChestBas();

        Vector3 bodyForward = Solver.GetBodyDirection() * Vector3.Forward;
        Basis chestEyesBendBasis = CalculateEyeBend(Solver, EyeAngleChestBend);
        Basis chestCrouchBendBasis = CalculateCrouchBend(Solver, CrouchPercentageChestBend);


        //calculate the final position and basis of the spine
        Basis spineBasisOffset = chestEyesBendBasis * chestCrouchBendBasis;
        float length = GetTree().Root.GetNode<MeasurementsAutoload>("VRUserMeasurements").Spine * 0.5f;
        _SpineBas = Basis.LookingAt(Vector3.Up, bodyForward) * spineBasisOffset;
        _SpinePos = chestPos + (_SpineBas * _SpineChestDirection.Normalized() * length);
    }

    //calculates the rotation of a body part based on the rotation of the eyes
    public Basis CalculateEyeBend(BodySolver Solver, Curve BendCurve)
    {
        Vector3 bodyForward = Solver.GetBodyDirection() * Vector3.Forward;
        Vector3 bodyRight = Solver.GetBodyDirection() * Vector3.Right;
        float eyeAngle = bodyForward.SignedAngleTo(Solver.GetEyesBas() * Vector3.Forward, bodyRight);
        float eyeAngle01 = Mathf.Remap(eyeAngle, -Mathf.Pi, Mathf.Pi, 0, 1);
        float neckBendAngle = BendCurve.Sample(eyeAngle01);
        Basis eyeBendBasis = new Basis(Vector3.Right, neckBendAngle);

        return eyeBendBasis;
    }
    //calculates the rotation of a body part based on how crouched the user is
    public Basis CalculateCrouchBend(BodySolver Solver, Curve BendCurve)
    {
        float crouch = CalculateCrouchPercentage(Solver);
        float crouchBendAngle = BendCurve.Sample(crouch);
        return new Basis(Vector3.Right, crouchBendAngle);
    }
    //calculates how crouched the user is
    public float CalculateCrouchPercentage(BodySolver Solver)
    {
        MeasurementsAutoload measurements = GetTree().Root.GetNode<MeasurementsAutoload>("VRUserMeasurements");
        float currentHeight = (Solver.GetEyesPos().Y + measurements.TrackedOffset) + 0.08f;
        float fullHeight = measurements.PlayerHeight;

        return Mathf.Clamp(currentHeight / fullHeight, 0, 1);
    }

    #region Getters
    public Vector3 GetNeckPos()
	{
		return _NeckPos;
	}
	public Basis GetNeckBas()
	{
		return _NeckBas;
	}

    public Vector3 GetChestPos()
    {
        return _ChestPos;
    }
    public Basis GetChestBas()
    {
        return _ChestBas;
    }

    public Vector3 GetSpinePos()
    {
        return _SpinePos;
    }
    public Basis GetSpineBas()
    {
        return _SpineBas;
    }

    public Basis GetBodyDirection()
    {
        return _BodyDirection;
    }
    #endregion
}
