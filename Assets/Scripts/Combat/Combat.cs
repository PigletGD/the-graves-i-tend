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
        ui.OnTargetingCancelRequested += StopTargetSelection;
        targeting.OnTargetsSelected += OnTargetsSelected;
        targeting.OnHoveredTargetChanged += OnHoveredTargetChanged;
    }

    private void OnDisable()
    {
        ui.OnActionButton -= OnActionSelected;
        ui.OnTargetingCancelRequested -= StopTargetSelection;
        targeting.OnTargetsSelected -= OnTargetsSelected;
        targeting.OnHoveredTargetChanged -= OnHoveredTargetChanged;
    }

    private void StartCombat()
    {
        CombatTurnOrder.InitializeTurnOrder(PlayerCombatants.Concat(EnemyCombatants).ToList());
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        activeCombatant = CombatTurnOrder.GetNextCombatant();

        canPlayerAct = activeCombatant.IsPlayerControlled;
        arena.SetCurrentCombatant(activeCombatant);
        OnTurnStarted?.Invoke(activeCombatant);

        Invoke(nameof(TryEnemyAct), 1f); // Not recommended to Invoke. This is only done so that we see the enemy "thinking".
    }

    public void OnActionSelected(CombatActionType actionType, int index)
    {
        if (!canPlayerAct)
            return;

        combatantActionArgs = new CombatantActionArgs(actionType, index);
        Skill selectedSkill = activeCombatant.GetSkillFromActionType(actionType, index);

        switch (selectedSkill.VisualTargetingMode)
        {
            case TargetSelectionMode.SingleEnemy:
            case TargetSelectionMode.AllEnemies:
                StartTargetSelection(selectedSkill.VisualTargetingMode, EnemyCombatants.Where(x => !x.IsDead).Cast<ITarget>().ToArray());
                break;
            case TargetSelectionMode.SingleAlly:
            case TargetSelectionMode.AllAllies:
                StartTargetSelection(selectedSkill.VisualTargetingMode, PlayerCombatants.Where(x => !x.IsDead).Cast<ITarget>().ToArray());
                break;
            case TargetSelectionMode.None:
            case TargetSelectionMode.Self:
                StartTargetSelection(selectedSkill.VisualTargetingMode, new[] { activeCombatant }.Cast<ITarget>().ToArray());
                break;
            default:
                Debug.LogWarning($"Unhandled visual targeting mode: {selectedSkill.VisualTargetingMode}");
                break;
        }
    }

    private void StartTargetSelection(TargetSelectionMode visualTargetingMode, ITarget[] targets)
    {
        targeting.SetTargetingSelectorEnabled(true, visualTargetingMode, targets);
    }

    private void StopTargetSelection()
    {
        targeting.SetTargetingSelectorEnabled(false);
    }

    private void OnHoveredTargetChanged(ITarget target)
    {
        arena.SetHoveredCombatant(target as Combatant);
    }

    private void OnTargetsSelected(ITarget[] selectedTargets)
    {
        StopTargetSelection();
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
            ITarget[] targets = skill.VisualTargetingMode switch
            {
                TargetSelectionMode.None => Array.Empty<ITarget>(),
                TargetSelectionMode.SingleEnemy => new ITarget[] { SelectRandomCombatant(PlayerCombatants) },
                TargetSelectionMode.AllEnemies => PlayerCombatants.Cast<ITarget>().ToArray(),
                TargetSelectionMode.SingleAlly => new ITarget[] { SelectRandomCombatant(EnemyCombatants) },
                TargetSelectionMode.AllAllies => EnemyCombatants.Cast<ITarget>().ToArray(),
                TargetSelectionMode.Self => new ITarget[] { activeCombatant },
                _ => throw new ArgumentOutOfRangeException(nameof(skill.VisualTargetingMode), skill.VisualTargetingMode, "Unsupported attempt targeting mode."),
            };

            if (TryExecuteAction(new(CombatActionType.Skill, skillIndex, targets)))
                skillExecuted = true;

            if (skillExecuted)
                return;
        }

        if (UnityEngine.Random.Range(0, 2) == 0)
            TryExecuteAction(new(CombatActionType.Skip, targets: new ITarget[] { activeCombatant }));
        else
            TryExecuteAction(new(CombatActionType.Attack, targets: new ITarget[] { SelectRandomCombatant(PlayerCombatants) }));
            
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

    private void FinishCurrentTurn()
    {
        CombatTurnOrder.ResetCombatantActionValue(activeCombatant);
        arena.SetCurrentCombatant(null);
        OnTurnEnded?.Invoke(activeCombatant);

        foreach (Combatant combatant in PlayerCombatants.Concat(EnemyCombatants))
        {
            if (combatant.IsDead)
                CombatTurnOrder.RemoveCombatant(combatant);
        }

        if (CheckCombatEnded())
            return;

        Invoke(nameof(StartNextTurn), 1f);
    }

    private bool CheckCombatEnded()
    {
        bool allPlayersDead = PlayerCombatants.All(combatant => combatant.IsDead);
        bool allEnemiesDead = EnemyCombatants.All(combatant => combatant.IsDead);

        if (!allPlayersDead && !allEnemiesDead)
            return false;

        OnCombatEnded?.Invoke(allEnemiesDead && !allPlayersDead);
        return true;
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

    public CombatantActionArgs(CombatActionType actionType, int actionIndex = -1, ITarget[] targets = null)
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