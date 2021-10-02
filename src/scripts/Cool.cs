using Godot;
using System;

public class Cool : AlchemyInput, IAlchemyInput
{
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.CoolState;
    }

    public void OnInteract()
    {
        //Play Button click sound
        //Make button animate
        GD.Print("Cool button clicked");
        OnComplete();
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for cool task");
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
        gameManager.AddStrike("Cool task failed", "");
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Cool task complete!");
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
