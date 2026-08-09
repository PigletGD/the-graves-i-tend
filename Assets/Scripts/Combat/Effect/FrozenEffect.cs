using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Frozen Effect")]
public class FrozenEffect : Effect
{
    [SerializeField] private ProbabilityCondition<float> applyChance;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Debug.Log("Target is not a combatant!");
            return;
        }

        Debug.Log($"[Frozen Effect] Applied: {applyChance.Check(0)}");
    }
}
