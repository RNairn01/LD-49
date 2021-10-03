using Godot;
using System;

public class Scrub : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    private bool isSelected = false;
    private bool hasLeftBeenScrubbed, hasRightBeenScrubbed = false;
    private int fullScrubCounter = 0;
    
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.ScrubState;
        VoiceLinesNormal = PopulateNormalLine("scrub");
        VoiceLinesQuick = PopulateQuickLine("scrub");
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
        GD.Print("Play voice line for scrub task");
        var index = GameManager.Rand.RandiRange(0, VoiceLinesNormal.Count - 1);
        GD.Print(VoiceLinesNormal[index]);
        voice.Stream = GD.Load<AudioStream>(VoiceLinesNormal[index]);
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
            OnInteract();
        }
    }
}
