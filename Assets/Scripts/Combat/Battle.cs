using UnityEngine;

// Refer to this dood youtube video on how I'm basing the combat on: https://www.youtube.com/watch?v=CyRtTwKeulE.
// TL;DR: Each Skill has N attempts where each attempt has N effects.
// Refactor as necessary. Only doing this so that there's something actionable.

// TO DO: 
// Combatant.cs
// Interface ITarget is in a weird spot right now. Needed to use a getter in Attempt.cs to execute basic attack.
public class Battle : MonoBehaviour
{
    // Combatant attacker;
    [SerializeField] Combatant defender;
    [SerializeField] private Skill basicAttack;

    public Combatant Defender => defender;

    private void Start()
    {
        InvokeRepeating(nameof(ExecuteAttempts), 0, 1f);
    }

    private void ExecuteAttempts()
    {
        basicAttack.Execute(this);
    }
}
