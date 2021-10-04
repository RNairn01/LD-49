using Godot;
using System;

public class SceneManager : Node
{
    private AudioStreamPlayer menuClick;

    public override void _Ready()
    {
        menuClick = GetNode<AudioStreamPlayer>("MenuClick");
    }
    public void LoadMainMenu()
    {
        GetTree().ChangeScene("res://src/scenes/MainMenu.tscn");
        menuClick.Play();
    }
    public void PlayGame()
    {
        GetTree().ChangeScene("res://src/scenes/Level.tscn");
        menuClick.Play();
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
