using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
public class Combat : MonoBehaviour
{
    [SerializeField] private Combatant attacker;
    [SerializeField] private Combatant defender;
    [SerializeField] private Skill basicAttack;

    public List<ITarget> Targets { get; private set; } = new();

    public Combatant Attacker => attacker;
    public Combatant Defender => defender;

    private void Start()
    {
        // TODO: Temporarily handle setting colors here
        attacker?.Visualizer?.SetToAttackerColor();

        if (attacker != null && defender != null && !attacker.Equals(defender))
        {
            defender?.Visualizer?.SetToDefenderColor();
            Targets.Add(defender);
        }
    }

    public void HandleAttackerSetupForSelected(ITarget selected)
    {
        if (selected == null)
            return;
        
        if (!selected.Equals(attacker))
        {
            selected.GetSelectionVisualizer()?.SetToAttackerColor();
            attacker?.GetSelectionVisualizer()?.SetToUnselectedColor();
            
            attacker = selected.GetRootObject()?.GetComponent<Combatant>();
            
            if (Targets.Contains(selected))
                Targets.Remove(selected);
            
            Debug.Log($"Attacker is set to {selected.GetRootObject().name}", selected.GetRootObject());
        }
        else
        {
            selected.GetSelectionVisualizer()?.SetToHoveredColor(true);
            
            attacker = null;
            
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

            if (selected.Equals(attacker))
                attacker = null;

            Debug.Log($"Added {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
        else
        {
            selected.GetSelectionVisualizer()?.SetToHoveredColor(true);

            Targets.Remove(selected);

            Debug.Log($"Removed {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
    }

    // TEMPORARY
    public void Attack()
    {
        TargetSelectionArgs targetSelectionArgsAttacker = new()
        {
            Combat = this,
            Invoker = attacker,
            Targets = new []{ defender }
        };

        attacker.TryUseSkill(0, targetSelectionArgsAttacker);

        TargetSelectionArgs targetSelectionArgsDefender = new()
        {
            Combat = this,
            Invoker = defender,
            Targets = new[] { attacker }
        };

        defender.TryUseSkill(0, targetSelectionArgsDefender);

        // TEMPORARY
        CombatUIManager.Instance.UpdateCharacterResourceBars(attacker);
        CombatUIManager.Instance.UpdateCharacterResourceBars(defender);
    }
}
