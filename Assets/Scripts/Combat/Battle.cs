using System.Linq;
using UnityEngine;

public class Battle : MonoBehaviour
{
    // Character attacker;
    // Character defender;
    [SerializeField] private Skill skill;

    private void Start()
    {
        InvokeRepeating(nameof(ExecuteAttempts), 0, 1f);
    }
    
    private void ExecuteAttempts()
    {
        skill.Execute();
    }
}
