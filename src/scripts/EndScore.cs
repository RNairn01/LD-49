using Godot;
using System;

public class EndScore : Sprite
{
    private Label endScore;
    public override void _Ready()
    {
        endScore = GetNode<Label>("EndScore");
        endScore.Text = GameManager.Score.ToString();
    }
}
