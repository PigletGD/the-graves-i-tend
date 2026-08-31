using System;
using UnityEngine;

[Serializable]
public class DamageEffect : Effect
{
    [SerializeField] private float damage = 10f;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        Log($"dealt {damage} damage");
        combatant.TakeDamage(damage);
    }
}