using Godot;
using System;

public class UIManager : Control
{
    public Label ScoreLabel;
    private Sprite strike1, strike2, strike3;
    public Sprite[] Strikes;
    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("Score");
        strike1 = GetNode<Sprite>("Strike1");
        strike2 = GetNode<Sprite>("Strike2");
        strike3 = GetNode<Sprite>("Strike3");
        Strikes = new[] { strike1, strike2, strike3 };
        strike1.Visible = false;
        strike2.Visible = false;
        strike3.Visible = false;
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
