using System;
using Godot;
using HexGrid;

public partial class Game : Node2D
{
    Arena arena;
    Shooter shooter;

    [Export]
    int Width = 10;

    [Export]
    int Height = 10;

    [Export]
    Vector2 Size = new(14, 14);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arena = GetNode<Arena>("Arena");
        shooter = GetNode<Shooter>("Shooter");

        arena.Create(Width, Height, Size);
        float leftEdge = arena.Position.X - 12;
        float rightEdge = leftEdge + Width * 12 * 2;
        shooter.Bounds = (leftEdge, rightEdge);

        GD.Print(shooter.Bounds.Item1, ", ", shooter.Bounds.Item2);
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

        if (Input.IsActionJustPressed("do_my_biding"))
        {
            arena.ShiftDown();
        }
    }

    public void OnHit(Vector2 position, string color)
    {
        arena.AddBall(position, color);
    }

    public override void _Draw()
    {
        DrawBounds();
    }

    private void DrawBounds()
    {
        DrawLine(
            new Vector2(shooter.Bounds.Item1 - 6, 0),
            new Vector2(shooter.Bounds.Item1 - 6, 1000),
            Colors.White
        );
        DrawLine(
            new Vector2(shooter.Bounds.Item2 + 6, 0),
            new Vector2(shooter.Bounds.Item2 + 6, 1000),
            Colors.White
        );
    }
}
