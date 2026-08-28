public interface IAttempt
{
    public void Execute(Battle battle, ITarget invoker, ITarget target);
}