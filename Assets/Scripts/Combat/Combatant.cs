using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    public static Action<Combatant, float> OnDamageTaken;

    [SerializeField] private bool isPlayerControlled; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CharacterData characterData; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CombatantStats stats; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private SkillSlot[] skills; // Temporary. This should be passed in when creating the combatant.

    [SerializeField] private Combatant[] targets;
    [SerializeField] private TargetRelationship targetRelationship; // Temporary

    private StatusEffectController statusEffectController;

    public bool IsPlayerControlled => isPlayerControlled;
    public CharacterData CharacterData => characterData;
    public CombatantStats Stats => stats;

    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

    public StatusEffectController EffectController => statusEffectController;

    private void Awake()
    {
        stats.Initialize(characterData);
        statusEffectController = new();

        Visualizer?.SetToUnselectedColor();
    }

    public void TakeDamage(float hp)
    {
        stats.UpdateHP(-hp);
        OnDamageTaken?.Invoke(this, hp);
    }

    public bool TryUseSkill(int index, TargetSelectionArgs args)
    {
        if (index < 0 || index >= skills.Length)
            return false;

        return skills[index].TryUse(args);
    }

    // TODO: Refactor this so that we get targets from the selection.
    public ITarget[] GetTargets(Combat _)
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
