using UnityEngine;

/// <summary>
/// Basic Skill that has costs for the invoker and attempts to targets (that aren't the same relationship).
/// </summary>
[CreateAssetMenu(fileName = "Basic Skill", menuName = "Skills/Basic Skill")]
public class Skill_Basic : Skill
{
    [SerializeField] private SkillCost[] skillCosts;
    [SerializeField] private TargetedAttempts[] targetedAttempts;

    public override void Execute(TargetSelectionArgs value)
    {
        foreach (SkillCost skillCost in skillCosts)
        {
            if (!skillCost.Check(value.Invoker))
            {
                Debug.Log($"{GetType().Name} failed due to {skillCost.ResourceType}!");
                return;
            }
        }

        foreach (SkillCost skillCost in skillCosts)
            skillCost.Apply(value.Invoker);

        foreach (TargetedAttempts targetedAttempt in targetedAttempts)
        {
            foreach (ITarget target in value.Targets)
            {
                if (target.GetTargetRelationshipTo(value.Invoker) != targetedAttempt.targetRelationship)
                    continue;

                foreach (Attempt attempt in targetedAttempt.attempts)
                    attempt.Execute(value.Combat, value.Invoker, target);
            }
        }
    }
}
