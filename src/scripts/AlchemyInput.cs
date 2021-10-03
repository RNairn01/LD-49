using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class AlchemyInput : AnimatedSprite
{
    
    protected GameManager gameManager;
    protected InputStates.InputState inputState;
    protected Alchemist alchemist;
    protected string[] VoiceLinesTutorial;
    protected List<string> VoiceLinesNormal;
    protected List<string> VoiceLinesQuick;
    protected List<string> FailLines;
    protected string voiceLinesTopDirectory = "res://src/assets/sfx/voice-clips/";
    protected Directory normalVoiceFiles = new Directory();
    protected AudioStreamPlayer voice;
    protected AudioStreamPlayer angerVoice;
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("Node2D/GameManager");
        alchemist = GetTree().Root.GetNode<Alchemist>("Node2D/Alchemist");
        voice = GetNode<AudioStreamPlayer>("../Voice");
        angerVoice = GetNode<AudioStreamPlayer>("../AngerVoice");
        FailLines = PopulateFailLine("wrong");
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

    protected List<string> PopulateNormalLine(string path)
    {
        var files = new List<string>();
        
        if (normalVoiceFiles.Open(voiceLinesTopDirectory + path) == Error.Ok)
        {
            normalVoiceFiles.ListDirBegin();
            var fileName = normalVoiceFiles.GetNext();
            while (fileName != "")
            {
                    fileName = normalVoiceFiles.GetNext();
                    files.Add(voiceLinesTopDirectory + path + "/" + fileName);
            }
            normalVoiceFiles.ListDirEnd();
        }
        return files.Where(e => e.Contains(".ogg") && !e.Contains(".import") && !e.Contains("Tutorial") && !e.Contains("Frantic")).ToList();
    }
    protected List<string> PopulateQuickLine(string path)
    {
        var files = new List<string>();
        
        if (normalVoiceFiles.Open(voiceLinesTopDirectory + path) == Error.Ok)
        {
            normalVoiceFiles.ListDirBegin();
            var fileName = normalVoiceFiles.GetNext();
            while (fileName != "")
            {
                    fileName = normalVoiceFiles.GetNext();
                    files.Add(voiceLinesTopDirectory + path + "/" + fileName);
            }
            normalVoiceFiles.ListDirEnd();
        }
        return files.Where(e => e.Contains(".ogg") && !e.Contains(".import") && !e.Contains("Tutorial") && e.Contains("Frantic")).ToList();
    }
    protected List<string> PopulateFailLine(string path)
    {
        var files = new List<string>();
        
        if (normalVoiceFiles.Open(voiceLinesTopDirectory + path) == Error.Ok)
        {
            normalVoiceFiles.ListDirBegin();
            var fileName = normalVoiceFiles.GetNext();
            while (fileName != "")
            {
                    fileName = normalVoiceFiles.GetNext();
                    files.Add(voiceLinesTopDirectory + path + "/" + fileName);
            }
            normalVoiceFiles.ListDirEnd();
        }
        return files.Where(e => e.Contains(".ogg") && !e.Contains(".import")).ToList();
    }
}
