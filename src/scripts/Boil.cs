using Godot;
using System;

public class Boil : AlchemyInput, IAlchemyInput
{
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.BoilState;
    }

    public void OnInteract()
    {
        //Play Button click sound
        //Make button animate
        GD.Print("Boil button clicked");
        OnComplete();
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for boil task");
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
        gameManager.AddStrike("Boil task failed", "");
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Boil task complete!");
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
