using Godot;
using System;

public class Soul : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    private bool holdingSoul, soulIsFalling, soulCanBeDropped = false;
    private Vector2 startPosition;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../SoulJar").GlobalPosition;
        inputState = InputStates.InputState.MoreSoulState;
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
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to thinking");
        //alchemist.ChangeState(AlchemistState.Thinking);
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        gameManager.AddStrike("More soul task failed", "");
        gameManager.GetNewTask();
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("More soul task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
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
