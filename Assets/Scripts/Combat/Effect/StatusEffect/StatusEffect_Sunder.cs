using UnityEngine;

public class StatusEffect_Sunder : StackableStatusEffect
{
    public override StatusEffectType StatusEffectType => statusEffectType;

    public StatusEffect_Sunder(StatusEffectSO_Sunder source)
    {
        currentStacks = 1;
        statusEffectType = source.StatusEffectType;
        maxStacks = source.MaxStacks;
    }

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} is affected by {StatusEffectType} ({currentStacks}/{maxStacks} stacks).");
        }
    }
}
