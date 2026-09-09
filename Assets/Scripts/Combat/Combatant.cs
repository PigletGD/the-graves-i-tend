using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    public static Action<Combatant, float> OnHPChanged;
    public static Action<Combatant, float> OnMPChanged;
    public static Action<Combatant> OnSkillUsed;

    [SerializeField] private bool isPlayerControlled; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CharacterData characterData; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CombatantStats stats; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private SkillSlot[] skills; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private TargetRelationshipType targetRelationship; // Temporary. This should be passed in when creating the combatant.

    [SerializeField] private Combatant[] targets;

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

#region Stat Updates
    public void TakeDamage(float hp)
    {
        stats.UpdateHP(-hp);
        OnHPChanged?.Invoke(this, -hp);
    }

    public void ConsumeMana(float mp)
    {
        stats.UpdateMP(-mp);
        OnMPChanged?.Invoke(this, -mp);
    }

    public void RecoverMana(float mp)
    {
        stats.UpdateMP(mp);
        OnMPChanged?.Invoke(this, mp);
    }
#endregion

    public bool TryUseSkill(int index, TargetSelectionArgs args)
    {
        if (index < 0 || index >= skills.Length)
            return false;

        bool wasSkillUsed = skills[index].TryUse(args);

        if (wasSkillUsed)
            OnSkillUsed?.Invoke(this);

        return wasSkillUsed;
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

    public TargetRelationshipType GetTargetRelationship()
    {
        return targetRelationship;
    }
}
