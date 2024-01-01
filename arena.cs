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
            CallDeferred(MethodName.AddSibling, ball);
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
        // foreach (Hex hex in grid.GetHexes())
        // {
        //     Vector2[] points = Layout.PolygonCorners(grid.layout, hex);
        //     // DrawPolygon(points, colors);
        // }
    }

    public void AddBall(Vector2 position, string color)
    {
        Hex hex = FractionalHex.HexRound(Layout.PixelToHex(grid.layout, position));

        Ball ball = Ball_Scene.Instantiate<Ball>();
        ball.SetColor(color);
        ball.Position = Layout.HexToPixel(grid.layout, hex);

        grid.Add(hex, ball);

        CallDeferred(MethodName.AddSibling, ball);

        List<Hex> matches = new List<Hex>();
        CheckForMatches(hex, Ball.BallColorFromString(color), matches);

        if (matches.Count < 3)
        {
            return;
        }

        foreach (Hex match in matches)
        {
            grid.Get(match).QueueFree();
            grid.Remove(match);
        }
    }

    // Check for connected balls of the same color
    public void CheckForMatches(Hex hex, Ball.Ball_Colors color, List<Hex> matches)
    {
        // Add self if match
        Ball ball = grid.Get(hex);
        if (ball != null)
        {
            if (ball.Ball_Color == color)
            {
                matches.Add(hex);
            }
        }
        Hex[] neighbors = hex.Neighbors();

        foreach (Hex neighbor in neighbors)
        {
            if (!grid.Contains(neighbor))
            {
                continue;
            }

            Ball n = grid.Get(neighbor);

            if (n.Ball_Color == color && !matches.Contains(neighbor))
            {
                CheckForMatches(neighbor, color, matches);
            }
        }
    }
}
