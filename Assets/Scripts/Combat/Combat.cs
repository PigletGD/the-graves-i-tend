using System;
using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
// TODO: SkillSlot.cs
public class Combat : MonoBehaviour
{
    public static event Action<Combatant> OnTurnStarted;
    public static event Action<Combatant> OnTurnEnded;
    public static event Action<bool> OnCombatEnded;

    [SerializeField] private Combatant playerCombatant;
    [SerializeField] private Combatant enemyCombatant;

    private bool canPlayerAct;

    private Combatant activeCombatant;

    public List<ITarget> Targets { get; private set; } = new();

    public Combatant PlayerCombatant => playerCombatant;
    public Combatant EnemyCombatant => enemyCombatant;
    public Combatant ActiveCombatant => activeCombatant;

    private void Start()
    {
        // TODO: Temporarily handle setting colors here
        playerCombatant?.Visualizer?.SetToAttackerColor();

        if (playerCombatant != null && enemyCombatant != null && !playerCombatant.Equals(enemyCombatant))
        {
            enemyCombatant?.Visualizer?.SetToDefenderColor();
            Targets.Add(enemyCombatant);
        }

        CombatTurnOrder.InitializeTurnOrder(new List<Combatant> { playerCombatant, enemyCombatant });
        StartNextTurn();
    }

    public void HandleAttackerSetupForSelected(ITarget selected)
    {
        if (selected == null)
            return;

        if (!selected.Equals(playerCombatant))
        {
            selected.GetSelectionVisualizer()?.SetToAttackerColor();
            playerCombatant?.GetSelectionVisualizer()?.SetToUnselectedColor();

            playerCombatant = selected.GetRootObject()?.GetComponent<Combatant>();

            if (Targets.Contains(selected))
                Targets.Remove(selected);

            Debug.Log($"Attacker is set to {selected.GetRootObject().name}", selected.GetRootObject());
        }
        else
        {
            selected.GetSelectionVisualizer()?.SetToHoveredColor(true);

            playerCombatant = null;

            Debug.Log($"Removed {selected.GetRootObject().name} as the attacker", selected.GetRootObject());
        }
    }

    public void HandleDefenderSetupForSelected(ITarget selected)
    {
        if (selected == null)
            return;

        if (!Targets.Contains(selected))
        {
            selected.GetSelectionVisualizer()?.SetToDefenderColor();

            Targets.Add(selected);

            if (selected.Equals(playerCombatant))
                playerCombatant = null;

            Debug.Log($"Added {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
        else
        {
            selected.GetSelectionVisualizer()?.SetToHoveredColor(true);

            Targets.Remove(selected);

            Debug.Log($"Removed {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
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
    public void TryPlayerAct(int index)
    {
        if (!canPlayerAct)
            return;


        if (!TryExecuteAction(playerCombatant, index, index == Combatant.SkipTurnIndex ? playerCombatant : enemyCombatant))
            canPlayerAct = true;
        else
            canPlayerAct = false;
    }

    private void TryEnemyAct()
    {
        if (activeCombatant.IsPlayerControlled)
            return;

        // If the enemy failed to attack then they'll simply skip for now.
        if (!TryExecuteAction(enemyCombatant, Combatant.BasicAttackIndex, playerCombatant))
            TryExecuteAction(enemyCombatant, Combatant.SkipTurnIndex, playerCombatant);
    }

    private bool TryExecuteAction(Combatant combatant, int index, Combatant target)
    {
        TargetSelectionArgs targetSelectionArgs = new()
        {
            Combat = this,
            Invoker = combatant,
            Targets = new[] { target }
        };

        if (combatant.TryUseSkill(index, targetSelectionArgs))
        {
            FinishCurrentTurn();
            return true;
        }

        Debug.Log($"Skill failed!");
        return false;
    }
}
