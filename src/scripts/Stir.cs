using Godot;
using System;

public class Stir : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    private int timesClicked = 0;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.StirState;
    }

    public void OnInteract()
    {
        timesClicked++;
        Frame++;
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
        //alchemist.ChangeState(AlchemistState.Pointing);
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        gameManager.AddStrike("Stir task failed", "");
        Frame = 0;
        //Play shaking cauldron animation here
        gameManager.GetNewTask();
    }

    public void OnComplete()
    {
        GD.Print("Stir task complete!");
        timesClicked = 0;
        Frame = 0;
        //Earn score
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
            else if (gameManager.PreviousTask != this)
            {
                OnFailure();
            }
        }
    }
}
