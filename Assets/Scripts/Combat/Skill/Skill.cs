using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject, ISkill
{
    // private string name; // We can probably just reuse the SO name.
    // private Element element
    [SerializeField] private Attempt[] attempts;

    public void Execute()
    {
        Debug.Log($"{name} was used!");
        foreach (IAttempt attempt in attempts)
            attempt.Execute();
    }
}