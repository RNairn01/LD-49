using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class SaltState : AlchemyInput, IAlchemyInput
{
    [Export] public bool IsActive { get; set;} = false;
    private bool holdingShaker = false;
    private bool movingDown = false;
    private int timesSaltReleased = 0;
    private Queue<Vector2> previousPositions = new Queue<Vector2>();
    private CPUParticles2D particleEmitter;
    
    private float cauldronLeftmostX;
    private float cauldronRightmostX;
    private float cauldronTopY;

    public override void _Ready()
    {
        AnimatedSprite cauldron = GetNode<AnimatedSprite>("../../Cauldron");
        Texture cauldronCurrentFrame = cauldron.Frames.GetFrame("default", cauldron.Frame);
        float cauldronWidth = cauldronCurrentFrame.GetWidth();
        float cauldronHeight = cauldronCurrentFrame.GetHeight();
        cauldronLeftmostX = cauldron.GlobalPosition.x - (cauldronWidth / 2);
        cauldronRightmostX = cauldronLeftmostX + cauldronWidth;
        cauldronTopY = cauldron.GlobalPosition.y - (cauldronHeight / 2);

        particleEmitter = GetNode<CPUParticles2D>("./SaltParticleEmitter");

        base._Ready();
        inputState = InputStates.InputState.MoreSaltState;
    }

    public override void _Process(float delta)
    {
        if (holdingShaker)
        {
            if (previousPositions.Count >= 5) previousPositions.Dequeue(); // Store the positions of the shaker over the past 5 frames.
            previousPositions.Enqueue(new Vector2(GlobalPosition));    // Calculate average velocity. If we're moving down at a decent speed, set movingDown to True.
            Vector2 velocity = calculateVelocity(previousPositions);       // If we change direction and start moving up, then release the salt. The requirement to be going down
            if (velocity.y > 5) movingDown = true;                         // pretty fast forces the player to shake the mouse vigorously.
            else if (movingDown && velocity.y < 0)
            {
                movingDown = false;
                ReleaseSalt();
            }

            GlobalRotationDegrees = 20;
            GlobalPosition = Drag(GlobalPosition, GetGlobalMousePosition(), 100 * delta);
            // Figure out if we shook it
        } else
        {
            GlobalRotationDegrees = 0;
            GlobalPosition = Drag(GlobalPosition, GetNode<Node2D>("..").GlobalPosition, 5 * delta);
        }
    }
    
    public void OnInteract()
    {
        if (!Input.IsActionJustPressed("click")) return;
        if (!IsActive) OnFailure();
        else if (!holdingShaker) {
            GD.Print("Picked up salt shaker");
            holdingShaker = true;
            Cursor.IsHoldingSomething = true;
        } else
        {
            GD.Print("Dropped salt shaker");
            holdingShaker = false;
            Cursor.IsHoldingSomething = false;
        }
    }

    public void ReleaseSalt()
    {
        GD.Print("Releasing salt");
        particleEmitter.Emitting = true;
        if (isAboveCauldron()) // Is salt shaker above cauldron
        {
            GD.Print("Salt released above cauldron");
            timesSaltReleased++;
            GD.Print("Salted " + timesSaltReleased + " times.");
            // Release salt animation
            if (timesSaltReleased >= 3)
            {
                OnComplete();
            }
        } else 
        {
            GD.Print("Salt released away from cauldron");
        }
    }

    public void PlayCurrentVoiceLine()
    {
        GD.Print("Play voice line for salt task");
        //Play voice line here
    }

    public void ChangeAlchemistState()
    {
        GD.Print("Change alchemist state to pointing");
        //alchemist.ChangeState(AlchemistState.Pointing);
        //Change Alchemist state here
    }

    public void OnFailure()
    {
        holdingShaker = false;
        Cursor.IsHoldingSomething = false;
        gameManager.AddStrike("Salt task failed", "");
        //Play shaking cauldron animation here
        gameManager.GetNewTask();
    }

    public void OnComplete()
    {
        timesSaltReleased = 0;
        holdingShaker = false;
        Cursor.IsHoldingSomething = false;
        GD.Print("Salt task complete!");
        //Earn score
        gameManager.GetNewTask();
    }

    public void BecomeActive()
    {
        IsActive = true;
        PlayCurrentVoiceLine();
        ChangeAlchemistState();
    }
    
    public void OnClick(Node viewport, InputEvent @event, int shapeIdx)
    {
        OnInteract();
    }

    private bool isAboveCauldron()
    {
        return GlobalPosition.y < cauldronTopY 
               && GlobalPosition.x > cauldronLeftmostX 
               && GlobalPosition.x < cauldronRightmostX;
    }

    private Vector2 calculateVelocity(Queue<Vector2> previousPositions)
    {
        Vector2 oldestPosition = previousPositions.First();
        Vector2 newestPosition = previousPositions.Last();
        return (newestPosition - oldestPosition) / 5;
    }
}
