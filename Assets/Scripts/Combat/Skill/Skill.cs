using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public abstract bool CanExecute(TargetSelectionArgs args);
    public abstract void Execute(TargetSelectionArgs args);
}