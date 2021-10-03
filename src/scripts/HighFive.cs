using Godot;
using System;

public class HighFive : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
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
        gameManager.AddStrike("High five task failed", "");
        gameManager.GetNewTask();
    }

    public void OnComplete()
    {
        GD.Print("High Five task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        IsActive = true;
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
