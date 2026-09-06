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
    // TODO: Will be using the first target element as the defender to return
    public Combatant Defender => Targets.Count > 0 ? Targets[0] as Combatant : null;

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
            attacker?.GetSelectionVisualizer()?.SetToUnselectedColor();
            
            attacker = selected.GetRootObject()?.GetComponent<Combatant>();
            if (attacker != null)
            {
                attacker.GetSelectionVisualizer()?.SetToAttackerColor();
            
                if (Targets.Contains(selected))
                {
                    Targets.Remove(selected);

                    var newPrimaryDefender = Defender;
                    if (newPrimaryDefender != null)
                        CombatUIManager.Instance.UpdateDefenderResourceBars(newPrimaryDefender, true);
                }
            
                Debug.Log($"Attacker is set to {selected.GetRootObject().name}", attacker.GetRootObject());
            }
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
            
            if (Targets.Count == 1)
                CombatUIManager.Instance.UpdateDefenderResourceBars(selected as Combatant, true);

            if (selected.Equals(attacker))
                attacker = null;

            Debug.Log($"Added {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
        else
        {
            selected.GetSelectionVisualizer()?.SetToHoveredColor(true);

            Targets.Remove(selected);
            
            var newPrimaryDefender = Defender;
            if (newPrimaryDefender != null)
                CombatUIManager.Instance.UpdateDefenderResourceBars(newPrimaryDefender, true);

            Debug.Log($"Removed {selected.GetRootObject()?.name} as a target", selected.GetRootObject());
        }
    }

    // TEMPORARY
    public void Attack()
    {
        var primaryDefender = Defender;
        
        TargetSelectionArgs targetSelectionArgsAttacker = new()
        {
            Combat = this,
            Invoker = attacker,
            Targets = new []{ primaryDefender }
        };

        attacker.TryUseSkill(0, targetSelectionArgsAttacker);

        TargetSelectionArgs targetSelectionArgsDefender = new()
        {
            Combat = this,
            Invoker = primaryDefender,
            Targets = new[] { primaryDefender }
        };

        defender.TryUseSkill(0, targetSelectionArgsDefender);

        // TEMPORARY
        CombatUIManager.Instance.UpdateCharacterResourceBars(attacker);
        CombatUIManager.Instance.UpdateDefenderResourceBars(primaryDefender, true);
    }
}
