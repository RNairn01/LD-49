using Godot;
using System;

public class GameManager : Node
{
    public static int Score;
    
    public bool IsGameOver = false;
    public int StrikeCount = 0;
    public float CurrentScoreMultiplier = 1;
    public InputStates.InputState CurrentInputState;

    private SceneManager sceneManager;
    
    public override void _Ready()
    {
        sceneManager = GetNode<SceneManager>("../SceneManager");
    }

    public override void _Process(float delta)
    {
        if (StrikeCount >= 3) GameOver();
      
    }

    private void GameOver()
    {
        IsGameOver = true;
        //Implement rest of game over logic here
    }
}
