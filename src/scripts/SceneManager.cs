using Godot;
using System;

public class SceneManager : Node
{
    public void LoadMainMenu()
    {
        GetTree().ChangeScene("res://src/scenes/MainMenu.tscn");
    }

    public void StartGame()
    {
        GetTree().ChangeScene("res://src/scenes/Level.tscn");
    }
}
