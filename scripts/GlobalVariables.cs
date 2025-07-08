using Godot;
using System;

public partial class GlobalVariables : Node
{
    public static GlobalVariables Instance { get; private set; }

    public int score = 0;
    public int gameTime = 0;
    public int difficulty = 5;

    public override void _Ready()
    {
        Instance = this;
    }
}
