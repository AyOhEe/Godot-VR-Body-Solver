using Godot;
using System;

//solves for the eyes and wrists based off of the CameraRig for the given BodySolver
public partial class CameraRigSolver : BodyPartSolver, IEyesSolver, ILWristSolver, IRWristSolver
{
 
    //position and basis of the eyes relative to the camera rig
    private Vector3 _EyesPos;
    private Basis _EyesBas;

    //position and basis of the left wrist relative to the camera rig
    private Vector3 _LWristPos;
    private Basis _LWristBas;

    //position and basis of the right wrist relative to the camera rig
    private Vector3 _RWristPos;
    private Basis _RWristBas;

    public override void Update(BodySolver Solver)
    {
        //hold a reference to the camera rig for shorter referencing
        CameraRig CameraRig = Solver.CameraRig;

        //store the position and basis of the camera relative to the camera rig
        _EyesPos = CameraRig.Camera.Position;
        _EyesBas = CameraRig.Camera.Basis;

        //store the position and basis of the left wrist relative to the camera rig
        _LWristPos = CameraRig.ToLocal(CameraRig.LeftWrist.GlobalPosition);
        _LWristBas = (CameraRig.Transform.Inverse() * CameraRig.LeftWrist.GlobalTransform).Orthonormalized().Basis;

        //store the position and basis of the right wrist relative to the camera rig
        _RWristPos = CameraRig.ToLocal(CameraRig.RightWrist.GlobalPosition);
        _RWristBas = (CameraRig.Transform.Inverse() * CameraRig.RightWrist.GlobalTransform).Orthonormalized().Basis;
    }

    #region Getters
    public Vector3 GetEyesPos()
    {
        return _EyesPos;
    }
    public Basis GetEyesBas()
    {
        return _EyesBas;
    }

    public Vector3 GetLWristPos()
    {
        return _LWristPos;
    }
    public Basis GetLWristBas()
    {
        return _LWristBas;
    }

    public Vector3 GetRWristPos()
    {
        return _RWristPos;
    }
    public Basis GetRWristBas()
    {
        return _RWristBas;
    }
    #endregion
}
