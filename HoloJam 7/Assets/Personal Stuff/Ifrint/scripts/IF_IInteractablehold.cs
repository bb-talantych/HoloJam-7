using BB_CommonStuff;
using UnityEngine;

public interface IF_IInteractablehold
{
    Vector3 MovePoint { get; }
    bool IsAvailable { get; }

    void OnEnter(Stats stats);
    void OnExit();
}
