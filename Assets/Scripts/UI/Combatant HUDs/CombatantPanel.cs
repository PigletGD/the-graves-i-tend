using UnityEngine;

public abstract class CombatantPanel : MonoBehaviour
{
    [SerializeField] protected CombatResourceBar hpBar;
    [SerializeField] protected CombatResourceBar mpBar;

    protected Combatant cachedCombatant;

    public void OnEnable()
    {
        Combatant.OnHPChanged += OnHPChanged;
        Combatant.OnMPChanged += OnMPChanged;
    }
    
    public void OnDisable()
    {
        Combatant.OnHPChanged -= OnHPChanged;
        Combatant.OnMPChanged -= OnMPChanged;
    }

    public virtual void Initialize(Combatant combatant)
    {
        cachedCombatant = combatant;

        hpBar.SetMinMaxValues(0, combatant.Stats.MaxHP);
        mpBar.SetMinMaxValues(0, combatant.Stats.MaxMP);

        hpBar.SetValue(combatant.Stats.CurrentHP);
        mpBar.SetValue(combatant.Stats.CurrentMP);
    }

    public void OnHPChanged(Combatant combatant, float _)
    {
        if (cachedCombatant != combatant)
            return;

        hpBar.SetValue(combatant.Stats.CurrentHP);
    }
    
    public void OnMPChanged(Combatant combatant, float _)
    {
        if (cachedCombatant != combatant)
            return;

        mpBar.SetValue(combatant.Stats.CurrentMP);
    }
}