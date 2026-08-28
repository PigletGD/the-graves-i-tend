using UnityEngine;

[CreateAssetMenu(fileName = "OffensiveSkill", menuName = "Skills/OffensiveSkill")]
public class OffensiveSkill : Skill
{
    [SerializeField] private Attempt[] attempts;

    public override void Execute(TargetSelectionArgs value)
    {
        foreach (IAttempt attempt in attempts)
        {
            foreach (ITarget target in value.Targets)
            {
                attempt.Execute(value.Battle, value.Invoker, target);
            }
        }
    }
}
