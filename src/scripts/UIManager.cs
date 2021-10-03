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

}
