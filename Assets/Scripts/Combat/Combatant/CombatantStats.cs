using System;
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
    public float CurrentElation => currentElation;

    /// <summary>
    /// Gets the action value of the combatant based on their current speed.
    /// </summary>
    public float ActionValue => 10000f / currentSpeed;

    public void Initialize(CharacterData characterData, float startingMP)
    {
        maxHP = characterData.MaxHP;
        maxMP = characterData.MaxMP;
        maxSpeed = characterData.Speed;

        currentHP = maxHP;
        currentMP = startingMP; // Reminder to move this literal out.
        currentSpeed = maxSpeed;

        currentElation = 0;
    }

    public float GetResourceAmount(CombatantResourceType resourceType)
    {
        return resourceType switch
        {
            CombatantResourceType.HP => CurrentHP,
            CombatantResourceType.MP => CurrentMP,
            CombatantResourceType.Elation => CurrentElation,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
        };
    }

    public void UpdateResource(CombatantResourceType resourceType, float amount)
    {
        switch (resourceType)
        {
            case CombatantResourceType.HP:
                currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
                break;
            case CombatantResourceType.MP:
                currentMP = Mathf.Clamp(currentMP + amount, 0, maxMP);
                break;
            case CombatantResourceType.Elation:
                currentElation = Mathf.Clamp(currentElation + amount, MinElation, MaxElation);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }
    }
}
