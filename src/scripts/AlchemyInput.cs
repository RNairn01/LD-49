using Godot;
using System;

public abstract class AlchemyInput : AnimatedSprite
{
    public bool IsActive = false;
    
    protected GameManager gameManager;
    protected InputStates.InputState inputState;
    protected string[] VoiceLinesTutorial;
    protected string[] VoiceLinesNormal;
    protected string[] VoiceLinesQuick;
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("Node2D/GameManager");
    }
}
