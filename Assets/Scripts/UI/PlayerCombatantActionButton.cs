using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatantActionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Button button;

    public event Action<int> OnClick;
    public int Index { get; private set; } = -1;

    private void Awake()
    {
        button.onClick.AddListener(() => OnClick?.Invoke(Index));
    }

    public void UpdateCombatantActionButton(int index, string action)
    {
        Index = index;
        actionText.SetText(action);
    }
}
