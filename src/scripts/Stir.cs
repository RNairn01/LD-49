using Godot;
using System;
using System.Collections.Generic;

public class Stir : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool NeedsTutorial { get; set; } = true;
    public bool canFail { get; set; } = false;
    private int timesClicked = 0;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.StirState;
        VoiceLinesNormal = PopulateNormalLine("stir");
        VoiceLinesQuick = PopulateQuickLine("stir");
        VoiceLinesTutorial = PopulateTutorialLine("stir");
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        VoiceLinesNormal = PopulateNormalLine("stir");
        VoiceLinesQuick = PopulateQuickLine("stir");
        VoiceLinesTutorial = PopulateTutorialLine("stir");
    }

    public void OnInteract()
    {
        timesClicked++;
        Frame++;
        GD.Print($"Clicked in cauldron! - {timesClicked}");
        if (timesClicked >= 5) OnComplete(); 
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
            Frame = 0;
            gameManager.AddStrike("Stir task failed");
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
        GD.Print("Stir task complete!");
        timesClicked = 0;
        Frame = 0;
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("Stir it!", bubbleTime, false);
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
            else if (gameManager.PreviousTask != this)
            {
                OnFailure();
            }
        }
    }
    protected List<string> PopulateNormalLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-5.ogg");
        return files;
    }
    protected List<string> PopulateQuickLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Frantic-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Frantic-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Frantic-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Frantic-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Frantic-5.ogg");
        return files;
    }
    protected List<string> PopulateTutorialLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/stir/Stir-Tutorial.ogg");
        return files;
    }
}
