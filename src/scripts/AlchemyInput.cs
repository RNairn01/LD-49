using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class AlchemyInput : AnimatedSprite
{
    
    protected GameManager gameManager;
    protected InputStates.InputState inputState;
    protected Alchemist alchemist;
    protected List<string> VoiceLinesTutorial;
    protected List<string> VoiceLinesNormal;
    protected List<string> VoiceLinesQuick;
    protected List<string> FailLines;
    protected string voiceLinesTopDirectory = "res://src/assets/sfx/voice-clips/";
    protected Directory normalVoiceFiles = new Directory();
    protected AudioStreamPlayer voice;
    protected AudioStreamPlayer angerVoice;
    protected AnimatedSprite failSmoke;
    protected AnimatedSprite fire;
    protected float bubbleTime = 2;
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("Node2D/GameManager");
        alchemist = GetTree().Root.GetNode<Alchemist>("Node2D/Alchemist");
        voice = GetNode<AudioStreamPlayer>("../Voice");
        angerVoice = GetNode<AudioStreamPlayer>("../AngerVoice");
        FailLines = PopulateFailLine("wrong");
        failSmoke = GetNode<AnimatedSprite>("../../Cauldron/Smoke");
        fire = GetNode<AnimatedSprite>("../../Cauldron/Fire");
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
    protected List<string> PopulateFailLine(string path)
    {
        var files = new List<string>();
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-1.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-2.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-3.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-4.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-5.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-Frantic-1.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-Frantic-2.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-Frantic-3.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-Frantic-4.ogg");
                files.Add("res://src/assets/sfx/voice-clips/wrong/Wrong-Frantic-5.ogg");
                return files;
    }
}
