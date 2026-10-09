using System;
using UnityEngine;

[Serializable]
public class Effect_ManaRecover : Effect, ICanOverrideTarget
{
    [SerializeField] private float mana = 2f;
    [SerializeField] private bool shouldOverrideTarget = false;
    [SerializeField] private TargetSelectionMode mode = TargetSelectionMode.Self;

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

    public bool ShouldOverrideTarget()
    {
        return shouldOverrideTarget;
    }

    public TargetSelectionMode GetOverridableTargetSelection()
    {
        return mode;
    }
}