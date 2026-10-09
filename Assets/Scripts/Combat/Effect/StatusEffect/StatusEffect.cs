using UnityEngine;

/// <summary>
/// Base class for applying status effect to a target.
/// </summary>
public abstract class StatusEffect
{
    protected StatusEffectType statusEffectType;

    public abstract StatusEffectType StatusEffectType { get; }

    public abstract void Apply(ITarget target);

    public virtual void OnTurnStart(Combatant combatant) { }
    public virtual void OnTurnEnd(Combatant combatant) { }

    // Just to standardize logging.
    public virtual void Log(object message)
    {
        Debug.Log($"{GetType().Name} {message}!");
    }
}