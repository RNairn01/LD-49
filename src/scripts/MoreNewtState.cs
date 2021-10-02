using Godot;
using System;

public class MoreNewtState : AlchemyInput, IAlchemyInput
{
    private bool holdingNewt, newtIsFalling, newtCanBeDropped = false;
    public override void _Ready()
    {
        base._Ready();
        inputState = InputStates.InputState.StirState;
    }

    public override void _Process(float delta)
    {
        if (holdingNewt && newtCanBeDropped && Input.IsActionJustPressed("click")) DropNewt();
        else if (holdingNewt)
        {
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 25 * delta);
        }
    }

    public void OnInteract()
    {
        if (Input.IsActionJustPressed("click"))
        {
            GD.Print("Picked up newt");
            holdingNewt = true;
            //Play jar sloshing sound effect
        }
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for stir task");
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
        gameManager.StrikeCount++;
        GD.Print("More Newt task failed");
        //Play failure voice line
        //Play shaking cauldron animation here
    }

    public void OnComplete()
    {
        GD.Print("More newt task complete!");
        //Earn score
        //Get new task from GameManager
    }

    public void BecomeActive()
    {
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
        if (area.Name == "NewtDrop")
        {
            GD.Print("Newt dropped in cauldron!");
            OnComplete();
        }

        if (area.Name == "Catcher") OnFailure();
    }

    private void DropNewt()
    {
        holdingNewt = false;
        newtIsFalling = true;
        //Play drop sound effect
    }
}
