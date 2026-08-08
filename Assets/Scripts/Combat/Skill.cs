using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    // private string name; // We can probably just reuse the SO name.
    // private Element element
    [SerializeField] private Attempt[] attempt;
}