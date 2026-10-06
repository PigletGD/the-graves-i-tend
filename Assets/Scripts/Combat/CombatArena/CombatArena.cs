using UnityEngine;

public class CombatArena : MonoBehaviour
{
    [SerializeField] private CombatArenaRow[] leftSidePositions;
    [SerializeField] private CombatArenaRow[] rightSidePositions;

    public void PositionCombatants(Combatant[] leftSideCombatants, Combatant[] rightSideCombatants)
    {
        PositionCombatants(leftSideCombatants, leftSidePositions, false);
        PositionCombatants(rightSideCombatants, rightSidePositions, true);
    }

    private void PositionCombatants(Combatant[] combatants, CombatArenaRow[] rows, bool shouldFaceOpposite)
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

                if (shouldFaceOpposite)
                    combatant.transform.localScale = new(-combatant.transform.localScale.x, combatant.transform.localScale.y, combatant.transform.localScale.z);
            }
        }
    }
}
