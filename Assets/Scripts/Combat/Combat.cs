using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
public class Combat : MonoBehaviour
{
    [SerializeField] private Combatant playerCombatant;
    [SerializeField] private Combatant enemyCombatant;
    [SerializeField] private Skill basicAttack;

    private bool canPlayerAct;
    private Combatant activeCombatant;

    public List<ITarget> Targets { get; private set; } = new();

    public Combatant PlayerCombatant => playerCombatant;
    public Combatant EnemyCombatant => enemyCombatant;

    private void Start()
    {
        // TODO: Temporarily handle setting colors here
        playerCombatant?.Visualizer?.SetToAttackerColor();

        if (playerCombatant != null && enemyCombatant != null && !playerCombatant.Equals(enemyCombatant))
        {
            enemyCombatant?.Visualizer?.SetToDefenderColor();
            Targets.Add(enemyCombatant);
        }

        CombatTurnManager.InitializeTurnOrder(new List<Combatant> { playerCombatant, enemyCombatant });
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
        activeCombatant = CombatTurnManager.GetNextCombatant();
        canPlayerAct = activeCombatant.IsPlayerControlled;

        if (!activeCombatant.IsPlayerControlled)
        {
            TargetSelectionArgs targetSelectionArgs = new()
            {
                Combat = this,
                Invoker = enemyCombatant,
                Targets = new[] { playerCombatant }
            };

            enemyCombatant.TryUseSkill(0, targetSelectionArgs);
            CombatTurnManager.ResetCombatantActionValue(activeCombatant);
            Invoke(nameof(StartNextTurn), 1f);
        }
    }

    // TEMPORARY
    public void Attack()
    {
        if (canPlayerAct)
        {
            canPlayerAct = false;

            TargetSelectionArgs targetSelectionArgsAttacker = new()
            {
                Combat = this,
                Invoker = playerCombatant,
                Targets = new[] { enemyCombatant }
            };

            playerCombatant.TryUseSkill(0, targetSelectionArgsAttacker);
            CombatTurnManager.ResetCombatantActionValue(activeCombatant);
            Invoke(nameof(StartNextTurn), 1f);
        }
    }
}

