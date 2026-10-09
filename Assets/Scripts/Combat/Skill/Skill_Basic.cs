using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillResourceCost
{
    public CombatantResourceType resourceType;
    public float amount;
}

/// <summary>
/// Basic Skill that has costs for the invoker and attempts to targets (that aren't the same relationship).
/// </summary>
[CreateAssetMenu(fileName = "Basic Skill", menuName = "Skills/Basic Skill")]
public class Skill_Basic : Skill
{
    [SerializeReference, SerializeReferenceDropdown] private ISkillCondition[] conditions;
    [SerializeField] private SkillResourceCost[] resourceCosts;

    public override bool CanExecute(TargetSelectionArgs args)
    {
        if (args.Invoker is not Combatant combatant)
            return false;

        return CanPayResourceCosts(combatant, GetTotalResourceCosts());
    }

    public override bool Execute(TargetSelectionArgs args)
    {
        if (args.Invoker is not Combatant combatant)
            return false;

        foreach (ISkillCondition condition in conditions)
        {
            if (!condition.Check(args))
                return false;
        }

        Dictionary<CombatantResourceType, float> totalResources = GetTotalResourceCosts();
        if (!CanPayResourceCosts(combatant, totalResources))
            return false;

        foreach (KeyValuePair<CombatantResourceType, float> resource in totalResources)
            combatant.UpdateResource(resource.Key, -resource.Value);

        foreach (Attempt attempt in attempts)
            attempt.Execute(args);

        return true;
    }

    private bool CanPayResourceCosts(Combatant combatant, Dictionary<CombatantResourceType, float> resourceCosts)
    {
        foreach (KeyValuePair<CombatantResourceType, float> resourceCost in resourceCosts)
        {
            if (combatant.GetResourceAmount(resourceCost.Key) < resourceCost.Value)
                return false;
        }

        return true;
    }

    private Dictionary<CombatantResourceType, float> GetTotalResourceCosts()
    {
        Dictionary<CombatantResourceType, float> totalCosts = new();
        foreach (SkillResourceCost resourceCost in resourceCosts)
        {
            totalCosts.TryGetValue(resourceCost.resourceType, out float total);
            totalCosts[resourceCost.resourceType] = total + resourceCost.amount;
        }

        return totalCosts;
    }
}
