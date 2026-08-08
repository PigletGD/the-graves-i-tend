using System;
using UnityEngine;

[Serializable]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;

    public override void Apply(Battle battle)
    {
        target.Resolve(battle).UpdateHP(-damage);
    }
}