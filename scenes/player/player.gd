extends CharacterBody2D

@export var speed = 400

signal player_fired

func get_input():
	if Input.is_action_just_pressed("shoot"):
		player_fired.emit()
		
	var input_direction = Input.get_vector("left", "right", "up", "down")
	input_direction = Vector2(0, input_direction.y)
	velocity = input_direction * speed
	
	if (position.y > get_viewport().size.y - 50):
		position.y = get_viewport().size.y - 50
		
	if (position.y < 50):
		position.y = 50

func _physics_process(_delta):
	get_input()
	move_and_slide()
