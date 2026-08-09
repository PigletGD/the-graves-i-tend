using System;
using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
// TL;DR: Each Skill has N attempts where each attempt has N effects.
// Refactor as necessary. Only doing this so that there's something actionable.

// TO DO: 
// Combatant.cs
// Class Effect/DamageEffect is in a weird spot right now. Needed to use a getter in DamageEffect.cs to execute basic attack.
public class Battle : MonoBehaviour
{
    [SerializeField] private Combatant attacker;
    [SerializeField] private Combatant defender;
    [SerializeField] private Skill basicAttack;

    [SerializeField] private bool enableRepeatingExecuteAttempts = true;
    
    public List<Combatant> Targets { get; private set; } = new();

    public Combatant Attacker => attacker;

    private void Start()
    {
        // TODO: Temporarily handle setting colors here
        attacker?.Visualizer?.SetToAttackerColor();
        
        if (attacker != null && defender != null && !attacker.Equals(defender))
        {
            defender?.Visualizer?.SetToDefenderColor();
            Targets.Add(defender);
        }
        
        if (enableRepeatingExecuteAttempts)
            InvokeRepeating(nameof(ExecuteAttempts), 0, 1f);
    }

    public void ExecuteAttempts()
    {
        basicAttack.Execute(this);
    }

    public void HandleAttackerSetupForSelected(Combatant selected)
    {
        if (selected == null)
            return;
        
        if (!selected.Equals(attacker))
        {
            selected.Visualizer?.SetToAttackerColor();
            attacker?.Visualizer?.SetToUnselectedColor();
            
            attacker = selected;
            
            if (Targets.Contains(selected))
                Targets.Remove(selected);
            
            Debug.Log($"Attacker is set to {selected.name}", selected.gameObject);
        }
        else
        {
            selected.Visualizer?.SetToHoveredColor(true);
            
            attacker = null;
            
            Debug.Log($"Removed {selected.name} as the attacker", selected.gameObject);
        }
    }

    public void HandleDefenderSetupForSelected(Combatant selected)
    {
        if (selected == null)
            return;
        
        if (!Targets.Contains(selected))
        {
            selected.Visualizer?.SetToDefenderColor();
            
            Targets.Add(selected);
            
            if (selected.Equals(attacker))
                attacker = null;
            
            Debug.Log($"Added {selected.name} as a target", selected.gameObject);
        }
        else
        {
            selected.Visualizer?.SetToHoveredColor(true);
            
            Targets.Remove(selected);
            
            Debug.Log($"Removed {selected.name} as a target", selected.gameObject);
        }
    }
}
