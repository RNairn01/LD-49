using Godot;
using System;

public class Soul : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    public bool canFail { get; set; } = false;
    private bool holdingSoul, soulIsFalling, soulCanBeDropped = false;
    private Vector2 startPosition;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../SoulJar").GlobalPosition;
        inputState = InputStates.InputState.MoreSoulState;
        VoiceLinesNormal = PopulateNormalLine("soul");
        VoiceLinesQuick = PopulateQuickLine("soul");
    }

    public override void _Process(float delta)
    {
        if (holdingSoul && soulCanBeDropped && Input.IsActionJustPressed("click")) DropSoul();
        else if (holdingSoul)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (soulIsFalling)
            GlobalPosition = Drag(GlobalPosition, new Vector2(GlobalPosition.x, GlobalPosition.y + 100), 15 * delta);
    }

    public void OnInteract()
    {
        if (Input.IsActionJustPressed("click"))
        {
            if (!IsActive)
            {
                OnFailure();
                return;
            }
            GD.Print("Picked up soul");
            holdingSoul = true;
            Cursor.IsHoldingSomething = true;
            //Play jar sloshing sound effect
        }
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for more soul task");
        var index = GameManager.Rand.RandiRange(0, VoiceLinesNormal.Count - 1);
        GD.Print(VoiceLinesNormal[index]);
        voice.Stream = GD.Load<AudioStream>(VoiceLinesNormal[index]);
        voice.Play();
    }

    public void ChangeAlchemistState()
    {
        alchemist.ChangeState(Alchemist.AlchemistState.Thinking);
    }

    public void OnFailure()
    {
        if (gameManager.CanAddStrike && canFail)
        {
            canFail = false;
            gameManager.AddStrike("Soul task failed");
            gameManager.GetNewTask();
            var index = GameManager.Rand.RandiRange(0, FailLines.Count - 1);
            GD.Print(FailLines[index]);
            angerVoice.Stream = GD.Load<AudioStream>(FailLines[index]);
            angerVoice.Play();
            failSmoke.Play("smoke");
        }
    }

    public void OnComplete()
    {
        canFail = false;
        GD.Print("More soul task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        if (gameManager.IsGameOver) return;
        failSmoke.Play("default");
        fire.Play("default");
        canFail = true;
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpSoul(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void SoulExitJar(Area2D area)
    {
        if (area.Name == "SoulArea") soulCanBeDropped = true;
    }
    
    public void SoulEnterJar(Area2D area)
    {
        if (area.Name == "SoulArea") soulCanBeDropped = false;
    }

    public void OnSoulCollide(Area2D area)
    {
        GD.Print("Soul collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop")
        {
            GD.Print("Soul dropped in cauldron!");
            if (holdingSoul) return;
            GlobalPosition = startPosition;
            soulIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            soulIsFalling = false;
            OnFailure();
        }
    }

    private void DropSoul()
    {
        holdingSoul = false;
        Cursor.IsHoldingSomething = false;
        soulIsFalling = true;
        //Play drop sound effect
    }
}
