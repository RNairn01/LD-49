using Godot;
using System;

public class Boil : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.BoilState;
        VoiceLinesNormal = PopulateNormalLine("boil");
        VoiceLinesQuick = PopulateQuickLine("boil");
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
            gameManager.AddStrike("Boil task failed");
            gameManager.GetNewTask();
            var index = GameManager.Rand.RandiRange(0, FailLines.Count - 1);
            GD.Print(FailLines[index]);
            angerVoice.Stream = GD.Load<AudioStream>(FailLines[index]);
            angerVoice.Play();
            //Play shaking cauldron animation here
        }
    }

    public void OnComplete()
    {
        GD.Print("Boil task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
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
