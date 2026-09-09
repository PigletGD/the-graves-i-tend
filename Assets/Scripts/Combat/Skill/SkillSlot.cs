using System;
using UnityEngine;

// TODO: This should be separated into two layers. Combat and Overworld where Combat should only store the Skill and reqs(?) while Overworld is if it's learned and locked.
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