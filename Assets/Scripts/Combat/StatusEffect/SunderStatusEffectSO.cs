using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Sunder", menuName = "Status Effect/Sunder")]
public class SunderStatusEffectSO : StatusEffectSO
{
    [SerializeField] private int maxStacks = 50;
    [SerializeField] private int stacksPerApplication = 1;

    public override StatusEffectType StatusEffectType => StatusEffectType.Frozen;
    public int MaxStacks => maxStacks;
    public int StacksPerApplication => stacksPerApplication;

    public override StatusEffect CreateInstance()
    {
        return new SunderStatusEffect(this);
    }
}