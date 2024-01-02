using System;
using Godot;

public partial class Ball : Area2D
{
    public Vector2 Veclocity = Vector2.Zero;
    public Ball_Colors Ball_Color;
    public bool Active = false;
    private (float, float) Bounds = (0f, 10000f);

    [Signal]
    public delegate void HitEventHandler(Vector2 position, string color);

    [Export]
    int numberOfColors = 6;

    public enum Ball_Colors
    {
        black,
        blue,
        green,
        orange,
        purple,
        red,
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Connections
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Node2D body)
    {
        Veclocity = Vector2.Zero;
        if (Active)
        {
            Active = false;
            EmitSignal(SignalName.Hit, Position, Ball_Color.ToString());
            QueueFree();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!Active)
        {
            return;
        }

        Vector2 nextPos = Position + Veclocity * (float)delta;

        if (nextPos.X - 7 < Bounds.Item1)
        {
            nextPos.X = Bounds.Item1 + 7;
            Veclocity.X *= -1;
        }
        else if (nextPos.X + 7 > Bounds.Item2)
        {
            nextPos.X = Bounds.Item2 - 7;
            Veclocity.X *= -1;
        }

        Position = nextPos;
    }

    public void Shoot(Vector2 position, float rotation, (float, float) bounds)
    {
        Position = position;
        Veclocity = Vector2.FromAngle(rotation - MathF.PI / 2) * 200f;
        Active = true;
        Bounds = bounds;
    }

    public Ball SetRandomColor()
    {
        Array colors = Enum.GetValues(typeof(Ball_Colors));
        Array selection = Array.CreateInstance(typeof(Ball_Colors), numberOfColors);
        Array.Copy(colors, selection, numberOfColors);
        SetColor(Utils.PickRandomElement<Ball_Colors>(selection));
        return this;
    }

    public Ball SetColor(Ball_Colors color)
    {
        Ball_Color = color;
        switch (Ball_Color)
        {
            case Ball_Colors.blue:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("blue");
                break;
            case Ball_Colors.black:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("black");
                break;
            case Ball_Colors.red:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("red");
                break;
            case Ball_Colors.green:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("green");
                break;
            case Ball_Colors.orange:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("orange");
                break;
            case Ball_Colors.purple:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("purple");
                break;
            default:
                GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("blue");
                break;
        }
        return this;
    }

    public static Ball_Colors BallColorFromString(string color)
    {
        return color switch
        {
            "blue" => Ball_Colors.blue,
            "black" => Ball_Colors.black,
            "red" => Ball_Colors.red,
            "green" => Ball_Colors.green,
            "orange" => Ball_Colors.orange,
            "purple" => Ball_Colors.purple,
            _ => throw new Exception($"Invalid color: {color}"),
        };
    }

    public void SetColor(string color)
    {
        SetColor(BallColorFromString(color));
    }
}
