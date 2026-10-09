using System;
using UnityEngine;

[Serializable]
public class SkillCondition_HasStatusEffect : ISkillCondition
{
    [SerializeField] private StatusEffectType statusEffectType;
    [SerializeField] private int stacksRequired = 1;

    public bool Check(TargetSelectionArgs value)
    {
        foreach (ITarget target in value.Targets)
        {
            if (target is not Combatant combatant)
                return false;

            if (stacksRequired > combatant.StatusEffects.GetStackCount(statusEffectType))
                return false;
        }

        return true;
    }
}
