public interface IAttempt
{
    public void Execute(Battle battle, ITarget source, ITarget[] targets);
}