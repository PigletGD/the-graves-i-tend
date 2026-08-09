using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Damage Effect")]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;

    public override void Apply(Battle battle)
    {
        // TO DO: Looks kinda weird. Remove Attacker getter from Battle. 
        // The Effect class should already know who/what the target is (if it should be self/enemy/multiple targets).
        // From there it should resolve what to target in the battle param.
        // May need to have targets as array in any case the move hits multiple people.

        //battle.Attacker.GetTarget(battle).UpdateHP(-damage);
        
        // TODO: We'll need to figure out at what level of the skill the target gets chosen.
        // I believe that it should probably maybe at the skill level (or maybe the attempt level),
        // depending on how we sequence the skills (like from an animation). Could also be in attempt.
        if (battle.Attacker == null)
            return;

        if (battle.Targets == null || battle.Targets.Count == 0)
            return;

        foreach (var target in battle.Targets)
            target.UpdateHP(-damage);
    }
}