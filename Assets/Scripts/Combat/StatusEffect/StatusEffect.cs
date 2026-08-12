/// <summary>
/// Base class for applying status effect to a target.
/// </summary>
public abstract class StatusEffect : Effect
{
    public abstract StatusEffectType StatusEffectType { get; }
}