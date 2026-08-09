using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Frozen Effect")]
public class FrozenEffect : Effect
{
    [SerializeField] private ProbabilityCondition<float> applyChance;

    public override void Apply(Battle battle)
    {
        Debug.Log($"[Frozen Effect] Applied: {applyChance.Check(0)}");
    }
}
