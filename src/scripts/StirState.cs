using Godot;
using System;

public class StirState : AlchemyInput, IAlchemyInput
{
    private int timesClicked = 0;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.StirState;
    }

    public void OnInteract()
    {
        timesClicked++;
        //Advance stir animation
        GD.Print($"Clicked in cauldron! - {timesClicked}");
        if (timesClicked >= 5) OnComplete(); 
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for stir task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to pointing");
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        gameManager.StrikeCount++;
        GD.Print("Stir task failed");
        //Play failure voice line
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Stir task complete!");
        //Earn score
        //Get new task from GameManager
    }

    public void BecomeActive()
    {
        //ChangeAlchemistState(AlchemistState.Pointing);
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
}
