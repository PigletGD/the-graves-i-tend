using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatantPanel : CombatantPanel
{
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private TMP_Text characterNameText;
    
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        characterNameText.SetText(combatant.CharacterData.CharacterName);
        characterPortraitImage.sprite = combatant.CharacterData.Portrait;
    }
}
