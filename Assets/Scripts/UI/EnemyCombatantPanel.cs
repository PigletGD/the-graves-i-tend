using UnityEngine;

public class EnemyCombatantPanel : MonoBehaviour, ICombatantPanel
{
    [SerializeField] private CombatResourceBar hpBar;
    [SerializeField] private CombatResourceBar mpBar;

    public void Initialize(Combatant combatant)
    {
        hpBar.SetMinMaxValues(0, combatant.Stats.MaxHP);
        mpBar.SetMinMaxValues(0, combatant.Stats.MaxMP);

        hpBar.SetValue(combatant.Stats.CurrentHP);
        mpBar.SetValue(combatant.Stats.CurrentMP);
    }

    public void UpdateResourceBars(Combatant combatant)
    {
        hpBar.SetValue(combatant.Stats.CurrentHP);
        mpBar.SetValue(combatant.Stats.CurrentMP);
    }
}