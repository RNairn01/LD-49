using Godot;
using System;

public class Boil : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
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
        alchemist.ChangeState(Alchemist.AlchemistState.Pointing);
    }

    public void OnFailure()
    {
        gameManager.AddStrike("Boil task failed", "");
        gameManager.GetNewTask();
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Boil task complete!");
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
