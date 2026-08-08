using System;
using UnityEngine;

[Serializable]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;

    public override void Apply(Battle battle)
    {
        // TO DO: Looks kinda weird. Remove Attacker getter from Battle. 
        // The Effect class should already know who/what the target is (if it should be self/enemy/multiple targets).
        // From there it should resolve what to target in the battle param.
        // May need to have targets as array in any case the move hits multiple people.

        battle.Attacker.GetTarget(battle).UpdateHP(-damage);
    }
}