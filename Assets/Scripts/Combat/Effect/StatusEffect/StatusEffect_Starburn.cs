using UnityEngine;

public class StatusEffect_Starburn : TimedStackableStatusEffect
{
    private float damagePerStack;

    public override StatusEffectType StatusEffectType => statusEffectType;

    public StatusEffect_Starburn(StatusEffectSO_Starburn source)
    {
        currentStacks = 1;
        statusEffectType = source.StatusEffectType;
        maxStacks = source.MaxStacks;
        damagePerStack = source.DamagePerStack;
        InitializeDuration(source.TurnDuration, source.CanRefreshOnApply);
    }

    public override void OnTurnStart(Combatant combatant)
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