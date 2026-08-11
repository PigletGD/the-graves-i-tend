public interface IStackable
{
    public void AddStacks(int count);
    public void RemoveStacks(int count);
    public int GetCurrentStacks();
    public int GetMaxStacks();
}