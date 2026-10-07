using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Helltouched", menuName = "Status Effect/Helltouched")]
public class StatusEffectSO_Helltouched : StatusEffectSO
{
    [SerializeField] private int maxStacks = int.MaxValue;
    [SerializeField] private float damagePerStack = 1f;

    [SerializeField] private int turnDuration = 3;
    [SerializeField] private bool canRefreshOnApply = true;

    public override StatusEffectType StatusEffectType => StatusEffectType.Helltouched;
    public int MaxStacks => maxStacks;
    public float DamagePerStack => damagePerStack;
    public int TurnDuration => turnDuration;
    public bool CanRefreshOnApply => canRefreshOnApply;

    public override StatusEffect CreateInstance()
    {
        return new StatusEffect_Helltouched(this);
    }
}