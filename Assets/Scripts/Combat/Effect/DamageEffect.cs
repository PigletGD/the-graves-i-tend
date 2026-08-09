using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Damage Effect")]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;
    [SerializeField] private CombatantEffectCondition amplifyCondition;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (amplifyCondition.Check(combatant))
        {
            Log($"was amplified by {amplifyCondition.Effect} and dealt {damage * 2f} damage");
            combatant.UpdateHP(-damage * 2f);
            combatant.RemoveEffect(amplifyCondition.Effect);
        }
        else
        {
            Log($"dealt {damage} damage");
            combatant.UpdateHP(-damage);
        }        
    }
}