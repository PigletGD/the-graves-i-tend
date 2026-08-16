using UnityEngine;

public class Tile : MonoBehaviour, ITarget
{
    [field: SerializeField] public TargetSelectionVisualizer Visualizer { get; private set; }
    
    [SerializeField] private Collider2D collider2D;
    private ITarget occupant;

    private void Awake()
    {
        var occupants = GetComponentsInChildren<ITarget>();
        if (occupants != null && occupants.Length > 0)
        {
            foreach (var toCheck in occupants)
            {
                if (toCheck.Equals(this))
                    continue;

                occupant = toCheck;
                break;
            }
        }
        
        if (occupant != null)
            SetOccupant(occupant);
    }

    public void SetOccupant(ITarget newOccupant)
    {
        if (collider2D != null)
            collider2D.enabled = false;
        
        occupant = newOccupant;
    }

    public void RemoveOccupant()
    {
        occupant = null;
        
        if (collider2D != null)
            collider2D.enabled = true;
    }

    public ITarget[] GetTargets(Battle battle)
    {
        return new[] { occupant };
    }

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }
}
