using Godot;
using System;

public partial class Level : Node2D
{
    [Export] public PackedScene Bullet;
    [Export] public PackedScene Enemy;
    [Export] public PackedScene Player;

    [Export] public int StartDifficulty = 5;

    [Export] public Label FpsLabel;
    [Export] public Label ScoreLabel;
    [Export] public Label TimerLabel;
    [Export] public Label EnemyWinLabel;

    [Export] public Timer GameTimer;
    [Export] public Timer EnemySpawnTimer;
    [Export] public Timer ShootTimer;

    private bool canShoot = true;

    private int score = 0;
    private int gameTime = 0;
    private int enemyHitGoal = 0;
    private int difficultyScaleTime = 0;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        Setup();
    }

    public override void _Process(double delta)
    {
        FpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
        ScaleDifficulty();
    }

    private void Setup()
    {
        ScoreLabel.Text = $"Score: {score}";
        TimerLabel.Text = $"Timer: {gameTime}";
        EnemyWinLabel.Text = $"Enemy Win Counter: {enemyHitGoal}";

        var playerInstance = SpawnPlayerAndSetupPlayerSignals();

        EnemySpawnTimer.Timeout += SpawnEnemy;
        GameTimer.Timeout += OnGameTimerTimeout;
        ShootTimer.Timeout += OnShootTimeout;
    }

    private void ScaleDifficulty()
    {
        if (difficultyScaleTime == 45)
        {
            GlobalVariables.Instance.difficulty = StartDifficulty;
            difficultyScaleTime = 0;
            StartDifficulty += 3;
        }
    }

    private void OnShootTimeout()
    {
        canShoot = true;
    }

    private void OnGameTimerTimeout()
    {
        gameTime += 1;
        difficultyScaleTime += 1;
        TimerLabel.Text = $"Timer: {gameTime}";
    }

    private void SpawnEnemy()
    {
        var enemyInstance = (Area2D)Enemy.Instantiate();
        var randomY = rng.RandiRange(50, (int)GetViewport().GetVisibleRect().Size.Y - 50);

        enemyInstance.Position = new Vector2(GetViewport().GetVisibleRect().Size.X + 50, randomY);
        AddChild(enemyInstance);

        enemyInstance.Connect("BulletHit", new Callable(this, nameof(OnBulletHit)));
        enemyInstance.Connect("EnemyHitGoal", new Callable(this, nameof(OnEnemyHitGoal)));
        enemyInstance.Connect("PlayerHit", new Callable(this, nameof(OnPlayerHit)));
    }

    private void OnPlayerHit()
    {
        CallDeferred(nameof(SetScoreAndShowKillScreen));
    }

    private void SetScoreAndShowKillScreen()
    {
        GlobalVariables.Instance.score = score;
        GlobalVariables.Instance.gameTime = gameTime;
        GlobalVariables.Instance.difficulty = 5;
        GetTree().ChangeSceneToFile("res://scenes/kill_screen.tscn");
    }

    private void OnEnemyHitGoal()
    {
        enemyHitGoal++;
        EnemyWinLabel.Text = $"Enemy Win Counter: {enemyHitGoal}";
        if (enemyHitGoal == 5)
        {
            CallDeferred(nameof(SetScoreAndShowKillScreen));
        }
    }

    private void OnBulletHit()
    {
        score++;
        ScoreLabel.Text = $"Score: {score}";
    }

    private Node2D SpawnPlayerAndSetupPlayerSignals()
    {
        var playerInstance = (Node2D)Player.Instantiate();
        AddChild(playerInstance);

        playerInstance.Position = new Vector2(75, GetViewport().GetVisibleRect().Size.Y / 2);

        playerInstance.Connect("PlayerFired", new Callable(this, nameof(OnPlayerFired)));
        return playerInstance;
    }

    private void SpawnBullet()
    {
        var bulletInstance = (Node2D)Bullet.Instantiate();
        AddChild(bulletInstance);

        // Assumes player has a node named "bulletSpawnMarker" under it
        var spawnMarker = GetNode<Node2D>("player/bulletSpawnMarker");
        bulletInstance.GlobalTransform = spawnMarker.GlobalTransform;
    }

    private void OnPlayerFired()
    {
        if (canShoot)
        {
            SpawnBullet();
            canShoot = false;
        }
    }
}