using Godot;
using System;

public class Cool : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.CoolState;
        VoiceLinesNormal = PopulateNormalLine("cool");
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
        gameManager.AddStrike("Cool task failed", "");
        gameManager.GetNewTask();
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("Cool task complete!");
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
