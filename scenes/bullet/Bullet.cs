using Godot;
using System;

public partial class Bullet : Area2D
{
    int speed = 750;

    public void MoveProjectile(double delta) {
        Position += Transform.X * speed * (float)delta;
    }

    public override async void _Ready()
    {
        await ToSignal(GetTree().CreateTimer(4.0f), "timeout");
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveProjectile(delta);
    }
}
