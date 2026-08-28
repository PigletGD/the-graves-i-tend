using UnityEngine;

public abstract class Skill : ScriptableObject, ISkill
{
    public abstract void Execute(TargetSelectionArgs value);
}