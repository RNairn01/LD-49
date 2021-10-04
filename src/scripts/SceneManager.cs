using Godot;
using System;

public class SceneManager : Node
{
    public void LoadMainMenu()
    {
        GetTree().ChangeScene("res://src/scenes/MainMenu.tscn");
    }

    public void PlayGame()
    {
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
