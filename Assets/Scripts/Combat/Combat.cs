using System;
using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
public class Combat : MonoBehaviour
{
    public static event Action<Combatant> OnTurnUpdated;

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
        canPlayerAct = activeCombatant.IsPlayerControlled;
        OnTurnUpdated?.Invoke(activeCombatant);

        Invoke(nameof(TryEnemyAct), 2f); // Not recommended to Invoke. This is only done so that we see the enemy "thinking".
    }

    private void FinishCurrentTurn()
    {
        CombatTurnOrder.ResetCombatantActionValue(activeCombatant);
        StartNextTurn();
    }

    private void TryEnemyAct()
    {
        if (!activeCombatant.IsPlayerControlled)
        {
            TargetSelectionArgs targetSelectionArgs = new()
            {
                Combat = this,
                Invoker = enemyCombatant,
                Targets = new[] { playerCombatant }
            };

            int index = -1;

            if (index == -1)
                enemyCombatant.DoBasicAttack(targetSelectionArgs);
            else
                enemyCombatant.TryUseSkill(0, targetSelectionArgs);
            FinishCurrentTurn();
        }
    }

    // TEMPORARY
    public void TryPlayerSkill(int index)
    {
        if (canPlayerAct)
        {
            canPlayerAct = false;

            TargetSelectionArgs targetSelectionArgs = new()
            {
                Combat = this,
                Invoker = playerCombatant,
                Targets = new[] { enemyCombatant }
            };

            if (index == -1)
                playerCombatant.DoBasicAttack(targetSelectionArgs);
            else
                playerCombatant.TryUseSkill(index, targetSelectionArgs);
            FinishCurrentTurn();
        }
    }

    public void SkipTurn()
    {
        if (canPlayerAct)
        {
            canPlayerAct = false;
            FinishCurrentTurn();
        }
    }
}

