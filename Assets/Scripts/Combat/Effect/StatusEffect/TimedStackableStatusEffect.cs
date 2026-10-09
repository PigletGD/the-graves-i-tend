public abstract class TimedStackableStatusEffect : StackableStatusEffect // TODO: Maybe have interface ITurnDuration.
{
    private int initialTurnDuration;
    private int remainingTurnDuration;
    private bool canRefreshOnApply;

    public bool HasExpired => remainingTurnDuration <= 0;

    protected void InitializeDuration(int turnDuration, bool refreshOnApply)
    {
        initialTurnDuration = turnDuration;
        remainingTurnDuration = turnDuration;
        canRefreshOnApply = refreshOnApply;
    }

    public override void OnReapplied(StatusEffect reappliedEffect, int stacks)
    {
        base.OnReapplied(reappliedEffect, stacks);

        if (canRefreshOnApply && reappliedEffect is TimedStackableStatusEffect timedEffect)
            remainingTurnDuration = timedEffect.initialTurnDuration;
    }

    protected void AdvanceTurnDuration()
    {
        remainingTurnDuration--;
    }
}
