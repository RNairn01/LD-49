using Godot;
using System;

public class Alchemist : AnimatedSprite
{
    public enum AlchemistState
    {
        Idle,
        Pointing,
        Thinking,
        HighFive
    }

    private Vector2 normalAlchemistPos;
    private Vector2 pointingAlchemistPos;

    private AlchemistState currentAlchemistState = AlchemistState.Idle;
    
    public override void _Ready()
    {
        normalAlchemistPos = GlobalPosition;
        pointingAlchemistPos = normalAlchemistPos + new Vector2(0, 13);

    }

    public override void _Process(float delta)
    {
        switch (currentAlchemistState)
        {
            case AlchemistState.Idle:
                Play("idle");
                GlobalPosition = normalAlchemistPos;
                break;
            case AlchemistState.Pointing:
                GlobalPosition = pointingAlchemistPos;
                Play("pointing");
                break;
            case AlchemistState.Thinking:
                Play("thinking");
                GlobalPosition = normalAlchemistPos;
                break;
            case AlchemistState.HighFive:
                Play("highfive");
                GlobalPosition = normalAlchemistPos;
                break;
        }
    }

    public void ChangeState(AlchemistState state)
    {
        currentAlchemistState = state;
    }
}
