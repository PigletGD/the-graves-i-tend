using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private float maxHP;

    private float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void UpdateHP(float hpValue)
    {
        currentHP = Mathf.Clamp(currentHP += hpValue, 0, maxHP);
        Debug.Log($"{name} is at {currentHP}HP!");
    }

    public Combatant Resolve(Battle _)
    {
        return this;
    }
}
