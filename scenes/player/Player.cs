using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    int speed = 400;

    [Signal]
    public delegate void PlayerFiredEventHandler();

    public void GetInput()
    {
        if (Input.IsActionJustPressed("shoot"))
        {
            EmitSignal(SignalName.PlayerFired);
        }

        Vector2 InputDirection = Input.GetVector("left", "right", "up", "down");
        InputDirection = new Vector2(0, InputDirection.Y);
        Velocity = InputDirection * speed;

        if (Position.Y > GetViewport().GetVisibleRect().Size.Y - 50)
        {
            Position = new Vector2(Position.X, GetViewport().GetVisibleRect().Size.Y - 50);
        }

        if (Position.Y < 50)
        {
            Position = new Vector2(Position.X, 50);
        }
    }

    public override void _PhysicsProcess(double _delta)
    {
        GetInput();
        MoveAndSlide();
    }
}
