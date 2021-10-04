using Godot;
using System;

public class SceneManager : Node
{
    private AudioStreamPlayer menuClick;

    public override void _Ready()
    {
        menuClick = GetNode<AudioStreamPlayer>("MenuClick");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("exit")) GetTree().Quit();
    }
    public void LoadMainMenu()
    {
        menuClick.Play();
        GetTree().ChangeScene("res://src/scenes/MainMenu.tscn");
    }
    public void PlayGame()
    {
        menuClick.Play();
        GetTree().ChangeScene("res://src/scenes/Level.tscn");
    }
    public void GameWinScreen()
    {
        GetTree().ChangeScene("res://src/scenes/GameWin.tscn");
    }
    public void GameOverScreen()
    {
        GetTree().ChangeScene("res://src/scenes/GameOver.tscn");
    }
}
