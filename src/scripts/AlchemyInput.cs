using Godot;
using System;

public abstract class AlchemyInput : Sprite
{
    protected GameManager gameManager;
    protected InputStates.InputState inputState;
    protected string[] VoiceLinesTutorial;
    protected string[] VoiceLinesNormal;
    protected string[] VoiceLinesQuick;
}
