using Godot;
using System;


[GlobalClass]
public partial class TwoShoulderSolver : BodyPartSolver, ILShoulderSolver, IRShoulderSolver
{
    [Export] private OneShoulderSolver _LShoulderSolver;
    [Export] private OneShoulderSolver _RShoulderSolver;
    [Export] private BodyPartSolver _LElbowSolver;
    [Export] private BodyPartSolver _RElbowSolver;

    public override void _Ready()
    {
        _LShoulderSolver.Start();
        _RShoulderSolver.Start();
    }

    public override void Update(BodySolver Solver)
    {
        //prepare the elbow estimate
        _LShoulderSolver.PreUpdate(Solver);
        _RShoulderSolver.PreUpdate(Solver);
        _LElbowSolver.Update(Solver);
        _RElbowSolver.Update(Solver);

        //calculate the true shoulder position using the elbow estimate
        Transform3D LWrist = new Transform3D(Solver.GetLWristBas(), Solver.GetLWristPos());
        Transform3D LElbow = new Transform3D(Solver.GetLElbowBas(), Solver.GetLElbowPos());

        Transform3D RWrist = new Transform3D(Solver.GetRWristBas(), Solver.GetRWristPos());
        Transform3D RElbow = new Transform3D(Solver.GetRElbowBas(), Solver.GetRElbowPos());

        _LShoulderSolver.Update(Solver, LWrist, LElbow);
        _RShoulderSolver.Update(Solver, RWrist, RElbow);

        //true elbow position will be calculated later by the body solver
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