using Godot;
using System;
using System.Collections.Generic;

public class Cool : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    public bool NeedsTutorial { get; set; } = true;
    private AudioStreamPlayer wooshSound;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.CoolState;
        VoiceLinesNormal = PopulateNormalLine("cool");
        VoiceLinesQuick = PopulateQuickLine("cool");
        VoiceLinesTutorial = PopulateTutorialLine("cool");
        wooshSound = GetNode<AudioStreamPlayer>("WooshSound");
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        VoiceLinesNormal = PopulateNormalLine("cool");
        VoiceLinesQuick = PopulateQuickLine("cool");
        VoiceLinesTutorial = PopulateTutorialLine("cool");
    }

    public void OnInteract()
    {
        wooshSound.Play();
        //Make button animate
        GD.Print("Cool button clicked");
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
        alchemist.ChangeState(Alchemist.AlchemistState.Idle);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            gameManager.AddStrike("Cool task failed");
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
        GD.Print("Cool task complete!");
        fire.Play("woosh");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("Cool it!", bubbleTime, false);
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
            else OnFailure();
        }
    }
    
    protected List<string> PopulateNormalLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-5.ogg");
        return files;
    }
    protected List<string> PopulateQuickLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Frantic-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Frantic-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Frantic-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Frantic-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Frantic-5.ogg");
        return files;
    }
    protected List<string> PopulateTutorialLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/cool/Cool-Tutorial.ogg");
        return files;
    }
}
