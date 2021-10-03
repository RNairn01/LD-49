using Godot;
using System;
using System.Collections.Generic;

public class Newt : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool NeedsTutorial { get; set; } = true;
    public bool canFail { get; set; } = false;
    private bool holdingNewt, newtIsFalling, newtCanBeDropped = false;
    private Vector2 startPosition;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../NewtJar").GlobalPosition;
        inputState = InputStates.InputState.MoreNewtState;
        VoiceLinesNormal = PopulateNormalLine("newt");
        VoiceLinesQuick = PopulateQuickLine("newt");
        VoiceLinesTutorial = PopulateTutorialLine("newt");
    }

    public override void _Process(float delta)
    {
        if (holdingNewt && newtCanBeDropped && Input.IsActionJustPressed("click")) DropNewt();
        else if (holdingNewt)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (newtIsFalling)
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
            GD.Print("Picked up newt");
            holdingNewt = true;
            Cursor.IsHoldingSomething = true;
            //Play jar sloshing sound effect
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
            gameManager.AddStrike("More newt task failed");
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
        GD.Print("More newt task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("More\nNewt!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpNewt(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void NewtExitJar(Area2D area)
    {
        if (area.Name == "NewtArea") newtCanBeDropped = true;
    }
    
    public void NewtEnterJar(Area2D area)
    {
        if (area.Name == "NewtArea") newtCanBeDropped = false;
    }

    public void OnNewtCollide(Area2D area)
    {
        GD.Print("Newt collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop")
        {
            GD.Print("Newt dropped in cauldron!");
            if (holdingNewt) return;
            GlobalPosition = startPosition;
            newtIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            newtIsFalling = false;
            OnFailure();
        }
    }

    private void DropNewt()
    {
        holdingNewt = false;
        Cursor.IsHoldingSomething = false;
        newtIsFalling = true;
        //Play drop sound effect
    }
}
