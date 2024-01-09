extends Control


# Called when the node enters the scene tree for the first time.
func _ready():
	$VBoxContainer/Time.text = "Timer: " + str(Global.gameTime)
	$VBoxContainer/Score.text = "Score: " + str(Global.score)
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if Input.is_action_just_pressed("shoot"):
		get_tree().change_scene_to_file("res://scenes/level.tscn")
