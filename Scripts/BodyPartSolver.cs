using Godot;
using System;

[GlobalClass]
public abstract partial class BodyPartSolver : Node
{
    public abstract void Update(BodySolver Solver);
}

//all body points
//eyes
//neck
//chest
//R/L shoulder
//R/L elbow
//R/L wrist
//spine
//hips
//R/L knee
//R/L ankle
//R/L toe
//R/L toe end

//TODO: hand posing support for this system


//interfaces for each body part
// Get{Part}Pos returns a Vector3 representing where the body part is relative to the body solver
// Get{Part}Bas returns an orthonormalised Basis representing the rotation of the body part
//              relative to the body solver


//central bone chain
public interface IEyesSolver
{
    public Vector3 GetEyesPos();
    public Basis GetEyesBas();
}
public interface INeckSolver
{
    public Vector3 GetNeckPos();
    public Basis GetNeckBas();
}
public interface IChestSolver
{
    public Vector3 GetChestPos();
    public Basis GetChestBas();
}
public interface ISpineSolver
{
    public Vector3 GetSpinePos();
    public Basis GetSpineBas();
}
public interface IBodyDirectionSolver
{
    public Basis GetBodyDirection();
}

//shoulders
public interface ILShoulderSolver
{
    public Vector3 GetLShoulderPos();
    public Basis GetLShoulderBas();
}
public interface IRShoulderSolver
{
    public Vector3 GetRShoulderPos();
    public Basis GetRShoulderBas();
}
//elbows
public interface ILElbowSolver
{
    public Vector3 GetLElbowPos();
    public Basis GetLElbowBas();
}
public interface IRElbowSolver
{
    public Vector3 GetRElbowPos();
    public Basis GetRElbowBas();
}
//wrists
public interface ILWristSolver
{
    public Vector3 GetLWristPos();
    public Basis GetLWristBas();
}
public interface IRWristSolver
{
    public Vector3 GetRWristPos();
    public Basis GetRWristBas();
}

//hips
public interface ILHipsSolver
{
    public Vector3 GetLHipsPos();
    public Basis GetLHipsBas();
}
public interface IRHipsSolver
{
    public Vector3 GetRHipsPos();
    public Basis GetRHipsBas();
}

//knees
public interface ILKneeSolver
{
    public Vector3 GetLKneePos();
    public Basis GetLKneeBas();
}
public interface IRKneeSolver
{
    public Vector3 GetRKneePos();
    public Basis GetRKneeBas();
}

//ankles
public interface ILAnkleSolver
{
    public Vector3 GetLAnklePos();
    public Basis GetLAnkleBas();
}
public interface IRAnkleSolver
{
    public Vector3 GetRAnklePos();
    public Basis GetRAnkleBas();
}

//toes
public interface ILToeSolver
{
    public Vector3 GetLToePos();
    public Basis GetLToeBas();
}
public interface IRToeSolver
{
    public Vector3 GetRToePos();
    public Basis GetRToeBas();
}