using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Ball : Area2D
{
    public Vector2 Veclocity = Vector2.Zero;
    public Ball_Colors Ball_Color;
    public bool Active = false;

    [Signal]
    public delegate void HitEventHandler(Vector2 position, string color);

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
        Position += Veclocity * (float)delta;
    }

    public Ball SetRandomColor()
    {
        Array colors = Enum.GetValues(typeof(Ball_Colors));
        SetColor(Utils.PickRandomElement<Ball_Colors>(colors));
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
