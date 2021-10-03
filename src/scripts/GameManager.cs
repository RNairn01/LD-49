using Godot;
using System;

public class GameManager : Node
{
    public static int Score;
    public static RandomNumberGenerator Rand = new RandomNumberGenerator();
    
    public bool IsGameOver = false;
    public float CurrentScoreMultiplier = 1;
    public InputStates.InputState CurrentInputState;
    public IAlchemyInput CurrentTask, PreviousTask;

    private SceneManager sceneManager;
    private bool canAddStrike = true;
    private int strikeCount = 0;

    private IAlchemyInput stirTask,
        scrubTask,
        boilTask,
        coolTask,
        moreNewtTask,
        moreSoulTask,
        moreEmeraldTask,
        addSaltTask,
        highFiveTask;

    private IAlchemyInput[] tasks;
    
    public override void _Ready()
    {
        sceneManager = GetNode<SceneManager>("../SceneManager");
        stirTask = GetNode<IAlchemyInput>("../StirStateOrigin/Stir");
        scrubTask = GetNode<IAlchemyInput>("../ScrubStateOrigin/Scrub");
        moreSoulTask = GetNode<IAlchemyInput>("../MoreSoulStateOrigin/Soul");
        addSaltTask = GetNode<IAlchemyInput>("../SaltStateOrigin/SaltState");
        //Add tasks as they receive art assets

        tasks = new[] { stirTask, scrubTask, moreSoulTask, addSaltTask };
        CurrentTask = stirTask;
    }

    public override void _Process(float delta)
    {
        if (strikeCount >= 3) GameOver();
      
    }

    private void GameOver()
    {
        IsGameOver = true;
        GD.Print("Game Over");
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

    private IAlchemyInput getRandomTask()
    {
        Rand.Randomize();
        var rand = Rand.RandiRange(0, tasks.Length - 1);
        return tasks[rand];
    }

    public async void GetNewTask()
    {
        PreviousTask = CurrentTask;
        CurrentTask.IsActive = false;
        await ToSignal(GetTree().CreateTimer(5f), "timeout");
        CurrentTask = getRandomTask();
        GD.Print($"Current task - {CurrentTask.GetType()}");
        CurrentTask.BecomeActive();

    }

    private async void StartFailGracePeriod(float time)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        canAddStrike = true;
    }
}
