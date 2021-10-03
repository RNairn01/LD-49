using Godot;
using System;

public class Emerald : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    private bool holdingEmerald, emeraldIsFalling, emeraldCanBeDropped = false;
    private Vector2 startPosition;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../EmeraldJar").GlobalPosition;
        inputState = InputStates.InputState.MoreEmeraldState;
    }

    public override void _Process(float delta)
    {
        if (holdingEmerald && emeraldCanBeDropped && Input.IsActionJustPressed("click")) DropEmerald();
        else if (holdingEmerald)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (emeraldIsFalling)
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
            GD.Print("Picked up emerald");
            holdingEmerald = true;
            Cursor.IsHoldingSomething = true;
            //Play jar sloshing sound effect
        }
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for more emerald task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to thinking");
        alchemist.ChangeState(Alchemist.AlchemistState.Thinking);
    }

    public void OnFailure()
    {
        gameManager.AddStrike("Emerald task failed", "");
        //Play shaking cauldron animation here
        gameManager.GetNewTask();
    }

    public void OnComplete()
    {
        GD.Print("More emerald task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpEmerald(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void EmeraldExitJar(Area2D area)
    {
        if (area.Name == "EmeraldArea") emeraldCanBeDropped = true;
    }
    
    public void EmeraldEnterJar(Area2D area)
    {
        if (area.Name == "EmeraldArea") emeraldCanBeDropped = false;
    }

    public void OnEmeraldCollide(Area2D area)
    {
        GD.Print("Emerald collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop")
        {
            GD.Print("Emerald dropped in cauldron!");
            if (holdingEmerald) return;
            GlobalPosition = startPosition;
            emeraldIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            emeraldIsFalling = false;
            OnFailure();
        }
    }

    private void DropEmerald()
    {
        holdingEmerald = false;
        Cursor.IsHoldingSomething = false;
        emeraldIsFalling = true;
        //Play drop sound effect
    }
}
