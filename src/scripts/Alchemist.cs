using Godot;
using System;

public class Alchemist : AnimatedSprite
{
    public enum AlchemistState
    {
        Idle,
        Pointing,
        Thinking,
        HighFive
    }

    private Vector2 normalAlchemistPos;
    private Vector2 pointingAlchemistPos;
    private Sprite speechBubble;
    private Label speechContent;
    private Texture normalSpeechBubble, angrySpeechBubble;

    private AlchemistState currentAlchemistState = AlchemistState.Idle;
    
    public override void _Ready()
    {
        normalAlchemistPos = GlobalPosition;
        pointingAlchemistPos = normalAlchemistPos + new Vector2(0, 13);
        speechBubble = GetNode<Sprite>("SpeechBubble");
        speechContent = GetNode<Label>("SpeechBubble/Speech");
        normalSpeechBubble = GD.Load<Texture>("res://src/assets/sprites/UI/speech-normal.png");
        angrySpeechBubble = GD.Load<Texture>("res://src/assets/sprites/UI/speech-angry.png");
        speechBubble.Visible = false;
    }

    public override void _Process(float delta)
    {
        switch (currentAlchemistState)
        {
            case AlchemistState.Idle:
                Play("idle");
                GlobalPosition = normalAlchemistPos;
                break;
            case AlchemistState.Pointing:
                GlobalPosition = pointingAlchemistPos;
                Play("pointing");
                break;
            case AlchemistState.Thinking:
                Play("thinking");
                GlobalPosition = normalAlchemistPos;
                break;
            case AlchemistState.HighFive:
                Play("highfive");
                GlobalPosition = normalAlchemistPos;
                break;
        }
    }

    public void ChangeState(AlchemistState state)
    {
        currentAlchemistState = state;
    }

    public async void SpeechBubble(string speech, float time, bool isAngry)
    {
        speechContent.Text = speech;
        speechBubble.Texture = isAngry ? angrySpeechBubble : normalSpeechBubble;
        speechBubble.Visible = true;
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        speechBubble.Visible = false;
    }
}
