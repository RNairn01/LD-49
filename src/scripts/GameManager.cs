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
    public int CurrentTimer = 10;
    public int TotalTasksCompleted = 0;

    private SceneManager sceneManager;
    private MusicManager music;
    private UIManager uiManager;
    private Timer countdown;
    private AudioStreamPlayer speedUp, intro, gameOver, gameWin;
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
        music = GetNode<MusicManager>("../MusicManager");
        countdown = GetNode<Timer>("../Countdown");
        stirTask = GetNode<IAlchemyInput>("../StirStateOrigin/Stir");
        scrubTask = GetNode<IAlchemyInput>("../ScrubStateOrigin/Scrub");
        moreSoulTask = GetNode<IAlchemyInput>("../MoreSoulStateOrigin/Soul");
        moreNewtTask = GetNode<IAlchemyInput>("../MoreNewtStateOrigin/Newt");
        moreEmeraldTask = GetNode<IAlchemyInput>("../MoreEmeraldStateOrigin/Emerald");
        addSaltTask = GetNode<IAlchemyInput>("../SaltStateOrigin/SaltState");
        coolTask = GetNode<IAlchemyInput>("../CoolStateOrigin/Cool");
        boilTask = GetNode<IAlchemyInput>("../BoilStateOrigin/Boil");
        highFiveTask = GetNode<IAlchemyInput>("../HighFiveStateOrigin/HighFive");
        speedUp = GetNode<AudioStreamPlayer>("SpeedUp");
        intro = GetNode<AudioStreamPlayer>("Intro");
        gameOver = GetNode<AudioStreamPlayer>("GameOver");
        gameWin = GetNode<AudioStreamPlayer>("GameWin");

        tasks = new[] { stirTask, scrubTask, moreSoulTask, moreNewtTask, moreEmeraldTask, addSaltTask, coolTask, boilTask, highFiveTask };
        CurrentTask = stirTask;
    }

    public override void _Process(float delta)
    {
        if (strikeCount >= 3 && !IsGameOver) GameOver();
      
    }

    private void GameOver()
    {
        IsGameOver = true;
        GD.Print("Game Over");
        countdown.Stop();
        //Implement rest of game over logic here
    }

    public void AddStrike(string message)
    {
        if (CanAddStrike && strikeCount < 4)
        {
            CanAddStrike = false;
            CurrentScoreMultiplier = 1;
            correctInputStreak = 0;
            strikeCount++;
            uiManager.Strikes[strikeCount - 1].Visible = true;
            GD.Print(message);
            StartFailGracePeriod(1);
        }
        
    }

    public void AddScore()
    {
        correctInputStreak++;
        TotalTasksCompleted++;
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
        var task = tasks[rand];
        while (task == PreviousTask)
        {
            rand = Rand.RandiRange(0, tasks.Length - 1);
            task = tasks[rand];
        }
        return tasks[rand];
    }

    public void GetNewTask()
    {
        if (IsGameOver) return;
        if (TotalTasksCompleted == 25)
        {
            music.StopMusic();
            IncreaseSpeed();
        }
        else NewTask(1f);
    }

    public void OnTimeout()
    {
        GD.Print("Timer timed out");
        CurrentTask.OnFailure();
    }

    private async void NewTask(float time)
    {
        countdown.Stop();
        PreviousTask = CurrentTask;
        CurrentTask.IsActive = false;
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        if (TotalTasksCompleted == 25) music.PlayFastMusic();
        PreviousTask.canFail = true;
        CurrentTask = getRandomTask();
        GD.Print($"Current task - {CurrentTask.GetType()}");
        CurrentTask.BecomeActive();
        countdown.WaitTime = CurrentTimer;
        countdown.Start();
    }

    private async void StartFailGracePeriod(float time)
    {
        await ToSignal(GetTree().CreateTimer(time), "timeout");
        CanAddStrike = true;
    }

    private void IncreaseSpeed()
    {
        CurrentTimer /= 2;
        speedUp.Play();
        NewTask(5);
    }
}
