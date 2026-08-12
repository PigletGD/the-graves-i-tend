using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject, ISkill
{ 
    // private Element element
    [SerializeField] private Attempt[] attempts;

    public void Execute(Battle battle, ITarget source, ITarget[] targets)
    {
        foreach (IAttempt attempt in attempts)
            attempt.Execute(battle, source, targets);
    }
}