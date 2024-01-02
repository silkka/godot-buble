using Godot;

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

    private int shots = 0;
    private Edges edges;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arena = GetNode<Arena>("Arena");
        shooter = GetNode<Shooter>("Shooter");

        arena.Create(Width, Height, Size);

        const int ballRadius = 12;

        edges = new()
        {
            Left = arena.Position.X - ballRadius,
            Right = arena.Position.X - ballRadius + Width * ballRadius * 2,
            Top = arena.Position.Y - ballRadius,
            Botttom = arena.Position.Y + (Height + 8) * ballRadius * 2
        };

        shooter.Bounds = (edges.Left, edges.Right);
        shooter.Position = new Vector2((edges.Left + edges.Right) / 2, edges.Botttom);

        GD.Print(shooter.Bounds.Item1, ", ", shooter.Bounds.Item2);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("quit"))
        {
            GetTree().Quit();
        }

        if (Input.IsActionJustPressed("do_my_biding"))
        {
            arena.ShiftDown();
        }
    }

    public void OnHit(Vector2 position, string color)
    {
        arena.AddBall(position, color);
        shots++;
        if (shots % 5 == 0)
        {
            arena.ShiftDown();
        }

        if (arena.OutOfBounds(edges.Botttom))
        {
            GD.Print("Game Over");
            GetTree().Quit();
        }

        if (arena.Victory())
        {
            GD.Print("You Win!");
            GetTree().Quit();
        }
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

        // Draw bottom edge
        DrawLine(
            new Vector2(shooter.Bounds.Item1 - 6, edges.Botttom + 6),
            new Vector2(shooter.Bounds.Item2 + 6, edges.Botttom + 6),
            Colors.White
        );
    }
}

struct Edges
{
    public float Left;
    public float Right;
    public float Top;
    public float Botttom;
}
