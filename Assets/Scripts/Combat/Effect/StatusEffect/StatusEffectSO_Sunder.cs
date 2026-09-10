using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Sunder", menuName = "Status Effect/Sunder")]
public class StatusEffectSO_Sunder : StatusEffectSO
{
    [SerializeField] private int maxStacks = 50;

    public override StatusEffectType StatusEffectType => StatusEffectType.Sunder;
    public int MaxStacks => maxStacks;

    public override StatusEffect CreateInstance()
    {
        return new StatusEffect_Sunder(this);
    }
}
