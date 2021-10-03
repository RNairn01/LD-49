using Godot;
using System;
using System.Collections.Generic;

public class HighFive : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    public bool NeedsTutorial { get; set; } = true;

    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.HighFiveState;
        VoiceLinesNormal = PopulateNormalLine("highfive");
        VoiceLinesQuick = PopulateQuickLine("highfive");
        VoiceLinesTutorial = PopulateTutorialLine("highfive");
    }

    public void OnInteract()
    {
        //Play high five sound
        GD.Print("High five clicked");
        OnComplete();
    }

    public void PlayCurrentVoiceLine()
    {
        List<string> activeList = VoiceLinesNormal;
        if (NeedsTutorial) activeList = VoiceLinesTutorial; 
        else if (gameManager.GameHasSpedUp) activeList = VoiceLinesQuick;
        else activeList = VoiceLinesNormal;
        var index = GameManager.Rand.RandiRange(0, activeList.Count - 1);
        GD.Print(activeList[index]);
        voice.Stream = GD.Load<AudioStream>(activeList[index]);
        voice.Play();
    }

    public void ChangeAlchemistState()
    {
        alchemist.ChangeState(Alchemist.AlchemistState.HighFive);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            gameManager.AddStrike("High five task failed");
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
        canFail = false;
        voice.Stop();
        GD.Print("High Five task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        alchemist.SpeechBubble("High Five!", bubbleTime, false);
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void OnClick(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (Input.IsActionJustPressed("click"))
        { 
            if (IsActive) OnInteract();
            else if (!IsActive && gameManager.CanAddStrike)
            {
                OnFailure();
            } 
        }
    }
}
