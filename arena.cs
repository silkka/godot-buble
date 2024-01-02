using System.Collections.Generic;
using Godot;
using HexGrid;

public partial class Arena : Node2D
{
    [Export]
    PackedScene Ball_Scene;

    private Grid grid;
    private const int WIDTH = 10;
    private const int HEIGHT = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        grid = new Grid(new Vector2(14, 14), Position);
        grid.CreateRectangular(WIDTH, HEIGHT);

        foreach (Hex hex in grid.GetHexes())
        {
            Ball ball = Ball_Scene.Instantiate<Ball>();
            ball.SetRandomColor();
            ball.Position = Layout.HexToPixel(grid.layout, hex);
            grid.Set(hex, ball);
            CallDeferred(MethodName.AddSibling, ball);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void AddBall(Vector2 position, string color)
    {
        Hex hex = FractionalHex.HexRound(Layout.PixelToHex(grid.layout, position));

        Ball ball = Ball_Scene.Instantiate<Ball>();
        ball.SetColor(color);
        ball.Position = Layout.HexToPixel(grid.layout, hex);

        grid.Set(hex, ball);

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

        RemoveUnconnected();
    }

    public void CheckForMatches(
        Hex hex,
        Ball.Ball_Colors color,
        List<Hex> matches,
        bool all = false
    )
    {
        // Add self if match
        Ball ball = grid.Get(hex);
        if (ball != null)
        {
            if (all || ball.Ball_Color == color)
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

            if ((all || n.Ball_Color == color) && !matches.Contains(neighbor))
            {
                CheckForMatches(neighbor, color, matches, all);
            }
        }
    }

    public void RemoveUnconnected()
    {
        Grid topRowGen = new(new(1, 1), Position);
        topRowGen.CreateRectangular(WIDTH, 1);
        List<Hex> topRow = topRowGen.GetHexes();

        List<Hex> matches = new List<Hex>();

        foreach (Hex hex in topRow)
        {
            if (grid.Contains(hex))
            {
                CheckForMatches(hex, Ball.Ball_Colors.red, matches, true);
            }
        }

        foreach (Hex hex in grid.GetHexes())
        {
            if (!matches.Contains(hex))
            {
                grid.Get(hex).QueueFree();
                grid.Remove(hex);
            }
        }
    }
}
