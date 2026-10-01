using UnityEngine;

public enum SkillTargetingMode
{
    None,
    SingleEnemy,
    AllEnemies,
    SingleAlly,
    AllAllies,
    Self
}

public abstract class Skill : ScriptableObject
{
    [SerializeField] private SkillTargetingMode skillTargetingMode = SkillTargetingMode.SingleEnemy;

    public SkillTargetingMode SkillTargetingMode => skillTargetingMode;

    public abstract bool CanExecute(TargetSelectionArgs args);
    public abstract void Execute(TargetSelectionArgs args);
}