using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Basic", menuName = "Skills/Skill_Basic")]
public class Skill_Basic : Skill
{
    [SerializeField] private TargetedAttempts[] targetedAttempts;

    public override void Execute(TargetSelectionArgs value)
    {
        foreach (TargetedAttempts targetedAttempt in targetedAttempts)
        {
            foreach (ITarget target in value.Targets)
            {
                if (target.GetTargetRelationship() != targetedAttempt.targetRelationship)
                    continue;

                foreach (Attempt attempt in targetedAttempt.attempts)
                    attempt.Execute(value.Battle, value.Invoker, target);
            }
        }
    }
}
