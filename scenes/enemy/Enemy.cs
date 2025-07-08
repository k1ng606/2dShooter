using Godot;
using System;

public partial class Enemy : Area2D
{
    [Signal] public delegate void BulletHitEventHandler();
    [Signal] public delegate void EnemyHitGoalEventHandler();
    [Signal] public delegate void PlayerHitEventHandler();

    [Export] public PackedScene explosion;

    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private int randomSpeed;

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));

        randomSpeed = rng.RandiRange(1, GlobalVariables.Instance.difficulty);
    }

    public void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("player"))
        {
            EmitSignal(nameof(PlayerHit));
        }
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("enemy"))
            return;

        EmitSignal(nameof(BulletHit));
        area.QueueFree();
        QueueFree();

        AnimatedSprite2D explosionInstance = (AnimatedSprite2D)explosion.Instantiate();
        explosionInstance.Position = Position;
        GetTree().CurrentScene.AddChild(explosionInstance);
    }

    public override void _Process(double delta)
    {
        Position += new Vector2(-100 * (float)delta * randomSpeed, 0);

        if (Position.X < -50)
        {
            EmitSignal(nameof(EnemyHitGoal));
            QueueFree();
        }
    }
}
