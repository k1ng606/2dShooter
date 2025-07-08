extends Area2D

var speed = 750

func _ready():
	await get_tree().create_timer(4.0).timeout
	queue_free()

func move_projectile(delta):
	position += transform.x * speed * delta

func _physics_process(delta):
	move_projectile(delta)
