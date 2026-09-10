public interface IStackable
{
    public int StackCount { get; }
    public int MaxStacks { get; }

    public void AddStacks(int amount);
    public void RemoveStacks(int amount);
}