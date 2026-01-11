using UnityEngine;

public abstract class SceneStep : ScriptableObject
{
    public virtual bool WaitsForCompletion => false;
    public abstract void Execute();
}