using Godot;
using System;
using System.Collections.Generic;

public class HighFive : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    public bool NeedsTutorial { get; set; } = true;
    private AudioStreamPlayer slapSound;

    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.HighFiveState;
        VoiceLinesNormal = PopulateNormalLine("highfive");
        VoiceLinesQuick = PopulateQuickLine("highfive");
        VoiceLinesTutorial = PopulateTutorialLine("highfive");
        slapSound = GetNode<AudioStreamPlayer>("SlapSound");
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        VoiceLinesNormal = PopulateNormalLine("highfive");
        VoiceLinesQuick = PopulateQuickLine("highfive");
        VoiceLinesTutorial = PopulateTutorialLine("highfive");
    }

    public void OnInteract()
    {
        slapSound.Play();
        GD.Print("High five clicked");
        OnComplete();
    }

    public void PlayCurrentVoiceLine()
    {
        List<string> activeList = VoiceLinesNormal;
        if (NeedsTutorial) activeList = VoiceLinesTutorial; 
        else if (gameManager.GameHasSpedUp) activeList = VoiceLinesQuick;
        else activeList = VoiceLinesNormal;
        var index = GameManager.Rand.RandiRange(0, activeList.Count - 1);
        GD.Print(activeList[index]);
        voice.Stream = GD.Load<AudioStream>(activeList[index]);
        voice.Play();
    }

    public void ChangeAlchemistState()
    {
        alchemist.ChangeState(Alchemist.AlchemistState.HighFive);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            gameManager.AddStrike("High five task failed");
            gameManager.GetNewTask();
            var index = GameManager.Rand.RandiRange(0, FailLines.Count - 1);
            GD.Print(FailLines[index]);
            angerVoice.Stream = GD.Load<AudioStream>(FailLines[index]);
            angerVoice.Play();
            failSmoke.Play("smoke");
        }
    }

    public void OnComplete()
    {
        canFail = false;
        voice.Stop();
        GD.Print("High Five task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("High Five!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void OnClick(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (Input.IsActionJustPressed("click"))
        { 
            if (IsActive) OnInteract();
            else if (!IsActive && gameManager.CanAddStrike)
            {
                OnFailure();
            } 
        }
    }
    
    protected List<string> PopulateNormalLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-5.ogg");
        return files;
    }
    protected List<string> PopulateQuickLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Frantic-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Frantic-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Frantic-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Frantic-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Frantic-5.ogg");
        return files;
    }
    protected List<string> PopulateTutorialLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/highfive/HighFive-Tutorial.ogg");
        return files;
    }
}
