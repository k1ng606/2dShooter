using Godot;
using System;

public partial class Explosion : AnimatedSprite2D
{
	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished()
    {
        QueueFree(); 
    }
}
