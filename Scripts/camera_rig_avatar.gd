extends Node3D

@export var body_solver : Node3D

@export var eyes : Node3D
@export var neck : Node3D
@export var chest : Node3D
@export var spine : Node3D
@export var l_wrist : Node3D
@export var r_wrist : Node3D

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	eyes.position = body_solver.GetEyesPos()
	eyes.basis = body_solver.GetEyesBas()
	
	neck.position = body_solver.GetNeckPos()
	neck.basis = body_solver.GetNeckBas()
	
	chest.position = body_solver.GetChestPos()
	chest.basis = body_solver.GetChestBas()
	
	spine.position = body_solver.GetSpinePos()
	spine.basis = body_solver.GetSpineBas()
	
	l_wrist.position = body_solver.GetLWristPos()
	l_wrist.basis = body_solver.GetLWristBas()
	
	r_wrist.position = body_solver.GetRWristPos()
	r_wrist.basis = body_solver.GetRWristBas()
