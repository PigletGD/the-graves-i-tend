using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public abstract void Execute(TargetSelectionArgs value);
}