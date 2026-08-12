using System.Collections.Generic;
using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
public class Battle : MonoBehaviour
{
    [SerializeField] private Combatant attacker;
    [SerializeField] private Combatant defender;
    [SerializeField] private Skill basicAttack;

    [SerializeField] private bool enableRepeatingExecuteAttempts = true;
    
    public List<ITarget> Targets { get; private set; } = new();

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
        basicAttack.Execute(this, attacker, Targets.ToArray());
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
}
