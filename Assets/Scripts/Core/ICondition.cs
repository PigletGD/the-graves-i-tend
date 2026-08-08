using System;
using System.Linq;

public interface ICondition<T>
{
    public bool Check(T value);
}

public sealed class AndCondition<T> : ICondition<T>
{
    private readonly ICondition<T>[] conditions;

    public AndCondition(params ICondition<T>[] conditions) => this.conditions = conditions;

    public bool Check(T value) => conditions.All(c => c.Check(value));
}

public sealed class OrCondition<T> : ICondition<T>
{
    private readonly ICondition<T>[] conditions;

    public OrCondition(params ICondition<T>[] conditions) => this.conditions = conditions;

    public bool Check(T value) => conditions.Any(c => c.Check(value));
}

public sealed class NotCondition<T> : ICondition<T>
{
    private readonly ICondition<T> condition;

    public NotCondition(ICondition<T> condition) => this.condition = condition;

    public bool Check(T value) => !condition.Check(value);
}

public sealed class ProbabilityCondition<T> : ICondition<T>
{
    private readonly float probability;

    public ProbabilityCondition(float probability)
    {
        if (probability < 0f || probability > 1f)
            throw new ArgumentOutOfRangeException(nameof(probability), "Must be between 0 and 1.");

        this.probability = probability;
    }

    public bool Check(T value) => UnityEngine.Random.value < probability;
}

public sealed class LessThanOrEqualCondition<T> : ICondition<T> where T : IComparable<T>
{
    private readonly T threshold;

    public LessThanOrEqualCondition(T threshold) => this.threshold = threshold;

    public bool Check(T value) => value.CompareTo(threshold) <= 0;
}

public sealed class GreaterThanOrEqualCondition<T> : ICondition<T> where T : IComparable<T>
{
    private readonly T threshold;

    public GreaterThanOrEqualCondition(T threshold) => this.threshold = threshold;

    public bool Check(T value) => value.CompareTo(threshold) >= 0;
}
