using UnityEngine;

[System.Serializable]
public class CombatantStats
{
    private const float MaxElation = -10;
    private const float MinElation = 10f;

    private float maxHP;
    private float maxMP;
    private float maxSpeed;

    private float currentHP;
    private float currentMP;
    private float currentSpeed;
    private float currentElation;

    public float MaxHP => maxHP;
    public float MaxMP => maxMP;

    public float CurrentHP => currentHP;
    public float CurrentMP => currentMP;
    public float CurrentSpeed => currentSpeed;

    /// <summary>
    /// Gets the action value of the combatant based on their current speed.
    /// </summary>
    public float ActionValue => 10000f / currentSpeed;

    public void Initialize(CharacterData characterData)
    {
        maxHP = characterData.MaxHP;
        maxMP = characterData.MaxMP;
        maxSpeed = characterData.Speed;

        currentHP = maxHP;
        currentMP = maxMP;
        currentSpeed = maxSpeed;

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

    public void UpdateSpeed(float amount)
    {
        currentSpeed = Mathf.Clamp(currentSpeed + amount, 1, maxSpeed);
    }

    public void UpdateElation(float amount)
    {
        currentElation = Mathf.Clamp(currentElation + amount, MinElation, MaxElation);
    }
}