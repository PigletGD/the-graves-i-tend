using UnityEngine;

public enum TargetSelectionMode
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
    [SerializeField] private TargetSelectionMode visualTargetingMode = TargetSelectionMode.SingleEnemy;
    [SerializeReference, SerializeReferenceDropdown] protected Attempt[] attempts;

    public TargetSelectionMode VisualTargetingMode => visualTargetingMode;

    public abstract bool CanExecute(TargetSelectionArgs args);
    public abstract bool Execute(TargetSelectionArgs args);
}