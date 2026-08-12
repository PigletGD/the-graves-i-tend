using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect/Frozen")]
public class FrozenStatusEffectSO : StatusEffectSO
{
    [SerializeField] private int maxStacks = 3;
    [SerializeField] private int stacksPerApplication = 1;

    public override StatusEffectType StatusEffectType => StatusEffectType.Frozen;
    public int MaxStacks => maxStacks;
    public int StacksPerApplication => stacksPerApplication;

    public override StatusEffect CreateInstance()
    {
        return new FrozenStatusEffect(this);
    }
}