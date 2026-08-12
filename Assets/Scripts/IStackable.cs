public interface IStackable
{
    public int StackCount { get; }
    public int MaxStacks { get; }
    public int StacksOnAdd { get; }

    public void AddStacks(int amount);
    public void RemoveStacks(int amount);
}