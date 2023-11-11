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
interface IEyesSolver
{
    public Vector3 GetEyesPos();
    public Basis GetEyesBas();
}
interface INeckSolver
{
    public Vector3 GetNeckPos();
    public Basis GetNeckBas();
}
interface IChestSolver
{
    public Vector3 GetChestPos();
    public Basis GetChestBas();
}
interface ISpineSolver
{
    public Vector3 GetSpinePos();
    public Basis GetSpineBas();
}
interface ILHipsSolver
{
    public Vector3 GetLHipsPos();
    public Basis GetLHipsBas();
}
interface IRHipsSolver
{
    public Vector3 GetRHipsPos();
    public Basis GetRHipsBas();
}

//shoulders
interface ILShoulderSolver
{
    public Vector3 GetLShoulderPos();
    public Basis GetLShoulderBas();
}
interface IRShoulderSolver
{
    public Vector3 GetRShoulderPos();
    public Basis GetRShoulderBas();
}
//elbows
interface ILElbowSolver
{
    public Vector3 GetLElbowPos();
    public Basis GetLElbowBas();
}
interface IRElbowSolver
{
    public Vector3 GetRElbowPos();
    public Basis GetRElbowBas();
}
//wrists
interface ILWristSolver
{
    public Vector3 GetLWristPos();
    public Basis GetLWristBas();
}
interface IRWristSolver
{
    public Vector3 GetRWristPos();
    public Basis GetRWristBas();
}

//knees
interface ILKneeSolver
{
    public Vector3 GetLKneePos();
    public Basis GetLKneeBas();
}
interface IRKneeSolver
{
    public Vector3 GetRKneePos();
    public Basis GetRKneeBas();
}

//ankles
interface ILAnkleSolver
{
    public Vector3 GetLAnklePos();
    public Basis GetLAnkleBas();
}
interface IRAnkleSolver
{
    public Vector3 GetRAnklePos();
    public Basis GetRAnkleBas();
}

//toes
interface ILToeSolver
{
    public Vector3 GetLToePos();
    public Basis GetLToeBas();
}
interface IRToeSolver
{
    public Vector3 GetRToePos();
    public Basis GetRToeBas();
}