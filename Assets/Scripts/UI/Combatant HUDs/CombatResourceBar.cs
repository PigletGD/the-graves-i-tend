using UnityEngine;
using UnityEngine.UI;

public class CombatResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMinMaxValues(float minValue, float maxValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }
    
    public void SetValue(float value)
    {
        slider.value = value;
    }
}
