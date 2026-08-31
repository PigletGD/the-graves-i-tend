using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private CombatantStats stats;
    [SerializeField] private SkillSlot[] skills;

    [SerializeField] private Combatant[] targets;
    [SerializeField] private TargetRelationship targetRelationship; // Temporary

    private StatusEffectController statusEffectController;

    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

    public StatusEffectController EffectController => statusEffectController;

    private void Awake()
    {
        Visualizer?.SetToUnselectedColor();
    }

    private void Start()
    {
        stats.Initialize();
        statusEffectController = new();
    }

    public void TakeDamage(float hp) => stats.UpdateHP(-hp);

    public bool TryUseSkill(int index, TargetSelectionArgs args)
    {
        if (index < 0 || index >= skills.Length)
            return false;

        return skills[index].TryUse(args);
    }

    // TODO: Refactor this so that we get targets from the selection.
    public ITarget[] GetTargets(Battle _)
    {
        return targets;
    }

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }

    public TargetRelationship GetTargetRelationship()
    {
        return targetRelationship;
    }
}
