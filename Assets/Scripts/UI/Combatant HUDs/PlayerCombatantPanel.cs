using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatantPanel : MonoBehaviour, ICombatantPanel

{
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private CombatResourceBar hpBar;
    [SerializeField] private CombatResourceBar mpBar;

    public void Initialize(Combatant combatant)
    {
        characterNameText.SetText(combatant.CharacterData.CharacterName);
        characterPortraitImage.sprite = combatant.CharacterData.Portrait;

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
