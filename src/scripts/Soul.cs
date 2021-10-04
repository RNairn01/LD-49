using Godot;
using System;
using System.Collections.Generic;

public class Soul : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool NeedsTutorial { get; set; } = true;
    public bool canFail { get; set; } = false;
    private bool holdingSoul, soulIsFalling, soulCanBeDropped = false;
    private Vector2 startPosition;
    private AudioStreamPlayer soulPickup, drop;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../SoulJar").GlobalPosition;
        inputState = InputStates.InputState.MoreSoulState;
        VoiceLinesNormal = PopulateNormalLine("soul");
        VoiceLinesQuick = PopulateQuickLine("soul");
        VoiceLinesTutorial = PopulateTutorialLine("soul");
        soulPickup = GetNode<AudioStreamPlayer>("../SoulJar/SoulPickup");
        drop = GetNode<AudioStreamPlayer>("../SoulJar/Drop");
    }

    public override void _Process(float delta)
    {
        if (holdingSoul && soulCanBeDropped && Input.IsActionJustPressed("click")) DropSoul();
        else if (holdingSoul)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (soulIsFalling)
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
            GD.Print("Picked up soul");
            holdingSoul = true;
            Cursor.IsHoldingSomething = true;
            soulPickup.Play();
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
            DropSoul();
            gameManager.AddStrike("Soul task failed");
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
        GD.Print("More soul task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("More\nSouls!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpSoul(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void SoulExitJar(Area2D area)
    {
        if (area.Name == "SoulArea") soulCanBeDropped = true;
    }
    
    public void SoulEnterJar(Area2D area)
    {
        if (area.Name == "SoulArea") soulCanBeDropped = false;
    }

    public void OnSoulCollide(Area2D area)
    {
        GD.Print("Soul collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop" && IsActive)
        {
            GD.Print("Soul dropped in cauldron!");
            if (holdingSoul) return;
            drop.Play();
            GlobalPosition = startPosition;
            soulIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            soulIsFalling = false;
            OnFailure();
        }
    }

    private void DropSoul()
    {
        holdingSoul = false;
        Cursor.IsHoldingSomething = false;
        soulIsFalling = true;
        //Play drop sound effect
    }
}
