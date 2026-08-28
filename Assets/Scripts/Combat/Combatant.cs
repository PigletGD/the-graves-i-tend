using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
[RequireComponent(typeof(StatusEffectController))]
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private float maxHP;
    [SerializeField] private StatusEffectController effectController;
    [SerializeField] private Combatant[] targets;
    [SerializeField] private TargetRelationship targetRelationship; // Temporary

    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

    private float currentHP;

    public StatusEffectController EffectController => effectController;

    private void Awake()
    {
        Visualizer?.SetToUnselectedColor();
    }

    private void Start()
    {
        currentHP = maxHP;
    }

    public void UpdateHP(float hpValue)
    {
        currentHP = Mathf.Clamp(currentHP += hpValue, 0, maxHP);
        Debug.Log($"{name} is at {currentHP}HP!");
    }

    // TODO: Refactor this so that we get targets from the selection.
    public ITarget[] GetTargets(Battle _)
    {
        return targets;
    }

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }

    public TargetRelationship GetTargetRelationship()
    {
        return targetRelationship;
    }
}