using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    public static event Action<Combatant, float> OnHealthChanged;
    public static event Action<Combatant, float> OnManaChanged;
    public static event Action<Combatant, Skill> OnSkillUsed;
    public static event Action<Combatant, StatusEffectType> OnTurnSkipped;

    [SerializeField] private bool isPlayerControlled; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CharacterData characterData; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private CombatantStats stats; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private float startingMP = 9; // Temporary. This should be passed in when creating the combatant.
    [SerializeField] private TargetSelectionVisualizer Visualizer;
    [SerializeField] private BoxCollider2D boxCollider;

    private CombatantStatusEffects statusEffectsController;

    public bool IsPlayerControlled => isPlayerControlled;
    public bool CanAct => !IsDead && !IsStunned;
    public bool IsDead => stats.CurrentHP <= 0;
    public bool IsStunned => statusEffectsController.HasStatusEffect(StatusEffectType.Stunned);

    public CharacterData CharacterData => characterData;
    public CombatantStats Stats => stats;
    public BoxCollider2D BoxCollider => boxCollider;


    public CombatantStatusEffects StatusEffects => statusEffectsController;

    private void Awake()
    {
        stats.Initialize(characterData, startingMP);
        statusEffectsController = new();

        Visualizer?.SetToUnselectedColor();
    }

    public bool TryStartTurn()
    {
        StatusEffects.OnTurnStart(this);

        if (!CanAct)
        {
            StatusEffectType reason = IsStunned ? StatusEffectType.Stunned : StatusEffectType.None;
            OnTurnSkipped?.Invoke(this, reason);

            if (IsStunned)
                StatusEffects.RemoveEffect(StatusEffectType.Stunned);

            return false;
        }
        return true;
    }

    public void EndTurn()
    {
        StatusEffects.OnTurnEnd(this);
    }

    public Skill GetSkillFromActionType(CombatActionType actionType, int actionIndex)
    {
        return actionType switch
        {
            CombatActionType.Attack => characterData.BasicAttack,
            CombatActionType.Skill => characterData.Skills[actionIndex],
            CombatActionType.Item => characterData.Items[actionIndex], // TODO: Implement item usage.
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
            Targets = args.Targets,
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

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }

    #region Stats
    public float GetResourceAmount(CombatantResourceType resourceType) => stats.GetResourceAmount(resourceType);
    public void UpdateResource(CombatantResourceType resourceType, float amount)
    {
        stats.UpdateResource(resourceType, amount);

        if (resourceType == CombatantResourceType.Health)
            OnHealthChanged?.Invoke(this, amount);
        else if (resourceType == CombatantResourceType.Mana)
            OnManaChanged?.Invoke(this, amount);
    }
    
    public void RecoverHealth(float health)
    {
        stats.UpdateResource(CombatantResourceType.Health, health);
        OnHealthChanged?.Invoke(this, health);
    }

    public void TakeDamage(float health)
    {
        stats.UpdateResource(CombatantResourceType.Health, -health);
        OnHealthChanged?.Invoke(this, -health);
    }

    public void RecoverMana(float mana)
    {
        stats.UpdateResource(CombatantResourceType.Mana, mana);
        OnManaChanged?.Invoke(this, mana);
    }
    
    public void ConsumeMana(float mana)
    {
        stats.UpdateResource(CombatantResourceType.Mana, -mana);
        OnManaChanged?.Invoke(this, -mana);
    }
    #endregion
}
