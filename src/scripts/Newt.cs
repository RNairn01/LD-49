using Godot;
using System;

public class Newt : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    private bool holdingNewt, newtIsFalling, newtCanBeDropped = false;
    private Vector2 startPosition;
    public override void _Ready()
    {
        base._Ready();
        startPosition = GetNode<Node2D>("../NewtJar").GlobalPosition;
        inputState = InputStates.InputState.MoreNewtState;
    }

    public override void _Process(float delta)
    {
        if (holdingNewt && newtCanBeDropped && Input.IsActionJustPressed("click")) DropNewt();
        else if (holdingNewt)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }

        if (newtIsFalling)
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
            GD.Print("Picked up newt");
            holdingNewt = true;
            Cursor.IsHoldingSomething = true;
            //Play jar sloshing sound effect
        }
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for more newt task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to thinking");
        alchemist.ChangeState(Alchemist.AlchemistState.Thinking);
    }

    public void OnFailure()
    {
        gameManager.AddStrike("More newt task failed", "");
        gameManager.GetNewTask();
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("More newt task complete!");
        gameManager.AddScore();
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }

    public void PickUpNewt(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    public void NewtExitJar(Area2D area)
    {
        if (area.Name == "NewtArea") newtCanBeDropped = true;
    }
    
    public void NewtEnterJar(Area2D area)
    {
        if (area.Name == "NewtArea") newtCanBeDropped = false;
    }

    public void OnNewtCollide(Area2D area)
    {
        GD.Print("Newt collided with: " + area.Name);
        
        if (area.Name == "CauldronDrop")
        {
            GD.Print("Newt dropped in cauldron!");
            if (holdingNewt) return;
            GlobalPosition = startPosition;
            newtIsFalling = false;
            OnComplete();
        }

        if (area.Name == "Catcher")
        {
            GlobalPosition = startPosition;
            newtIsFalling = false;
            OnFailure();
        }
    }

    private void DropNewt()
    {
        holdingNewt = false;
        Cursor.IsHoldingSomething = false;
        newtIsFalling = true;
        //Play drop sound effect
    }
}
