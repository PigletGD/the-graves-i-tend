using System.Collections.Generic;
using UnityEngine;

public class CombatArena : MonoBehaviour
{
    [SerializeField] private CombatArenaRow[] leftSidePositions;
    [SerializeField] private CombatArenaRow[] rightSidePositions;
    [SerializeField] private CombatArenaTileData allyTileData;
    [SerializeField] private CombatArenaTileData enemyTileData;

    private Combatant currentCombatant;
    private HashSet<Combatant> hoveredCombatants = new();

    public void PositionCombatants(Combatant[] leftSideCombatants, Combatant[] rightSideCombatants)
    {
        currentCombatant = null;
        hoveredCombatants.Clear();

        PositionCombatants(leftSideCombatants, leftSidePositions, false);
        PositionCombatants(rightSideCombatants, rightSidePositions, true);
    }

    private void PositionCombatants(Combatant[] combatants, CombatArenaRow[] rows, bool isRightSide)
    {
        int combatantIndex = 0;

        foreach (CombatArenaRow row in rows)
        {
            foreach (CombatArenaTile tile in row.CombatantTiles)
            {
                CombatArenaTileData tileData = isRightSide ? enemyTileData : allyTileData;
                tile.SetTile(null, tileData);
                tile.SetBaseTile(tileData.BaseTile);
                tile.SetBaseTileOutline(tileData.BaseOutlineTile);
                tile.SetModifierTile(null);

                if (combatantIndex >= combatants.Length)
                    continue;

                Combatant combatant = combatants[combatantIndex++];
                tile.SetTile(combatant, tileData);
                combatant.transform.SetParent(tile.CombatantPosition, false);
                combatant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                if (isRightSide)
                    combatant.transform.localScale = new(-combatant.transform.localScale.x, combatant.transform.localScale.y, combatant.transform.localScale.z);

            }
        }
    }

    public void SetCurrentCombatant(Combatant combatant)
    {
        currentCombatant = combatant;
        RefreshCombatantTiles();
    }

    public void SetHoveredCombatant(Combatant combatant)
    {
        hoveredCombatants.Clear();
        if (combatant != null)
            hoveredCombatants.Add(combatant);
        RefreshCombatantTiles();
    }

    public void SetHoveredCombatants(IEnumerable<Combatant> combatants)
    {
        hoveredCombatants.Clear();
        foreach (Combatant combatant in combatants)
        {
            if (combatant != null)
                hoveredCombatants.Add(combatant);
        }
        RefreshCombatantTiles();
    }

    private void RefreshCombatantTiles()
    {
        RefreshCombatantTiles(leftSidePositions);
        RefreshCombatantTiles(rightSidePositions);
    }

    private void RefreshCombatantTiles(CombatArenaRow[] rows)
    {
        foreach (CombatArenaRow row in rows)
        {
            foreach (CombatArenaTile tile in row.CombatantTiles)
            {
                Combatant combatant = tile.Combatant;
                if (combatant == null)
                    continue;

                Sprite modifierSprite = hoveredCombatants.Contains(combatant) ? tile.TileData.TargetedSprite
                    : combatant == currentCombatant ? tile.TileData.CurrentTurnSprite : null;

                tile.SetModifierTile(modifierSprite);
            }
        }
    }
}
