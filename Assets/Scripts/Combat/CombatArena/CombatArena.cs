using UnityEngine;

public class CombatArena : MonoBehaviour
{
    [SerializeField] private CombatArenaRow[] leftSidePositions;
    [SerializeField] private CombatArenaRow[] rightSidePositions;

    public void PositionCombatants(Combatant[] leftSideCombatants, Combatant[] rightSideCombatants)
    {
        PositionCombatants(leftSideCombatants, leftSidePositions);
        PositionCombatants(rightSideCombatants, rightSidePositions);
    }

    private void PositionCombatants(Combatant[] combatants, CombatArenaRow[] rows)
    {
        int combatantIndex = 0;

        foreach (CombatArenaRow row in rows)
        {
            foreach (Transform position in row.combatantPositions)
            {
                if (combatantIndex >= combatants.Length)
                    return;

                Combatant combatant = combatants[combatantIndex++];
                combatant.transform.SetParent(position, false);
                combatant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }
    }
}
