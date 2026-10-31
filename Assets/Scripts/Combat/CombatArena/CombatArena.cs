using System.Collections.Generic;
using UnityEngine;

public class CombatArena : MonoBehaviour
{
    // TODO: Remove once we have a proper way of setting up starting position data for enemies
    public struct CombatantSetupInfo
    {
        public int CombatantIndex;
        public int TileIndex;
    }
    
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

        PositionCombatants(leftSideCombatants, leftSidePositions, false, false);
        PositionCombatants(rightSideCombatants, rightSidePositions, true, true);
    }

    private void PositionCombatants(Combatant[] combatants, CombatArenaRow[] rows, bool isRightSide, bool randomizePositions)
    {
        int combatantIndex = 0;
        
        // TODO: Have a way to assign troops to different tiles in the 2x3 tile setup. Randomizing positions for enemies for now
        int tileIndex = 0;
        CombatantSetupInfo[] infos = new CombatantSetupInfo[combatants.Length];
        if (randomizePositions)
        {
            var tileIndexes = new List<int>();
            foreach (CombatArenaRow row in rows)
            {
                foreach (CombatArenaTile tile in row.CombatantTiles)
                {
                    tileIndexes.Add(tileIndex);
                    tileIndex++;
                }
            }

            tileIndex = 0;

            for (int i = 0; i < infos.Length; i++)
            {
                var indexToCheck = Random.Range(0, tileIndexes.Count);
                var newTileIndex = tileIndexes[indexToCheck];
                tileIndexes.RemoveAt(indexToCheck);

                infos[i] = new CombatantSetupInfo()
                {
                    CombatantIndex = i,
                    TileIndex = newTileIndex,
                };
            }
        }

        foreach (CombatArenaRow row in rows)
        {
            foreach (CombatArenaTile tile in row.CombatantTiles)
            {
                CombatArenaTileData tileData = isRightSide ? enemyTileData : allyTileData;
                tile.SetTile(null, tileData);
                tile.SetBaseTile(tileData.BaseTile);
                tile.SetBaseTileOutline(tileData.BaseOutlineTile);
                tile.SetModifierTile(null);

                if (!randomizePositions)
                {
                    if (combatantIndex >= combatants.Length)
                        continue;

                    Combatant combatant = combatants[combatantIndex++];
                    tile.SetTile(combatant, tileData);
                    combatant.transform.SetParent(tile.CombatantPosition, false);
                    combatant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                    if (isRightSide)
                        combatant.transform.localScale = new(-combatant.transform.localScale.x, combatant.transform.localScale.y, combatant.transform.localScale.z);
                }
                else if (infos.Length > 0)
                {
                    Combatant combatant = null;
                    foreach (var info in infos)
                    {
                        if (info.TileIndex != tileIndex)
                            continue;

                        combatant = combatants[info.CombatantIndex];
                    }

                    if (combatant == null)
                    {
                        tileIndex++;
                        continue;
                    }
                    
                    tile.SetTile(combatant, tileData);
                    combatant.transform.SetParent(tile.CombatantPosition, false);
                    combatant.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    combatant.TileIndex = tileIndex;

                    if (isRightSide)
                        combatant.transform.localScale = new(-combatant.transform.localScale.x, combatant.transform.localScale.y, combatant.transform.localScale.z);
                    
                    tileIndex++;
                }
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
    
    public List<ITarget> GetTargetsInRow(Combatant combatant, bool rightSideCombatant)
    {
        if (combatant == null)
            return null;
        
        var targets = new List<ITarget>();
        var rows = rightSideCombatant ? rightSidePositions : leftSidePositions;

        int tileIndexToCheck = combatant.TileIndex;
        foreach (var row in rows)
        {
            if (row.CombatantTiles.Length < tileIndexToCheck)
            {
                tileIndexToCheck -= row.CombatantTiles.Length;
                continue;
            }

            foreach (var tiles in row.CombatantTiles)
            {
                if (tiles.Combatant == null)
                    continue;

                /*if (combatant.Equals(tiles.Combatant))
                    continue;*/
                
                targets.Add(tiles.Combatant);
            }

            break;
        }
        
        return targets;
    }
    
    public List<ITarget> GetTargetsInColumn(Combatant combatant, bool rightSideCombatant)
    {
        if (combatant == null)
            return null;
        
        var targets = new List<ITarget>();
        var rows = rightSideCombatant ? rightSidePositions : leftSidePositions;

        int column = combatant.TileIndex;
        foreach (var row in rows)
        {
            if (row.CombatantTiles.Length > column)
                break;
            
            column -= row.CombatantTiles.Length;
        }
        
        foreach (var row in rows)
        {
            if (row.CombatantTiles.Length <= column)
                continue;
            
            var combatantToCheck = row.CombatantTiles[column].Combatant;
            if (combatantToCheck == null)
                continue;
            
            /*if (combatant.Equals(combatantToCheck))
                continue;*/
            
            targets.Add(combatantToCheck);
        }
        
        return targets;
    }
}
