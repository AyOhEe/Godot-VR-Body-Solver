using Godot;
using Godot.Collections;
using System;

interface IFullbodySolver :
    IEyesSolver,
    INeckSolver,
    IChestSolver,
    ISpineSolver,
    IBodyDirectionSolver,

    ILShoulderSolver,
    ILElbowSolver,
    ILWristSolver,

    IRShoulderSolver,
    IRElbowSolver,
    IRWristSolver,

    ILHipsSolver,
    ILKneeSolver,
    ILAnkleSolver,
    ILToeSolver,

    IRHipsSolver,
    IRKneeSolver,
    IRAnkleSolver,
    IRToeSolver
{ }

public partial class BodySolver : Node, IFullbodySolver
{
    //The CameraRig this BodySolver will solve for
    [Export] public CameraRig CameraRig;
	//the solvers that solve for one or many solved points, in order of updating
	[Export] private Array<BodyPartSolver> _BodyPartSolvers;

    [ExportGroup("Body Parts")]
    [ExportSubgroup("Core Chain")]
    [Export] private BodyPartSolver EyesSolver;
    [Export] private BodyPartSolver NeckSolver;
    [Export] private BodyPartSolver ChestSolver;
    [Export] private BodyPartSolver SpineSolver;
    [Export] private BodyPartSolver BodyDirectionSolver;

    [ExportSubgroup("Left Arm")]
    [Export] private BodyPartSolver LShoulderSolver;
    [Export] private BodyPartSolver LElbowSolver;
    [Export] private BodyPartSolver LWristSolver;

    [ExportSubgroup("Right Arm")]
    [Export] private BodyPartSolver RShoulderSolver;
    [Export] private BodyPartSolver RElbowSolver;
    [Export] private BodyPartSolver RWristSolver;

    [ExportSubgroup("Left Leg")]
    [Export] private BodyPartSolver LHipsSolver;
    [Export] private BodyPartSolver LKneeSolver;
    [Export] private BodyPartSolver LAnkleSolver;
    [Export] private BodyPartSolver LToeSolver;

    [ExportSubgroup("Right Leg")]
    [Export] private BodyPartSolver RHipsSolver;
    [Export] private BodyPartSolver RKneeSolver;
    [Export] private BodyPartSolver RAnkleSolver;
    [Export] private BodyPartSolver RToeSolver;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CameraRig == null)
		{
			throw new Exception("BodySolver has unfilled CameraRig Export! Cannot solve player body!");
		}

        //iterate over each solver and process them
        foreach (BodyPartSolver solver in _BodyPartSolvers)
        {
            solver.Update(this);
        }
	}

    #region getters
    //this isn't the most elegant, but it makes getting solved points much
    //nicer for when we use them in AvatarMappers

    //core chain
    public Vector3 GetEyesPos()
    {
        return ((IEyesSolver)EyesSolver).GetEyesPos();
    }
    public Vector3 GetNeckPos()
    {
        return ((INeckSolver)NeckSolver).GetNeckPos();
    }
    public Vector3 GetChestPos()
    {
        return ((IChestSolver)ChestSolver).GetChestPos();
    }
    public Vector3 GetSpinePos()
    {
        return ((ISpineSolver)SpineSolver).GetSpinePos();
    }
    public Basis GetEyesBas()
    {
        return ((IEyesSolver)EyesSolver).GetEyesBas();
    }
    public Basis GetNeckBas()
    {
        return ((INeckSolver)NeckSolver).GetNeckBas();
    }
    public Basis GetChestBas()
    {
        return ((IChestSolver)ChestSolver).GetChestBas();
    }
    public Basis GetSpineBas()
    {
        return ((ISpineSolver)SpineSolver).GetSpineBas();
    }
    public Basis GetBodyDirection()
    {
        return ((IBodyDirectionSolver)BodyDirectionSolver).GetBodyDirection();
    }

    //left arm
    public Vector3 GetLShoulderPos()
    {
        return ((ILShoulderSolver)LShoulderSolver).GetLShoulderPos();
    }
    public Vector3 GetLElbowPos()
    {
        return ((ILElbowSolver)LElbowSolver).GetLElbowPos();
    }
    public Vector3 GetLWristPos()
    {
        return ((ILWristSolver)LWristSolver).GetLWristPos();
    }
    public Basis GetLShoulderBas()
    {
        return ((ILShoulderSolver)LShoulderSolver).GetLShoulderBas();
    }
    public Basis GetLElbowBas()
    {
        return ((ILElbowSolver)LElbowSolver).GetLElbowBas();
    }
    public Basis GetLWristBas()
    {
        return ((ILWristSolver)LWristSolver).GetLWristBas();
    }

    //right arm
    public Vector3 GetRShoulderPos()
    {
        return ((IRShoulderSolver)RShoulderSolver).GetRShoulderPos();
    }
    public Vector3 GetRElbowPos()
    {
        return ((IRElbowSolver)RElbowSolver).GetRElbowPos();
    }
    public Vector3 GetRWristPos()
    {
        return ((IRWristSolver)RWristSolver).GetRWristPos();
    }
    public Basis GetRShoulderBas()
    {
        return ((IRShoulderSolver)RShoulderSolver).GetRShoulderBas();
    }
    public Basis GetRElbowBas()
    {
        return ((IRElbowSolver)RElbowSolver).GetRElbowBas();
    }
    public Basis GetRWristBas()
    {
        return ((IRWristSolver)RWristSolver).GetRWristBas();
    }

    //left leg
    public Vector3 GetLHipsPos()
    {
        return ((ILHipsSolver)LHipsSolver).GetLHipsPos();
    }
    public Vector3 GetLKneePos()
    {
        return ((ILKneeSolver)LKneeSolver).GetLKneePos();
    }
    public Vector3 GetLAnklePos()
    {
        return ((ILAnkleSolver)LAnkleSolver).GetLAnklePos();
    }
    public Vector3 GetLToePos()
    {
        return ((ILToeSolver)LToeSolver).GetLToePos();
    }
    public Basis GetLHipsBas()
    {
        return ((ILHipsSolver)LHipsSolver).GetLHipsBas();
    }
    public Basis GetLKneeBas()
    {
        return ((ILKneeSolver)LKneeSolver).GetLKneeBas();
    }
    public Basis GetLAnkleBas()
    {
        return ((ILAnkleSolver)LAnkleSolver).GetLAnkleBas();
    }
    public Basis GetLToeBas()
    {
        return ((ILToeSolver)LToeSolver).GetLToeBas();
    }

    //right leg
    public Vector3 GetRHipsPos()
    {
        return ((IRHipsSolver)RHipsSolver).GetRHipsPos();
    }
    public Vector3 GetRKneePos()
    {
        return ((IRKneeSolver)RKneeSolver).GetRKneePos();
    }
    public Vector3 GetRAnklePos()
    {
        return ((IRAnkleSolver)RAnkleSolver).GetRAnklePos();
    }
    public Vector3 GetRToePos()
    {
        return ((IRToeSolver)RToeSolver).GetRToePos();
    }
    public Basis GetRHipsBas()
    {
        return ((IRHipsSolver)RHipsSolver).GetRHipsBas();
    }
    public Basis GetRKneeBas()
    {
        return ((IRKneeSolver)RKneeSolver).GetRKneeBas();
    }
    public Basis GetRAnkleBas()
    {
        return ((IRAnkleSolver)RAnkleSolver).GetRAnkleBas();
    }
    public Basis GetRToeBas()
    {
        return ((IRToeSolver)RToeSolver).GetRToeBas();
    }
    #endregion
}
