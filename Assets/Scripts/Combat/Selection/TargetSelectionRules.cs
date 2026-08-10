using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetSelectionDefinition", menuName = "Scriptable Objects/TargetSelectionDefinition")]
public class TargetSelectionRules : ScriptableObject
{
    [SerializeReference, SerializeReferenceDropdown] private ITargetSelectionCondition<TargetSelectionArgs>[] conditions;
}
