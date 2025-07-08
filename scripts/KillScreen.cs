using Godot;
using System;

public partial class KillScreen : Control
{
    public override void _Ready()
    {
        GetNode<Label>("VBoxContainer/Time").Text = "Timer: " + GlobalVariables.Instance.gameTime;
        GetNode<Label>("VBoxContainer/Score").Text = "Score: " + GlobalVariables.Instance.score;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("shoot"))
        {
            GetTree().ChangeSceneToFile("res://scenes/level.tscn");
        }
    }
}
