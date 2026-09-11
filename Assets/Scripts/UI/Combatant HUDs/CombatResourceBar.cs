using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI resourceText;

    public void SetMinMaxValues(float minValue, float maxValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }
    
    public void SetValue(float value)
    {
        slider.value = value;
        resourceText.SetText($"{slider.value}/{slider.maxValue}");
    }
}
