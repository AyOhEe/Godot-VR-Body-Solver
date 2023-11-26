using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Net;

public struct TrackedPoint 
{
	public Vector3 Pos = default;
	public Vector3 Vel = default;
	public Vector3 Acc = default;
	
	public Basis Bas = default;
	public Quaternion AngVel = default;
	public Quaternion AngAcc = default;
	
	public TrackedPoint(
        Vector3 _pos = default, 
        Vector3 _vel = default, 
        Vector3 _acc = default, 
        Basis _bas = default, 
        Quaternion _angVel = default, 
        Quaternion _angAcc = default
    )
	{
        Pos = _pos;
        Vel = _vel;
        Acc = _acc;

        Bas = _bas;
        AngVel = _angVel;
        AngAcc = _angAcc;
	}
}

public struct Landmark 
{
    public Vector3 Posision;
    public float Visibility;
    public float Confidence;

    public Landmark(Vector3 _pos, float _vis, float _conf)
    {
        Posision = _pos;
        Visibility = _vis;
        Confidence = _conf;
    }
}


public partial class WebSolver : BodyPartSolver, IFullbodySolver
{
    [Export] public string Address = "127.0.0.1";
    [Export] public int Port = 8080;
    [Export] public int Framerate = 30;

    //core chain
    private TrackedPoint _Eyes = default;
    private TrackedPoint _Neck = default;
    private TrackedPoint _Chest = default;
    private TrackedPoint _Spine = default;
    private Basis _BodyForward = default;

    //left arm
    private TrackedPoint _LShoulder = default;
    private TrackedPoint _LElbow = default;
    private TrackedPoint _LWrist = default;

    //right arm
    private TrackedPoint _RShoulder = default;
    private TrackedPoint _RElbow = default;
    private TrackedPoint _RWrist = default;

    //left leg
    private TrackedPoint _LHips = default;
    private TrackedPoint _LKnee = default;
    private TrackedPoint _LAnkle = default;
    private TrackedPoint _LToe = default;

    //right leg
    private TrackedPoint _RHips = default;
    private TrackedPoint _RKnee = default;
    private TrackedPoint _RAnkle = default;
    private TrackedPoint _RToe = default;

    private HttpRequest _RequestNode;
    private Timer _RequestTimer;
    private bool _RequestInProgress;

    private List<MeshInstance3D> TrackedPointMeshes;


    private string _FullAddress { get { return string.Format("http://{0}:{1}/pose", Address, Port.ToString()); } }


    public override void _Ready()
    {
        _RequestNode = CreatePoseRequestNode();
        _RequestTimer = CreatePoseRequestTimer();

        CreateTPMeshes();
    }

    public override void Update(BodySolver Solver)
	{
		//extrapolate old data using the calculated velocity and acceleration
	
		//if request in progress
		//    use old data, return
		
		//if request failed
		//    use old data, return
		
		//if request finished
		//    prepare new points, calculate point data
	}

    private HttpRequest CreatePoseRequestNode()
    {
        HttpRequest request = new HttpRequest();
        AddChild(request);
        request.RequestCompleted += GetPoseRequestFinished;

        return request;
    }

    private Timer CreatePoseRequestTimer() 
    {
        Timer timer = new Timer();
        AddChild(timer);

        timer.WaitTime = 1f / Framerate;
        timer.OneShot = false;
        timer.Timeout += SendPoseRequest;
        timer.Start();

        return timer;
    }
	
	private void GetPoseRequestFinished(long result, long responseCode, string[] headers, byte[] body)
	{
        HttpRequest.Result resultCode = (HttpRequest.Result)result;
        _RequestInProgress = false;
		if (resultCode != HttpRequest.Result.Success)
		{
			GD.PushError(string.Format("<<{1}>> Couldn't get pose from server {0} ", _FullAddress, resultCode.ToString()));
            return;
		}
		GD.Print(string.Format("Recieved pose from server {0}", _FullAddress));
		
		
		var json = new Json();
		json.Parse(body.GetStringFromUtf8());
		var response = json.Data.AsGodotDictionary();
        List<Landmark> Landmarks = ProcessLandmarks(response["landmarks"].AsGodotArray<Dictionary>());

        VisualisePoints(Landmarks);
	}

    private List<Landmark> ProcessLandmarks(Array<Dictionary> RawLandmarks) 
    {
        List<Landmark> processed = new List<Landmark>();
        foreach(Dictionary rawLandmark in RawLandmarks)
        {
            float x = (float)rawLandmark["x"].AsDouble();
            float y = (float)rawLandmark["y"].AsDouble();
            float z = (float)rawLandmark["z"].AsDouble();
            float vis = (float)rawLandmark["vis"].AsDouble();
            float pres = (float)rawLandmark["pres"].AsDouble();

            processed.Add(new Landmark(new Vector3(-x, -y + 0.5f, z), vis, pres));
        }

        return processed;
    }

    private void CreateTPMeshes() 
    {
        SphereMesh SmallSphere = new SphereMesh();
        SmallSphere.Radius = 0.02f;
        SmallSphere.Height = 2 * SmallSphere.Radius;

        TrackedPointMeshes = new List<MeshInstance3D>();
        for (int i = 0; i < 34; i++) 
        {
            TrackedPointMeshes.Add(new MeshInstance3D());
            AddChild(TrackedPointMeshes[i]);
            TrackedPointMeshes[i].Mesh = SmallSphere;
        }
    }
    private void VisualisePoints(List<Landmark> Landmarks) 
    {
        for (int i = 0; i < Landmarks.Count; i++) 
        {
            TrackedPointMeshes[i].Position = Landmarks[i].Posision;
        }
    }

    private void SendPoseRequest() 
    {
        if (_RequestInProgress) 
        {
            return;
        }

        Error error = _RequestNode.Request(_FullAddress);
        _RequestInProgress = true;
        if (error == Error.Busy)
        {
            //ignore this case. we'll send another request later.
        }
        else if (error != Error.Ok)
        {
            GD.PushError(string.Format("Couldn't request new pose from server {0}", _FullAddress));
        }
    }

    #region getters
    //core chain
    public Vector3 GetEyesPos()
    {
        return _Eyes.Pos;
    }
    public Vector3 GetNeckPos()
    {
        return _Neck.Pos;
    }
    public Vector3 GetChestPos()
    {
        return _Chest.Pos;
    }
    public Vector3 GetSpinePos()
    {
        return _Spine.Pos;
    }
    public Basis GetEyesBas()
    {
        return _Eyes.Bas;
    }
    public Basis GetNeckBas()
    {
        return _Neck.Bas;
    }
    public Basis GetChestBas()
    {
        return _Chest.Bas;
    }
    public Basis GetSpineBas()
    {
        return _Spine.Bas;
    }
    public Basis GetBodyDirection()
    {
        return _BodyForward;
    }

    //left arm
    public Vector3 GetLShoulderPos()
    {
        return _LShoulder.Pos;
    }
    public Vector3 GetLElbowPos()
    {
        return _LElbow.Pos;
    }
    public Vector3 GetLWristPos()
    {
        return _LWrist.Pos;
    }
    public Basis GetLShoulderBas()
    {
        return _LShoulder.Bas;
    }
    public Basis GetLElbowBas()
    {
        return _LElbow.Bas;
    }
    public Basis GetLWristBas()
    {
        return _LWrist.Bas;
    }

    //right arm
    public Vector3 GetRShoulderPos()
    {
        return _RShoulder.Pos;
    }
    public Vector3 GetRElbowPos()
    {
        return _RElbow.Pos;
    }
    public Vector3 GetRWristPos()
    {
        return _RWrist.Pos;
    }
    public Basis GetRShoulderBas()
    {
        return _RShoulder.Bas;
    }
    public Basis GetRElbowBas()
    {
        return _RElbow.Bas;
    }
    public Basis GetRWristBas()
    {
        return _RWrist.Bas;
    }

    //left leg
    public Vector3 GetLHipsPos()
    {
        return _LHips.Pos;
    }
    public Vector3 GetLKneePos()
    {
        return _LKnee.Pos;
    }
    public Vector3 GetLAnklePos()
    {
        return _LAnkle.Pos;
    }
    public Vector3 GetLToePos()
    {
        return _LToe.Pos;
    }
    public Basis GetLHipsBas()
    {
        return _LHips.Bas;
    }
    public Basis GetLKneeBas()
    {
        return _LKnee.Bas;
    }
    public Basis GetLAnkleBas()
    {
        return _LAnkle.Bas;
    }
    public Basis GetLToeBas()
    {
        return _LToe.Bas;
    }

    //right leg
    public Vector3 GetRHipsPos()
    {
        return _RHips.Pos;
    }
    public Vector3 GetRKneePos()
    {
        return _RKnee.Pos;
    }
    public Vector3 GetRAnklePos()
    {
        return _RAnkle.Pos;
    }
    public Vector3 GetRToePos()
    {
        return _RToe.Pos;
    }
    public Basis GetRHipsBas()
    {
        return _RHips.Bas;
    }
    public Basis GetRKneeBas()
    {
        return _RKnee.Bas;
    }
    public Basis GetRAnkleBas()
    {
        return _RAnkle.Bas;
    }
    public Basis GetRToeBas()
    {
        return _RToe.Bas;
    }
    #endregion
}