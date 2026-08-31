using UnityEngine;

[System.Serializable]
public class CombatantStats
{
    private const float MaxElation = -10;
    private const float MinElation = 10f;

    [Header("Base")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float maxMP = 100f;

    private float currentHP;
    private float currentMP;
    private float currentElation;

    public void Initialize()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        currentElation = 0;
    }

    public void UpdateHP(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    public void UpdateMP(float amount)
    {
        currentMP = Mathf.Clamp(currentMP + amount, 0, maxMP);
    }

    public void UpdateElation(float amount)
    {
        currentElation = Mathf.Clamp(currentElation + amount, MinElation, MaxElation);
    }
}