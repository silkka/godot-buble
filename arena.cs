using System.Collections.Generic;
using Godot;
using HexGrid;

public partial class Arena : Node2D
{
    [Export]
    PackedScene Ball_Scene;

    private Grid grid;
    private Color[] colors;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        grid = new Grid(new Vector2(14, 14), Position);
        grid.CreateRectangular(10, 10);

        foreach (Hex hex in grid.GetHexes())
        {
            Ball ball = Ball_Scene.Instantiate<Ball>();
            ball.SetRandomColor();
            ball.Position = Layout.HexToPixel(grid.layout, hex);
            grid.Set(hex, ball);
            AddChild(ball);
        }

        colors = new Color[6]
        {
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _Draw()
    {
        foreach (Hex hex in grid.GetHexes())
        {
            Vector2[] points = Layout.PolygonCorners(grid.layout, hex);
            // DrawPolygon(points, colors);
        }
    }
}
