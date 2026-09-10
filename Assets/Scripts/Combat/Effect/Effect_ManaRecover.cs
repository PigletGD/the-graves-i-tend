using System;
using UnityEngine;

[Serializable]
public class Effect_ManaRecover : Effect
{
    [SerializeField] private float mana = 2f;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        // Log($"dealt {damage} damage");
        combatant.RecoverMana(mana);
    }
}