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
    private UIManager uiManager;
    public bool CanAddStrike = true;
    private int strikeCount = 0;
    private int correctInputStreak = 0;

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
        Score = 0;
        sceneManager = GetNode<SceneManager>("../SceneManager");
        uiManager = GetNode<UIManager>("../UI");
        stirTask = GetNode<IAlchemyInput>("../StirStateOrigin/Stir");
        scrubTask = GetNode<IAlchemyInput>("../ScrubStateOrigin/Scrub");
        moreSoulTask = GetNode<IAlchemyInput>("../MoreSoulStateOrigin/Soul");
        moreNewtTask = GetNode<IAlchemyInput>("../MoreNewtStateOrigin/Newt");
        moreEmeraldTask = GetNode<IAlchemyInput>("../MoreEmeraldStateOrigin/Emerald");
        addSaltTask = GetNode<IAlchemyInput>("../SaltStateOrigin/SaltState");
        coolTask = GetNode<IAlchemyInput>("../CoolStateOrigin/Cool");
        boilTask = GetNode<IAlchemyInput>("../BoilStateOrigin/Boil");
        highFiveTask = GetNode<IAlchemyInput>("../HighFiveStateOrigin/HighFive");

        tasks = new[] { stirTask, scrubTask, moreSoulTask, moreNewtTask, moreEmeraldTask, addSaltTask, coolTask, boilTask, highFiveTask };
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

    public void AddStrike(string message)
    {
        if (CanAddStrike)
        {
            CurrentScoreMultiplier = 1;
            correctInputStreak = 0;
            CanAddStrike = false;
            strikeCount++;
            GD.Print(message);
            StartFailGracePeriod(1);
        }
        
    }

    public void AddScore()
    {
        correctInputStreak++;
        uiManager.ScorePop(0.5f);
        if (correctInputStreak % 5 == 0) CurrentScoreMultiplier += 0.2f;
        if (Score <= 999999)
        {
            Score += Mathf.RoundToInt(50 * CurrentScoreMultiplier);
            GD.Print($"Score = {Score}");
            //Add extra score based on percentage of time remaining
        }
    }

    private IAlchemyInput getRandomTask()
    {
        Rand.Randomize();
        var rand = Rand.RandiRange(0, tasks.Length - 1);
        return tasks[rand];
    }

    public void GetNewTask()
    {
        PreviousTask = CurrentTask;
        CurrentTask.IsActive = false;
        NewTask();
    }

    private async void NewTask()
    {
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        CurrentTask = getRandomTask();
        GD.Print($"Current task - {CurrentTask.GetType()}");
        CurrentTask.BecomeActive();
    }

    private async void StartFailGracePeriod(float time)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        CanAddStrike = true;
    }
}
