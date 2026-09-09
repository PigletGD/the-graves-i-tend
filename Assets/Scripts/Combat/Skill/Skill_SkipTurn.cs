using UnityEngine;

[CreateAssetMenu(fileName = "Skip Turn", menuName = "Skills/Skip Turn")]
public class Skill_SkipTurn : Skill
{
    [SerializeField] private Attempt skipAttempt;

    public override void Execute(TargetSelectionArgs value)
    {
        skipAttempt.Execute(null, value.Invoker, null);
    }
}