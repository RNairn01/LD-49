using Godot;
using System;

public abstract class AlchemyInput : AnimatedSprite
{
    
    protected GameManager gameManager;
    protected InputStates.InputState inputState;
    protected Alchemist alchemist;
    protected string[] VoiceLinesTutorial;
    protected string[] VoiceLinesNormal;
    protected string[] VoiceLinesQuick;
    protected string voiceLinesTopDirectory = "res://src/assets/sfx/voice-clips/";
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("Node2D/GameManager");
        alchemist = GetTree().Root.GetNode<Alchemist>("Node2D/Alchemist");
    }
    
    private float Drag(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
    
    protected Vector2 Drag(Vector2 firstVector, Vector2 secondVector, float by)
    {
        float retX = Drag(firstVector.x, secondVector.x, by);
        float retY = Drag(firstVector.y, secondVector.y, by);
        return new Vector2(retX, retY);
    }
}
