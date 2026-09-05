using UnityEngine;

public interface ITarget
{
    public ITarget[] GetTargets(Combat combat);
    
    public TargetRelationship GetTargetRelationship();

    public TargetSelectionVisualizer GetSelectionVisualizer();

    // TODO: Just getting an easy place to get the root object for now for better debugging. We probably won't need this later on.
    public GameObject GetRootObject();

}
