using Godot;
using System;

public class FinalScore : Sprite
{
    private Label scoreTotal;
    public override void _Ready()
    {
        scoreTotal = GetNode<Label>("ScoreTotal");
        scoreTotal.Text = $"You scored {GameManager.Score.ToString()} points";
    }
}
