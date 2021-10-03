using Godot;
using System;

public interface IAlchemyInput
{
    bool IsActive { get; set; }
    bool canFail { get; set;} 
    void OnInteract();
    void PlayCurrentVoiceLine();
    void ChangeAlchemistState();
    void OnFailure();
    void OnComplete();
    void BecomeActive();
}
