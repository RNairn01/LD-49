using Godot;
using System;
using System.Collections.Generic;

public class Scrub : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool NeedsTutorial { get; set; } = true;
    public bool canFail { get; set; } = false;
    private bool isSelected = false;
    private bool hasLeftBeenScrubbed, hasRightBeenScrubbed = false;
    private int fullScrubCounter = 0;
    private AudioStreamPlayer scrub1, scrub2;
    
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.ScrubState;
        scrub1 = GetNode<AudioStreamPlayer>("Scrub1");
        scrub2 = GetNode<AudioStreamPlayer>("Scrub2");
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        VoiceLinesNormal = PopulateNormalLine("scrub");
        VoiceLinesQuick = PopulateQuickLine("scrub");
        VoiceLinesTutorial = PopulateTutorialLine("scrub");
    }

    public override void _Process(float delta)
    {
        if (isSelected)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }
        else
        {
            GlobalPosition = Drag(GlobalPosition, GetNode<Node2D>("..").GlobalPosition, 5 * delta);
        }
      
    }
    
    public void OnInteract()
    {
        if (hasLeftBeenScrubbed && hasRightBeenScrubbed)
        {
            hasLeftBeenScrubbed = false;
            hasRightBeenScrubbed = false;
            fullScrubCounter++;
        }
        
        if (fullScrubCounter >= 2) OnComplete();
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
        alchemist.ChangeState(Alchemist.AlchemistState.Pointing);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            isSelected = false;
            Cursor.IsHoldingSomething = false;
            gameManager.AddStrike("Scrub task failed");
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
        GD.Print("Scrub task complete!");
        isSelected = false;
        Cursor.IsHoldingSomething = false;
        fullScrubCounter = 0;
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("Scrub it!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void OnDragSponge(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (!Input.IsActionJustPressed("click")) return;
        if (!IsActive) OnFailure();
        else if (!isSelected) {
            GD.Print("Picked up sponge");
            isSelected = true;
            Cursor.IsHoldingSomething = true;
        } else
        {
            GD.Print("Dropped sponge");
            isSelected = false;
            Cursor.IsHoldingSomething = false;
        }
    }

    public void OnScrubOneEntered(Area2D area)
    {
        if (area.Name == "ScrubDrag")
        {
            hasLeftBeenScrubbed = true;
            //Play scrub sound effect
            GD.Print("Left Scrubbed");
            scrub1.Play();
            OnInteract();
        }
    }
    
    public void OnScrubTwoEntered(Area2D area)
    {
        GD.Print(area.Name);
        if (area.Name == "ScrubDrag")
        {
            hasRightBeenScrubbed = true;
            //Play scrub sound effect
            GD.Print("Right Scrubbed");
            scrub2.Play();
            OnInteract();
        }
    }
    protected List<string> PopulateNormalLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-5.ogg");
        return files;
    }
    protected List<string> PopulateQuickLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Frantic-1.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Frantic-2.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Frantic-3.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Frantic-4.ogg");
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Frantic-5.ogg");
        return files;
    }
    protected List<string> PopulateTutorialLine(string path)
    {
        var files = new List<string>();
        files.Add("res://src/assets/sfx/voice-clips/scrub/Scrub-Tutorial.ogg");
        return files;
    }
}
