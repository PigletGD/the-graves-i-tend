using System;
using UnityEngine;

[Serializable]
public class SkillSlot
{
    [SerializeField] private Skill skill;
    [SerializeField] private bool isLearned;
    [SerializeField] private bool isLocked;

    public bool IsUsable => isLearned && !isLocked;

    public bool TryUse(TargetSelectionArgs args)
    {
        if (!IsUsable)
            return false;

        skill.Execute(args);
        return true;
    }

    public void SetLearned(bool value)
    {
        isLearned = value;
    }

    public void SetLocked(bool value)
    {
        isLocked = value;
    }
}