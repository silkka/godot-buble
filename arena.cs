using System.Collections.Generic;
using Godot;
using HexGrid;

public partial class Arena : Node2D
{
    [Export]
    PackedScene Ball_Scene;

    private List<Ball> Grid;
    private Vector2[] points;
    private Color[] colors;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        (points, colors) = Create();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _Draw()
    {
        DrawPolygon(points, colors);
    }

    public static (Vector2[], Color[]) Create()
    {
        Layout layout =
            new(
                orientation: HexGrid.Orientation.POINTY,
                size: new Vector2(32, 32),
                origin: new Vector2(100, 100)
            );

        Vector2[] points = Layout.PolygonCorners(layout, new Hex(0, 0));
        Color color = new(0.5f, 0.5f, 0.5f, 1.0f);
        Color[] colors = new Color[6] { color, color, color, color, color, color };

        return (points, colors);
    }
}
