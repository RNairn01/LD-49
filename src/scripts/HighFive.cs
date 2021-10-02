using Godot;
using System;

public class HighFive : AlchemyInput, IAlchemyInput
{
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.HighFiveState;
    }

    public void OnInteract()
    {
        //Play high five sound
        GD.Print("High five clicked");
        OnComplete();
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for high five task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to high five");
        //alchemist.ChangeState(AlchemistState.HighFive);
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        gameManager.StrikeCount++;
        GD.Print("High Five task failed");
        //Play failure voice line
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("High Five task complete!");
        //Earn score
        //Get new task from GameManager
    }

    public void BecomeActive()
    {
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
