using Godot;
using System;

interface ICoreChainSolver :
	INeckSolver,
	IChestSolver,
	ISpineSolver,
	ILHipsSolver,
	IRHipsSolver
{ }

public partial class CoreChainSolver : BodyPartSolver, ICoreChainSolver
{
	//the offset between the neck and the eyes, relative to the eyes' position and basis
	[Export] private Vector3 _NeckEyesOffset;
    [Export] private Vector3 _ChestNeckOffset;
    [Export] private Vector3 _SpineChestOffset;


	//the position and basis of the neck relative to the camera rig
	private Vector3 _NeckPos;
	private Basis _NeckBas;

    //the position and basis of the chest relative to the camera rig
    private Vector3 _ChestPos;
    private Basis _ChestBas;

    //the position and basis of the spine relative to the camera rig
    private Vector3 _SpinePos;
    private Basis _SpineBas;

    //the position and basis of the neck relative to the camera rig
    private Vector3 _LHipPos;
    private Basis _LHipBas;

    //the position and basis of the neck relative to the camera rig
    private Vector3 _RHipPos;
    private Basis _RHipBas;


    public override void Update(BodySolver Solver)
	{
		//solve for each part of the chain in turn
		SolveNeck(Solver);
        SolveChest(Solver);
        SolveSpine(Solver);
		SolveHips(Solver);
	}


	//solves for the neck's position and basis
	private void SolveNeck(BodySolver Solver)
	{
        //get the eyes' position and basis
        Vector3 eyePos = Solver.GetEyesPos();
        Basis eyeBas = Solver.GetEyesBas();

        //move back and keep the rotation
        Vector3 BodyForward = CalculateBodyForward(Solver);
        _NeckPos = eyePos + (eyeBas * _NeckEyesOffset);
        _NeckBas = Basis.LookingAt(eyePos - _NeckPos, Vector3.Up);
    }

    //solves for the chest's position and basis
    private void SolveChest(BodySolver Solver)
    {
        //get the neck's position and basis
        Vector3 neckPos = Solver.GetNeckPos();
        Basis neckBas = Solver.GetNeckBas();

        Vector3 BodyForward = CalculateBodyForward(Solver);
        _ChestPos = neckPos + _ChestNeckOffset;
        _ChestBas = Basis.LookingAt(neckPos - _ChestPos, BodyForward);
    }

    //TODO this needs to make more sense, perhaps adding a IBodyForward interface now could be a good idea
    //solves for the spine's position and basis
    private void SolveSpine(BodySolver Solver)
    {
        //get the chest's position and basis
        Vector3 chestPos = Solver.GetChestPos();
        Basis chestBas = Solver.GetChestBas();

        _SpinePos = chestPos + _SpineChestOffset;

        Vector3 bodyForward = CalculateBodyForward(Solver, out Vector3 neckRight);
        _SpineBas = new Basis(neckRight, Vector3.Up, bodyForward);
    }

    //solves for the position and basis of each hip
    private void SolveHips(BodySolver Solver)
    {

    }


    //calculates the direction the player's body is facing
    private Vector3 CalculateBodyForward(BodySolver Solver, out Vector3 neckRight)
    {
        //get a vector3 of length 1 in the direction of the neck's right vector
        //the right vector doesn't change if the player looks beyond straight up, 
        //and usually stays on the correct side of straight up/down to be a good
        //indicator of the body's forward direction
        neckRight = new Plane(Vector3.Up).Project(Vector3.Right * Solver.GetNeckBas()).Normalized();
        Vector3 bodyForward = neckRight.Cross(Vector3.Up).Normalized();

        return bodyForward;
    }
    //overload that does not require the {out Vector3 neckRight} parameter
    private Vector3 CalculateBodyForward(BodySolver Solver)
    {
        return CalculateBodyForward(Solver, out _);
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

    public Vector3 GetLHipsPos()
    {
        return _LHipPos;
    }
    public Basis GetLHipsBas()
    {
        return _LHipBas;
    }

    public Vector3 GetRHipsPos()
    {
        return _RHipPos;
    }
    public Basis GetRHipsBas()
    {
        return _RHipBas;
    }
    #endregion
}
