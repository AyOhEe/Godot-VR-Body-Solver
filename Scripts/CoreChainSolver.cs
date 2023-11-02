using Godot;
using System;

interface ICoreChainSolver :
	INeckSolver//,
	//IChestSolver,
	//ISpineSolver,
	//ILHipsSolver,
	//IRHipsSolver
{ }

public partial class CoreChainSolver : BodyPartSolver, ICoreChainSolver
{
	//the offset between the neck and the eyes, relative to the eyes' position and basis
	[Export] private Vector3 _NeckEyesOffset;


	//the position and basis of the neck relative to the camera rig
	private Vector3 _NeckPos;
	private Basis _NeckBas;

    //the position and basis of the spine relative to the camera rig
    private Vector3 _SpinePos;
    private Basis _SpineBas;

    //the position and basis of the chest relative to the camera rig
    private Vector3 _ChestPos;
    private Basis _ChestBas;

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
		SolveSpine(Solver);
		SolveChest(Solver);
		SolveHips(Solver);
	}


	//solves for the neck's position and basis
	private void SolveNeck(BodySolver Solver)
	{
        //get the eyes' position and basis
        Vector3 eyePos = Solver.GetEyesPos();
        Basis eyeBas = Solver.GetEyesBas();

        //move back and keep the rotation
        _NeckPos = eyePos + (eyeBas * _NeckEyesOffset);
        _NeckBas = eyeBas;
    }

    //solves for the spine's position and basis
    private void SolveSpine(BodySolver Solver)
	{

	}

    //solves for the chest's position and basis
    private void SolveChest(BodySolver Solver)
    {

    }

    //solves for the position and basis of each hip
    private void SolveHips(BodySolver Solver)
    {

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
    #endregion
}
