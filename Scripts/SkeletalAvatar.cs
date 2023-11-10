using Godot;
using System;

public partial class SkeletalAvatar : Node
{
	[Export] public BodySolver Solver;

    [ExportGroup("Body Parts")]
    [ExportSubgroup("Core Chain")]
    [Export] private Node3D Eyes;
    [Export] private Node3D Neck;
    [Export] private Node3D Chest;
    [Export] private Node3D Spine;

    [ExportSubgroup("Left Arm")]
    [Export] private Node3D LShoulder;
    [Export] private Node3D LElbow;
    [Export] private Node3D LWrist;

    [ExportSubgroup("Right Arm")]
    [Export] private Node3D RShoulder;
    [Export] private Node3D RElbow;
    [Export] private Node3D RWrist;

    [ExportSubgroup("Left Leg")]
    [Export] private Node3D LHips;
    [Export] private Node3D LKnee;
    [Export] private Node3D LAnkle;
    [Export] private Node3D LToe;

    [ExportSubgroup("Right Leg")]
    [Export] private Node3D RHips;
    [Export] private Node3D RKnee;
    [Export] private Node3D RAnkle;
    [Export] private Node3D RToe;

    public override void _Process(double delta)
    {
        Eyes.Transform = new Transform3D(Solver.GetEyesBas(), Solver.GetEyesPos());
        Neck.Transform = new Transform3D(Solver.GetNeckBas(), Solver.GetNeckPos());
        Chest.Transform = new Transform3D(Solver.GetChestBas(), Solver.GetChestPos());
        Spine.Transform = new Transform3D(Solver.GetSpineBas(), Solver.GetSpinePos());

        
        LShoulder.Transform = new Transform3D(Solver.GetLShoulderBas(), Solver.GetLShoulderPos());
        LElbow.Transform = new Transform3D(Solver.GetLElbowBas(), Solver.GetLElbowPos());
        LWrist.Transform = new Transform3D(Solver.GetLWristBas(), Solver.GetLWristPos());

        RShoulder.Transform = new Transform3D(Solver.GetRShoulderBas(), Solver.GetRShoulderPos());
        RElbow.Transform = new Transform3D(Solver.GetRElbowBas(), Solver.GetRElbowPos());
        RWrist.Transform = new Transform3D(Solver.GetRWristBas(), Solver.GetRWristPos());


        LHips.Transform = new Transform3D(Solver.GetLHipsBas(), Solver.GetLHipsPos());
        LKnee.Transform = new Transform3D(Solver.GetLKneeBas(), Solver.GetLKneePos());
        LAnkle.Transform = new Transform3D(Solver.GetLAnkleBas(), Solver.GetLAnklePos());
        LToe.Transform = new Transform3D(Solver.GetLToeBas(), Solver.GetLToePos());

        RHips.Transform = new Transform3D(Solver.GetRHipsBas(), Solver.GetRHipsPos());
        RKnee.Transform = new Transform3D(Solver.GetRKneeBas(), Solver.GetRKneePos());
        RAnkle.Transform = new Transform3D(Solver.GetRAnkleBas(), Solver.GetRAnklePos());
        RToe.Transform = new Transform3D(Solver.GetRToeBas(), Solver.GetRToePos());
    }
}
