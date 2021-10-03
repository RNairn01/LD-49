using Godot;
using System;

public class UIManager : Control
{
    public Label ScoreLabel;
    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("Score");
    }

    public override void _Process(float delta)
    {
        ScoreLabel.Text = GameManager.Score.ToString();
    }
    
    public async void ScorePop(float time)
    {
        var originalScale = ScoreLabel.RectScale;
        ScoreLabel.RectScale += new Vector2(0.05f, 0.05f);
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        ScoreLabel.RectScale = originalScale;
    }

}
