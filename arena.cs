using System;
using System.Collections.Generic;
using Godot;

public partial class arena : Node2D
{
    [Export]
    PackedScene Ball_Scene;

    private List<Ball> Grid;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void Create()
    {
        Grid = new List<Ball>();
        Console.WriteLine("Arena created", Grid);
    }
}
