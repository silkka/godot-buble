using System;
using Godot;

public partial class Shooter : Node2D
{
    [Export]
    PackedScene Ball_Scene;
    private float RotationSpeed = 2;
    private const float MAX_TURN = MathF.PI / 2 - MathF.PI / 16;
    private const float MIN_TURN = -MathF.PI / 2 + MathF.PI / 16;
    public (float, float) Bounds = (0f, 10000f);
    private bool ReadyToShoot = true;

    private Ball Next;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Next = GetNode<Ball>("Next");
        Next.SetRandomColor();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        float d = (float)delta;
        if (Input.IsActionPressed("turn_left"))
        {
            Rotation -= RotationSpeed * d;
        }
        if (Input.IsActionPressed("turn_right"))
        {
            Rotation += RotationSpeed * d;
        }

        Rotation = Mathf.Clamp(Rotation, MIN_TURN, MAX_TURN);

        if (ReadyToShoot && Input.IsActionJustPressed("shoot"))
        {
            ReadyToShoot = false;

            var ball = Ball_Scene.Instantiate<Ball>();
            ball.SetColor(Next.Ball_Color);
            ball.Shoot(Position, Rotation, Bounds);
            CallDeferred(MethodName.AddSibling, ball);

            ball.Hit += OnHit;

            Game game = GetParentOrNull<Game>();
            if (game != null)
            {
                ball.Hit += game.OnHit;
            }

            Next.SetRandomColor();
        }
    }

    public void OnHit(Vector2 position, string color)
    {
        ReadyToShoot = true;
    }
}
