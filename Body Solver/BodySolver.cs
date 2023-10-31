using Godot;
using System;

public partial class BodySolver : Node3D
{
	[Export]
	//The CameraRig this BodySolver will solve for
	private CameraRig CameraRig;


	//parent node for all solved points
    private Node3D _Skeleton;

	//spine/head bones
    private Node3D _Eyes;
	private Node3D _Head;
	private Node3D _Chest;
	private Node3D _Spine;
	private Node3D _Pelvis;

	//left arm chain bones
	private Node3D _LeftClavicle;
	private Node3D _LeftShoulder;
	private Node3D _LeftElbow;
    private Node3D _LeftWrist;

	//right arm chain bones
    private Node3D _RightClavicle;
    private Node3D _RightShoulder;
    private Node3D _RightElbow;
    private Node3D _RightWrist;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//get references to skeleton nodes
		_Skeleton = GetNode<Node3D>("Skeleton");

		_Eyes = _Skeleton.GetNode<Node3D>("Eyes");
		_Head = _Skeleton.GetNode<Node3D>("Head");
        _Chest = _Skeleton.GetNode<Node3D>("Chest");
        _Spine = _Skeleton.GetNode<Node3D>("Spine");
        _Pelvis = _Skeleton.GetNode<Node3D>("Pelvis");

        _LeftClavicle = _Skeleton.GetNode<Node3D>("LeftClavicle");
        _LeftShoulder = _Skeleton.GetNode<Node3D>("LeftShoulder");
        _LeftElbow = _Skeleton.GetNode<Node3D>("LeftElbow");
        _LeftWrist = _Skeleton.GetNode<Node3D>("LeftWrist");

        _RightClavicle = _Skeleton.GetNode<Node3D>("RightClavicle");
        _RightShoulder = _Skeleton.GetNode<Node3D>("RightShoulder");
        _RightElbow = _Skeleton.GetNode<Node3D>("RightElbow");
        _RightWrist = _Skeleton.GetNode<Node3D>("RightWrist");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CameraRig == null)
		{
			throw new Exception("BodySolver has unfilled CameraRig Export! Cannot solve player body!");
		}

		//TODO get rid of this in favour of solvers - this is temporary
		_Eyes.Position = CameraRig.Camera.Position;
		_Eyes.Rotation = CameraRig.Camera.Rotation;

		_LeftWrist.Position = CameraRig.ToLocal(CameraRig.LeftWrist.GlobalPosition);
		_LeftWrist.Basis = (CameraRig.Transform.Inverse() * CameraRig.LeftWrist.Transform).Orthonormalized().Basis;

		_RightWrist.Position = CameraRig.ToLocal(CameraRig.RightWrist.GlobalPosition);
		_RightWrist.Basis = (CameraRig.Transform.Inverse() * CameraRig.RightWrist.Transform).Orthonormalized().Basis;
    }
}
