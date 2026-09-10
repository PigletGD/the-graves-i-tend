using UnityEngine;

/// <summary>
/// Basic Skill that has costs for the invoker and attempts to targets (that aren't the same relationship).
/// </summary>
[CreateAssetMenu(fileName = "Basic Skill", menuName = "Skills/Basic Skill")]
public class Skill_Basic : Skill
{
    [SerializeField] private SkillCost[] skillCosts;
    [SerializeField] private TargetedAttempts[] targetedAttempts;

    public override bool CanExecute(TargetSelectionArgs args)
    {
        foreach (SkillCost skillCost in skillCosts)
        {
            if (!skillCost.Check(args.Invoker))
            {
                Debug.Log($"{GetType().Name} cannot be executed due to {skillCost.ResourceType}!");
                return false;
            }
        }

        return true;
    }

    public override void Execute(TargetSelectionArgs args)
    {
        foreach (SkillCost skillCost in skillCosts)
            skillCost.Apply(args.Invoker);

        foreach (TargetedAttempts targetedAttempt in targetedAttempts)
        {
            foreach (ITarget target in args.Targets)
            {
                if (target.GetTargetRelationshipTo(args.Invoker) != targetedAttempt.targetRelationship)
                    continue;

                foreach (Attempt attempt in targetedAttempt.attempts)
                    attempt.Execute(args.Combat, args.Invoker, target);
            }
        }
    }
}
