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
        VoiceLinesNormal = PopulateNormalLine("stir");
        VoiceLinesQuick = PopulateQuickLine("stir");
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
        var index = GameManager.Rand.RandiRange(0, VoiceLinesNormal.Count - 1);
        GD.Print(VoiceLinesNormal[index]);
        voice.Stream = GD.Load<AudioStream>(VoiceLinesNormal[index]);
        voice.Play();
    }

    public void ChangeAlchemistState()
    {
        alchemist.ChangeState(Alchemist.AlchemistState.Idle);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike)
        {
            Frame = 0;
            gameManager.AddStrike("Stir task failed");
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
        GD.Print("Stir task complete!");
        timesClicked = 0;
        Frame = 0;
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
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
