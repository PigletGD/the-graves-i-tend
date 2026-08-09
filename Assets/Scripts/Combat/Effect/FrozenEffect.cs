using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Frozen Effect")]
public class FrozenEffect : Effect
{
    [SerializeField] private ProbabilityCondition<float> applyChance;
    [SerializeField] private CombatantEffectCondition[] effectConditions;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (!applyChance.Check(0))
        {
            Log("missed");
            return;
        }

        foreach (CombatantEffectCondition effectCondition in effectConditions)
        {
            if (!effectCondition.Check(combatant))
            {
                Log($"failed because of {effectCondition.Effect}");
                return;
            }
        }

        combatant.AddEffect(this);
    }
}
