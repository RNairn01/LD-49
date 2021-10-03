using Godot;
using System;

public class UIManager : Control
{
    private Timer countdownTimer;
    private Label scoreLabel, countdownLabel;
    private Sprite strike1, strike2, strike3;
    public Sprite[] Strikes;
    public override void _Ready()
    {
        countdownTimer = GetNode<Timer>("../Countdown");
        scoreLabel = GetNode<Label>("Score");
        countdownLabel = GetNode<Label>("CountdownLabel");
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
        scoreLabel.Text = GameManager.Score.ToString();
        countdownLabel.Text = Mathf.RoundToInt(countdownTimer.TimeLeft).ToString("00");
    }
    
    public async void ScorePop(float time)
    {
        var originalScale = scoreLabel.RectScale;
        scoreLabel.RectScale += new Vector2(0.05f, 0.05f);
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        scoreLabel.RectScale = originalScale;
    }

}
