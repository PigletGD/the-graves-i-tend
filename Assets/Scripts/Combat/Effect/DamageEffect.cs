using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Damage Effect")]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Debug.Log("Target is not a combatant!");
            return;
        }

        combatant.UpdateHP(-damage);

        if (battle.Targets == null || battle.Targets.Count == 0)
            return;

        foreach (var target in battle.Targets)
            target.GetRootObject()?.GetComponent<Combatant>()?.UpdateHP(-damage);
    }
}