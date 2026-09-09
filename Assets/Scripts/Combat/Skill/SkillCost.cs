using System;
using UnityEngine;

// Could honestly have separated this into Effect and Condition but let's simplify checking and consuming resources for skills instead of having to input it twice.
[Serializable]
public class SkillCost : ICondition<ITarget>
{
    [SerializeField] private CombatantResourceType resourceType;
    [SerializeField] private float value = 10f;

    public CombatantResourceType ResourceType => resourceType;

    public void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Debug.Log($"{GetType().Name} failed because Target is not a combatant!");
            return;
        }

        switch (resourceType)
        {
            case CombatantResourceType.HP:
                combatant.TakeDamage(value);
                break;
            case CombatantResourceType.MP:
                combatant.ConsumeMana(value);
                break;
        }
    }

    public bool Check(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Debug.Log($"{GetType().Name} failed because Target is not a combatant!");
            return false;
        }

        return resourceType switch
        {
            CombatantResourceType.HP => combatant.Stats.CurrentHP >= value,
            CombatantResourceType.MP => combatant.Stats.CurrentMP >= value,
            CombatantResourceType.Elation => combatant.Stats.CurrentElation >= value,
            _ => false,
        };
    }
}