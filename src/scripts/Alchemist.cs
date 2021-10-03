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

    private AlchemistState currentAlchemistState = AlchemistState.Idle;
    
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        switch (currentAlchemistState)
        {
            case AlchemistState.Idle:
                Play("idle");
                break;
            case AlchemistState.Pointing:
                Play("pointing");
                break;
            case AlchemistState.Thinking:
                Play("thinking");
                break;
            case AlchemistState.HighFive:
                Play("highfive");
                break;
        }
    }

    public void ChangeState(AlchemistState state)
    {
        currentAlchemistState = state;
    }
}
