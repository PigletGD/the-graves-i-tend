// TODO: This was a temp fix for all effects applying to the defined target at the skill level. Remove once the adjustments get in.
public interface ICanOverrideTarget
{
    public bool ShouldOverrideTarget();
    public TargetSelectionMode GetOverridableTargetSelection();
}