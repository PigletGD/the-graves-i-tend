using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    public static event Action<Combatant, float> OnHPChanged;
    public static event Action<Combatant, float> OnMPChanged;
    public static event Action<Combatant, Skill> OnSkillUsed;

    [SerializeField] private bool isPlayerControlled; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CharacterData characterData; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CombatantStats stats; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private TargetRelationshipType allegiance; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private TargetSelectionVisualizer Visualizer;
    [SerializeField] private Combatant[] targets; // TODO: Might be obsolete/deprecated.

    private CombatantStatusEffects statusEffectsController;

    public bool IsPlayerControlled => isPlayerControlled;
    public bool IsAlive => stats.CurrentHP > 0;

    public CharacterData CharacterData => characterData;
    public CombatantStats Stats => stats;


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
        stats.UpdateResource(CombatantResourceType.HP, -hp);
        OnHPChanged?.Invoke(this, -hp);
    }

    public void ConsumeMana(float mp)
    {
        stats.UpdateResource(CombatantResourceType.MP, -mp);
        OnMPChanged?.Invoke(this, -mp);
    }

    public void RecoverMana(float mp)
    {
        stats.UpdateResource(CombatantResourceType.MP, mp);
        OnMPChanged?.Invoke(this, mp);
    }
    #endregion

    public Skill GetSkillFromActionType(CombatActionType actionType, int actionIndex)
    {
        return actionType switch
        {
            CombatActionType.Attack => characterData.BasicAttack,
            CombatActionType.Skill => characterData.Skills[actionIndex],
            CombatActionType.Item => null, // TODO: Implement item usage.
            CombatActionType.Skip => characterData.SkipTurn,
            _ => null
        };
    }

    public bool TryUseSkill(Combat combat, CombatantActionArgs args)
    {
        TargetSelectionArgs targetSelectionArgs = new()
        {
            Combat = combat,
            Invoker = this,
            Targets = args.Targets
        };

        Skill skill = GetSkillFromActionType(args.ActionType, args.ActionIndex);
        if (skill.CanExecute(targetSelectionArgs))
        {
            skill.Execute(targetSelectionArgs);
            OnSkillUsed?.Invoke(this, skill);
            return true;
        }
        
        return false;
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



    public float GetResourceAmount(CombatantResourceType resourceType) => stats.GetResourceAmount(resourceType);
    public void UpdateResource(CombatantResourceType resourceType, float amount) => stats.UpdateResource(resourceType, amount);
}
