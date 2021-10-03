using Godot;
using System;

public class Cursor : Node
{
    public static bool IsHoldingSomething = false;
    
    public override void _Ready()
    {
        var arrow = ResourceLoader.Load("res://src/assets/sprites/cursor/cursorsmall.png");
        var beam = ResourceLoader.Load("res://src/assets/sprites/cursor/cursor-clickedsmall.png");

        Input.SetCustomMouseCursor(arrow);

        Input.SetCustomMouseCursor(beam, Input.CursorShape.Drag);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("click") || Cursor.IsHoldingSomething) Input.SetDefaultCursorShape(Input.CursorShape.Drag);
        else Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
    }
}
