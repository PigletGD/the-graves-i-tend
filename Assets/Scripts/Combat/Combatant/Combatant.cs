using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    public const int SkipTurnIndex = -2;
    public const int BasicAttackIndex = -1;

    public static event Action<Combatant, float> OnHPChanged;
    public static event Action<Combatant, float> OnMPChanged;
    public static event Action<Combatant, Skill> OnSkillUsed;

    [SerializeField] private bool isPlayerControlled; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CharacterData characterData; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CombatantStats stats; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private TargetRelationshipType allegiance; // Temporary. This should be passed in when creating the combatant.

    [SerializeField] private Combatant[] targets;

    private CombatantStatusEffects statusEffectsController;

    public bool IsPlayerControlled => isPlayerControlled;
    public bool IsAlive => stats.CurrentHP > 0;

    public CharacterData CharacterData => characterData;
    public CombatantStats Stats => stats;

    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

    public CombatantStatusEffects StatusEffects => statusEffectsController;

    private void Awake()
    {
        stats.Initialize(characterData);
        statusEffectsController = new();

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

    private void SkipTurn(TargetSelectionArgs args)
    {
        characterData.SkipTurn.Execute(args);
        OnSkillUsed?.Invoke(this, characterData.SkipTurn);
    }

    private void BasicAttack(TargetSelectionArgs args)
    {
        characterData.BasicAttack.Execute(args);
        OnSkillUsed?.Invoke(this, characterData.BasicAttack);
    }

    public bool TryUseSkill(int index, TargetSelectionArgs args)
    {
        if (index == SkipTurnIndex)
        {
            SkipTurn(args);
            return true;
        }

        if (index == BasicAttackIndex)
        {
            BasicAttack(args);
            return true;
        }

        if (index < 0 || index >= characterData.Skills.Length)
            return false;

        if (!characterData.Skills[index].CanExecute(args))
            return false;

        Skill skill = characterData.Skills[index];
        skill.Execute(args);
        OnSkillUsed?.Invoke(this, skill);
        return true;
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

    public TargetRelationshipType GetAllegiance() => allegiance;

    public TargetRelationshipType GetTargetRelationshipTo(ITarget other)
    {
        if (allegiance == TargetRelationshipType.None)
            return TargetRelationshipType.None;

        TargetRelationshipType otherRelationship = other.GetAllegiance();

        if (otherRelationship == TargetRelationshipType.None)
            return TargetRelationshipType.None;

        return allegiance == otherRelationship ? TargetRelationshipType.Friendly : TargetRelationshipType.Hostile;
    }
}
