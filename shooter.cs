using System;
using Godot;

public partial class Shooter : Node2D
{
    [Export]
    PackedScene Ball_Scene;
    private float RotationSpeed = 2;
    private const float MAX_TURN = MathF.PI / 2 - MathF.PI / 16;
    private const float MIN_TURN = -MathF.PI / 2 + MathF.PI / 16;

    [Signal]
    public delegate void HitEventHandler(Vector2 position, string color);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

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

        if (Input.IsActionJustPressed("shoot"))
        {
            var ball = Ball_Scene.Instantiate<Ball>().SetRandomColor();
            ball.Veclocity = Vector2.FromAngle(Rotation - MathF.PI / 2) * 200f;
            ball.Position = Position;
            ball.Active = true;
            AddSibling(ball);

            Game game = GetParentOrNull<Game>();
            if (game != null)
            {
                ball.Hit += game.OnHit;
            }
        }
    }
}
