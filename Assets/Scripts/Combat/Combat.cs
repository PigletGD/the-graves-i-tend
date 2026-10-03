using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
// TODO: SkillSlot.cs
public class Combat : MonoBehaviour
{
    public static event Action<Combatant> OnTurnStarted;
    public static event Action<Combatant> OnTurnEnded;
    public static event Action<bool> OnCombatEnded;

    [SerializeField] private CombatArena arena;
    [SerializeField] private CombatUI ui;
    [SerializeField] private TargetSelector targeting;

    private bool canPlayerAct;

    private Combatant activeCombatant;
    private CombatantActionArgs combatantActionArgs;

    public List<Combatant> PlayerCombatants { get; private set; } = new();
    public List<Combatant> EnemyCombatants { get; private set; } = new();

    private void Start()
    {
        Combatant[] combatants = FindObjectsByType<Combatant>(FindObjectsSortMode.None);
        PlayerCombatants = combatants.Where(combatant => combatant.IsPlayerControlled).ToList();
        EnemyCombatants = combatants.Where(combatant => !combatant.IsPlayerControlled).ToList();

        arena.PositionCombatants(PlayerCombatants.ToArray(), EnemyCombatants.ToArray());
        ui.Initialize(this);
        StartCombat();
    }

    private void OnEnable()
    {
        ui.OnActionButton += OnActionSelected;
        targeting.OnTargetsSelected += OnTargetsSelected;
    }

    private void OnDisable()
    {
        ui.OnActionButton -= OnActionSelected;
        targeting.OnTargetsSelected -= OnTargetsSelected;
    }

    private void StartCombat()
    {
        CombatTurnOrder.InitializeTurnOrder(PlayerCombatants.Concat(EnemyCombatants).ToList());
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        activeCombatant = CombatTurnOrder.GetNextCombatant();

        if (!activeCombatant.IsAlive)
        {
            OnCombatEnded?.Invoke(!activeCombatant.IsPlayerControlled);
        }
        else
        {
            canPlayerAct = activeCombatant.IsPlayerControlled;
            OnTurnStarted?.Invoke(activeCombatant);

            Invoke(nameof(TryEnemyAct), 1f); // Not recommended to Invoke. This is only done so that we see the enemy "thinking".
        }
    }

    private void FinishCurrentTurn()
    {
        CombatTurnOrder.ResetCombatantActionValue(activeCombatant);
        OnTurnEnded?.Invoke(activeCombatant);

        Invoke(nameof(StartNextTurn), 1f);
    }

    // TEMPORARY
    public void OnActionSelected(CombatActionType actionType, int index)
    {
        if (!canPlayerAct)
            return;

        combatantActionArgs = new CombatantActionArgs(actionType, index);
        Skill selectedSkill = activeCombatant.GetSkillFromActionType(actionType, index);

        switch (selectedSkill.SkillTargetingMode)
        {
            case SkillTargetingMode.SingleEnemy:
            case SkillTargetingMode.AllEnemies:
                StartTargetSelection(selectedSkill.SkillTargetingMode, EnemyCombatants.Cast<ITarget>().ToArray());
                break;
            case SkillTargetingMode.SingleAlly:
            case SkillTargetingMode.AllAllies:
                StartTargetSelection(selectedSkill.SkillTargetingMode, PlayerCombatants.Cast<ITarget>().ToArray());
                break;
            case SkillTargetingMode.Self:
                StartTargetSelection(selectedSkill.SkillTargetingMode, new[] { activeCombatant }.Cast<ITarget>().ToArray());
                break;
            default:
                Debug.LogWarning($"Unhandled skill targeting mode: {selectedSkill.SkillTargetingMode}");
                break;
        }
    }

    private void StartTargetSelection(SkillTargetingMode targetingMode, ITarget[] targets)
    {
        targeting.SetTargetingSelectorEnabled(true, targetingMode, targets);
    }

    private void OnTargetsSelected(ITarget[] selectedTargets)
    {
        targeting.SetTargetingSelectorEnabled(false);
        combatantActionArgs.SetTargets(selectedTargets);
        TryExecuteAction(combatantActionArgs);
    }

    private void TryEnemyAct()
    {
        if (activeCombatant.IsPlayerControlled)
            return;

        Skill[] skills = activeCombatant.CharacterData.Skills;

        if (skills.Length > 0)
        {
            bool skillExecuted = false;
            int skillIndex = UnityEngine.Random.Range(0, skills.Length);
            Skill skill = skills[skillIndex];
            Combatant[] targets = skill.SkillTargetingMode switch
            {
                SkillTargetingMode.SingleEnemy => new[] { SelectRandomCombatant(PlayerCombatants) },
                SkillTargetingMode.AllEnemies => PlayerCombatants.ToArray(),
                SkillTargetingMode.SingleAlly => new[] { SelectRandomCombatant(EnemyCombatants) },
                SkillTargetingMode.AllAllies => EnemyCombatants.ToArray(),
                SkillTargetingMode.Self => new[] { activeCombatant },
                SkillTargetingMode.None => Array.Empty<Combatant>(),
                _ => null
            };

            if (TryExecuteAction(new(CombatActionType.Skill, skillIndex, targets)))
                skillExecuted = true;

            if (skillExecuted)
                return;
        }

        if (UnityEngine.Random.Range(0, 2) == 0)
            TryExecuteAction(new(CombatActionType.Skip, targets: new[] { activeCombatant }));
        else
            TryExecuteAction(new(CombatActionType.Attack, targets: new[] { SelectRandomCombatant(PlayerCombatants) }));
            
        static Combatant SelectRandomCombatant(IEnumerable<Combatant> combatants)
        {
            Combatant[] combatantArray = combatants.ToArray();
            int targetIndex = UnityEngine.Random.Range(0, combatantArray.Length);
            return combatantArray[targetIndex];
        }
    }

    private bool TryExecuteAction(CombatantActionArgs args)
    {
        if (activeCombatant.TryUseSkill(this, args))
        {
            FinishCurrentTurn();
            return true;
        }

        Debug.Log($"Skill failed!");
        return false;
    }
}

public enum CombatActionType
{
    Attack,
    Skill,
    Item,
    Skip,
}

public class CombatantActionArgs
{
    public CombatActionType ActionType { get; }
    public int ActionIndex { get; }
    public ITarget[] Targets { get; private set; }

    public CombatantActionArgs(CombatActionType actionType, int actionIndex = -1) : this(actionType, actionIndex, null)
    {
        ActionType = actionType;
        ActionIndex = actionIndex;
    }

    public CombatantActionArgs(CombatActionType actionType, int actionIndex = -1, Combatant[] targets = null)
    {
        ActionType = actionType;
        ActionIndex = actionIndex;
        Targets = targets;
    }

    public void SetTargets(ITarget[] targets)
    {
        Targets = targets;
    }
}