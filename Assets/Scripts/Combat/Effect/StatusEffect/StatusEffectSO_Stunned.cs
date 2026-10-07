using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Stunned", menuName = "Status Effect/Stunned")]
public class StatusEffectSO_Stunned : StatusEffectSO
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Stunned;

    public override StatusEffect CreateInstance()
    {
        return new StatusEffect_Stunned(this);
    }
}
