using System;
using Godot;
using HexGrid;

public partial class Game : Node2D
{
    Arena arena;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arena = GetNode<Arena>("Arena");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("quit"))
        {
            GetTree().Quit();
        }

        // Connections
        Shooter shooter = (Shooter)GetNode("Shooter");
        shooter.Hit += OnHit;
    }

    public void OnHit(Vector2 position, string color)
    {
        arena.AddBall(position, color);
    }
}
