extends Node2D

var bullet: PackedScene = preload("res://scenes/bullet.tscn")
var enemy: PackedScene = preload("res://scenes/enemy.tscn")
var player: PackedScene = preload("res://scenes/player.tscn")

@export var fpsLabel: Label
@export var scoreLabel: Label
@export var timerLabel: Label
@export var enemyWinLabel: Label

@export var gameTimer: Timer
@export var enemySpawnTimer: Timer
@export var shootTimer: Timer

var canShoot = true

var score: int = 0
var gameTime: int = 0
var enemyHitGoal: int = 0

var rng = RandomNumberGenerator.new()

func _process(_delta):
	fpsLabel.text = "FPS: " + str(Engine.get_frames_per_second())
	
func setup():
	scoreLabel.text = "Score: " + str(score)
	timerLabel.text = "Timer: " + str(gameTime)
	enemyWinLabel.text = "Enemy Win Counter: " + str(enemyHitGoal)
	spawn_player_and_setup_player_signals()
	enemySpawnTimer.timeout.connect(spawn_enemy)
	gameTimer.timeout.connect(on_game_timer_timeout)
	shootTimer.timeout.connect(on_shoot_timeout)
	

func _ready():
	setup()
	
func on_shoot_timeout():
	canShoot = true

func on_game_timer_timeout():
	gameTime = gameTime + 1
	timerLabel.text = "Timer: " + str(gameTime)
	
func spawn_enemy():
	var enemyInstance = enemy.instantiate()
	var random_y = rng.randi_range(50, get_viewport().size.y - 50)
	enemyInstance.position.x = get_viewport().size.x + 50
	enemyInstance.position.y = random_y
	add_child(enemyInstance)
	enemyInstance.bullet_hit.connect(on_bullet_hit)
	enemyInstance.enemy_hit_goal.connect(on_enemy_hit_goal)
	enemyInstance.player_hit.connect(on_player_hit)
	return enemyInstance
	
func on_player_hit():
	call_deferred("set_score_and_show_killscreen")
	

func set_score_and_show_killscreen():
	Global.gameTime = gameTime
	Global.score = score
	get_tree().change_scene_to_file("res://scenes/kill_screen.tscn")
	

func on_enemy_hit_goal():
	enemyHitGoal = enemyHitGoal + 1
	enemyWinLabel.text = "Enemy Win Counter: " + str(enemyHitGoal)
	if (enemyHitGoal == 5):
		Global.gameTime = gameTime
		Global.score = score
		get_tree().change_scene_to_file("res://scenes/kill_screen.tscn")
	
func on_bullet_hit():
	score = score + 1
	scoreLabel.text = "Score: " + str(score)

func spawn_player_and_setup_player_signals():
	var playerInstance = player.instantiate()
	add_child(playerInstance)
	playerInstance.position.x = 50
	playerInstance.position.y = get_viewport().size.y / 2
	playerInstance.player_fired.connect(on_player_fired)
	return playerInstance


func spawn_bullet():
	var bulletInstance = bullet.instantiate()
	add_child(bulletInstance)
	bulletInstance.transform = $player/bulletSpawnMarker.global_transform
	

func on_player_fired():
	if (canShoot):
		spawn_bullet()
		canShoot = false
	
