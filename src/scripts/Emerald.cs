using Godot;
using System;
using System.Collections.Generic;

public class Emerald : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    public bool NeedsTutorial { get; set; } = true;
    private bool holdingEmerald, emeraldIsFalling, emeraldCanBeDropped = false;
    private Vector2 startPosition;
    private AudioStreamPlayer emeraldPickup, drop;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../EmeraldJar").GlobalPosition;
        inputState = InputStates.InputState.MoreEmeraldState;
        VoiceLinesNormal = PopulateNormalLine("emeralds");
        VoiceLinesQuick = PopulateQuickLine("emeralds");
        VoiceLinesTutorial = PopulateTutorialLine("emeralds");
        emeraldPickup = GetNode<AudioStreamPlayer>("../EmeraldJar/EmeraldPickup");
        drop = GetNode<AudioStreamPlayer>("../EmeraldJar/Drop");
    }

    public override void _Process(float delta)
    {
        if (holdingEmerald && emeraldCanBeDropped && Input.IsActionJustPressed("click")) DropEmerald();
        else if (holdingEmerald)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (emeraldIsFalling)
            GlobalPosition = Drag(GlobalPosition, new Vector2(GlobalPosition.x, GlobalPosition.y + 100), 15 * delta);
    }

    public void OnInteract()
    {
        if (Input.IsActionJustPressed("click"))
        {
            if (!IsActive)
            {
                OnFailure();
                return;
            }
            GD.Print("Picked up emerald");
            holdingEmerald = true;
            Cursor.IsHoldingSomething = true;
            emeraldPickup.Play();
        }
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
        alchemist.ChangeState(Alchemist.AlchemistState.Thinking);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            gameManager.AddStrike("Emerald task failed");
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
        GD.Print("More emerald task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("More\nEmerald!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpEmerald(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void EmeraldExitJar(Area2D area)
    {
        if (area.Name == "EmeraldArea") emeraldCanBeDropped = true;
    }
    
    public void EmeraldEnterJar(Area2D area)
    {
        if (area.Name == "EmeraldArea") emeraldCanBeDropped = false;
    }

    public void OnEmeraldCollide(Area2D area)
    {
        GD.Print("Emerald collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop")
        {
            GD.Print("Emerald dropped in cauldron!");
            if (holdingEmerald) return;
            drop.Play();
            GlobalPosition = startPosition;
            emeraldIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            emeraldIsFalling = false;
            OnFailure();
        }
    }

    private void DropEmerald()
    {
        holdingEmerald = false;
        Cursor.IsHoldingSomething = false;
        emeraldIsFalling = true;
        //Play drop sound effect
    }
}
