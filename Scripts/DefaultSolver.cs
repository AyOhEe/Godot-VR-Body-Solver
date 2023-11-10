using Godot;
using System;

public partial class DefaultSolver : BodyPartSolver, IFullbodySolver
{
	public override void Update(BodySolver Solver)
	{

	}

    #region getters
    //core chain
    public Vector3 GetEyesPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetNeckPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetChestPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetSpinePos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetLHipsPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetRHipsPos()
    {
        return Vector3.Zero;
    }
    public Basis GetEyesBas()
    {
        return Basis.Identity;
    }
    public Basis GetNeckBas()
    {
        return Basis.Identity;
    }
    public Basis GetChestBas()
    {
        return Basis.Identity;
    }
    public Basis GetSpineBas()
    {
        return Basis.Identity;
    }
    public Basis GetLHipsBas()
    {
        return Basis.Identity;
    }
    public Basis GetRHipsBas()
    {
        return Basis.Identity;
    }

    //left arm
    public Vector3 GetLShoulderPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetLElbowPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetLWristPos()
    {
        return Vector3.Zero;
    }
    public Basis GetLShoulderBas()
    {
        return Basis.Identity;
    }
    public Basis GetLElbowBas()
    {
        return Basis.Identity;
    }
    public Basis GetLWristBas()
    {
        return Basis.Identity;
    }

    //right arm
    public Vector3 GetRShoulderPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetRElbowPos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetRWristPos()
    {
        return Vector3.Zero;
    }
    public Basis GetRShoulderBas()
    {
        return Basis.Identity;
    }
    public Basis GetRElbowBas()
    {
        return Basis.Identity;
    }
    public Basis GetRWristBas()
    {
        return Basis.Identity;
    }

    //left leg
    public Vector3 GetLKneePos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetLAnklePos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetLToePos()
    {
        return Vector3.Zero;
    }
    public Basis GetLKneeBas()
    {
        return Basis.Identity;
    }
    public Basis GetLAnkleBas()
    {
        return Basis.Identity;
    }
    public Basis GetLToeBas()
    {
        return Basis.Identity;
    }

    //right leg
    public Vector3 GetRKneePos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetRAnklePos()
    {
        return Vector3.Zero;
    }
    public Vector3 GetRToePos()
    {
        return Vector3.Zero;
    }
    public Basis GetRKneeBas()
    {
        return Basis.Identity;
    }
    public Basis GetRAnkleBas()
    {
        return Basis.Identity;
    }
    public Basis GetRToeBas()
    {
        return Basis.Identity;
    }
    #endregion
}
