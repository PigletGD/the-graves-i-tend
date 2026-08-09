public interface ITarget
{
    // Allow classes that use ITarget to target other targets (ex: Combatant [and Tiles if ever]).
    public ITarget[] GetTargets(Battle battle);
}
