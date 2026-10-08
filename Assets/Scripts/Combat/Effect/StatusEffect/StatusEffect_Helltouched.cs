using UnityEngine;

public class StatusEffect_Helltouched : TimedStackableStatusEffect
{
    private float damagePerStack;

    public override StatusEffectType StatusEffectType => statusEffectType;

    public StatusEffect_Helltouched(StatusEffectSO_Helltouched source)
    {
        currentStacks = 1;
        statusEffectType = source.StatusEffectType;
        maxStacks = source.MaxStacks;
        damagePerStack = source.DamagePerStack;
        InitializeDuration(source.TurnDuration, source.CanRefreshOnApply);
    }

    public override void OnTurnEnd(Combatant combatant)
    {
        combatant.TakeDamage(damagePerStack * StackCount);
        AdvanceTurnDuration();
    }

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} is affected by {StatusEffectType} ({currentStacks}/{maxStacks} stacks).");
        }
    }
}