using Godot;
using System;

public class Scrub : AlchemyInput, IAlchemyInput
{
    private bool isSelected = false;
    private bool hasLeftBeenScrubbed, hasRightBeenScrubbed = false;
    private int fullScrubCounter = 0;
    
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.ScrubState;
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
        GD.Print("Play voice line for stir task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to pointing");
        //alchemist.ChangeState(AlchemistState.Pointing);
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        isSelected = false;
        gameManager.AddStrike("Scrub task failed", "");
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Scrub task complete!");
        isSelected = false;
        fullScrubCounter = 0;
        //Earn score
        //Get new task from GameManager
    }

    public void BecomeActive()
    {
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void OnDragSponge(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (Input.IsActionJustPressed("click") && !IsActive) OnFailure();
        
        if (Input.IsActionPressed("click") && IsActive)
        {
            isSelected = true;
            Cursor.IsHoldingSomething = true;
            //play sponge pickup sound effect
            //TODO: Fix Bug where picking up sponge inside cauldron causes stir task fail
        }
        else
        {
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
