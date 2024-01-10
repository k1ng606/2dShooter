extends Area2D

signal bullet_hit
signal enemy_hit_goal
signal player_hit
var rng = RandomNumberGenerator.new()

func _ready():
	connect("area_entered", on_area_entered)
	connect("body_entered", on_body_entered)


func on_body_entered(_body):
	player_hit.emit()

func on_area_entered(area):
	bullet_hit.emit()
	area.queue_free()
	queue_free()
	
	
	
func _process(delta):
	var random_speed = rng.randi_range(1, 5)
	position.x -= 100 * delta * random_speed
	
	if (position.x < -50):
		enemy_hit_goal.emit()
		queue_free()
