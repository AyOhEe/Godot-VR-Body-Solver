using Godot;
using System;


[GlobalClass]
public partial class TwoShoulderSolver : BodyPartSolver, ILShoulderSolver, IRShoulderSolver
{
    [Export] private OneShoulderSolver _LShoulderSolver;
    [Export] private OneShoulderSolver _RShoulderSolver;


    public override void Update(BodySolver Solver)
    {
        _LShoulderSolver.Update(Solver, Solver.GetLWristPos(), Solver.GetLWristBas());
        _RShoulderSolver.Update(Solver, Solver.GetRWristPos(), Solver.GetRWristBas());
    }


    #region Getters
    public Vector3 GetLShoulderPos()
    {
        return _LShoulderSolver.ShoulderPos;
    }
    public Basis GetLShoulderBas()
    {
        return _LShoulderSolver.ShoulderBas;
    }

    public Vector3 GetRShoulderPos() 
    {  
        return _RShoulderSolver.ShoulderPos; 
    }
    public Basis GetRShoulderBas() 
    {  
        return _RShoulderSolver.ShoulderBas; 
    }
    #endregion
}