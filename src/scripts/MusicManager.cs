using Godot;
using System;

public class MusicManager : Node
{
    private AudioStreamPlayer musicPlayer;
    public override void _Ready()
    {
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void PlayFastMusic()
    {
        musicPlayer.Stream = GD.Load<AudioStream>("res://src/assets/music/AlchemyLoopFrantic.ogg");
        musicPlayer.Play();
    }
}
