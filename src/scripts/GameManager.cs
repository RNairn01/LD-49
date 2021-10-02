using Godot;
using System;

public class GameManager : Node
{
    public static int Score;
    
    public bool IsGameOver = false;
    public float CurrentScoreMultiplier = 1;
    public InputStates.InputState CurrentInputState;

    private SceneManager sceneManager;
    private bool canAddStrike = true;
    private int strikeCount = 0;
    
    public override void _Ready()
    {
        sceneManager = GetNode<SceneManager>("../SceneManager");
    }

    public override void _Process(float delta)
    {
        if (strikeCount >= 3) GameOver();
      
    }

    private void GameOver()
    {
        IsGameOver = true;
        //Implement rest of game over logic here
    }

    public void AddStrike(string message, string voiceClip)
    {
        if (canAddStrike)
        {
            canAddStrike = false;
            strikeCount++;
            GD.Print(message);
            //Play voice clip
            StartFailGracePeriod(1);
        }
        
    }

    private async void StartFailGracePeriod(float time)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        canAddStrike = true;
    }
}
