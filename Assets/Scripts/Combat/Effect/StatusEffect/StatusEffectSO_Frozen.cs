using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Frozen", menuName = "Status Effect/Frozen")]
public class StatusEffectSO_Frozen : StatusEffectSO
{
    [SerializeField] private int maxStacks = 6;

    public override StatusEffectType StatusEffectType => StatusEffectType.Frozen;
    public int MaxStacks => maxStacks;

    public override StatusEffect CreateInstance()
    {
        return new StatusEffect_Frozen(this);
    }
}