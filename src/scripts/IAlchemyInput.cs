using Godot;
using System;

public interface IAlchemyInput
{
    void OnInteract();
    void PlayCurrentVoiceLine();
    void ChangeAlchemistState();
    void OnFailure();
    void OnComplete();
    void BecomeActive();
}
