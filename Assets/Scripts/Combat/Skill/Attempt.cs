using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Attempt
{
    public static Action<TargetSelectionArgs, ISkillCondition> OnAttemptFailed;

    [SerializeField] private TargetSelectionMode targetingMode = TargetSelectionMode.SingleEnemy;
    [SerializeReference, SerializeReferenceDropdown] public Effect[] effects;

    public void Execute(TargetSelectionArgs args)
    {
        ITarget[] targets = ResolveTargets(args);
        ExecuteAttempt(args, targets);
    }

    protected virtual void ExecuteAttempt(TargetSelectionArgs args, ITarget[] targets)
    {
        foreach (ITarget target in targets)
        {
            foreach (Effect effect in effects)
            {
                effect.Apply(target);
            }
        }
    }

    private ITarget[] ResolveTargets(TargetSelectionArgs args)
    {
        return targetingMode switch
        {
            TargetSelectionMode.None => Array.Empty<ITarget>(),
            TargetSelectionMode.SingleEnemy => GetInitialTarget(args, false),
            TargetSelectionMode.AllEnemies => GetCombatants(args, false),
            TargetSelectionMode.SingleAlly => GetInitialTarget(args, true),
            TargetSelectionMode.AllAllies => GetCombatants(args, true),
            TargetSelectionMode.Self => new[] { args.Invoker },
            _ => throw new ArgumentOutOfRangeException(nameof(targetingMode), targetingMode, "Unsupported attempt targeting mode."),
        };
    }

    private ITarget[] GetInitialTarget(TargetSelectionArgs args, bool isAlly)
    {
        if (args.Invoker is not Combatant invoker)
            return null;

        ITarget target = args.Targets
            .FirstOrDefault(candidate => candidate is Combatant combatant && combatant.IsPlayerControlled == (invoker.IsPlayerControlled == isAlly));
            
        if (target == null)
        {
            Debug.LogWarning($"No selected target matches the attempt's ally setting (ally: {isAlly}).");
            return null;
        }

        return new[] { target };
    }

    private ITarget[] GetCombatants(TargetSelectionArgs args, bool isAlly)
    {
        if (args.Invoker is not Combatant invoker)
            return null;

        IEnumerable<Combatant> combatants = invoker.IsPlayerControlled == isAlly
            ? args.Combat.PlayerCombatants
            : args.Combat.EnemyCombatants;

        return combatants.Cast<ITarget>().ToArray();
    }
}
