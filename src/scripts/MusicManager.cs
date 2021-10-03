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
}
