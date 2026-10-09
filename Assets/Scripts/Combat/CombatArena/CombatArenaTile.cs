using System;
using UnityEngine;

public class CombatArenaTile : MonoBehaviour
{
    [SerializeField] private Transform combatantPosition;
    [SerializeField] private SpriteRenderer baseTileRenderer;
    [SerializeField] private SpriteRenderer baseTileOutlineRenderer;
    [SerializeField] private SpriteRenderer modifierTileRenderer;

    public Combatant Combatant { get; private set; }
    public CombatArenaTileData TileData { get; private set; }

    public Transform CombatantPosition => combatantPosition;

    public void SetTile(Combatant combatant, CombatArenaTileData tileData)
    {
        Combatant = combatant;
        TileData = tileData;

        baseTileRenderer.gameObject.SetActive(Combatant != null);
    }

    public void SetBaseTile(Sprite sprite)
    {
        baseTileRenderer.sprite = sprite;
    }

    public void SetBaseTileOutline(Sprite sprite)
    {
        baseTileOutlineRenderer.sprite = sprite;
    }

    public void SetModifierTile(Sprite sprite)
    {
        modifierTileRenderer.sprite = sprite;
    }
}

[Serializable]
public class CombatArenaTileData
{
    [SerializeField] private Sprite baseTile;
    [SerializeField] private Sprite baseOutlineTile;
    [SerializeField] private Sprite currentTurnSprite;
    [SerializeField] private Sprite targetedSprite;

    public Sprite BaseTile => baseTile;
    public Sprite BaseOutlineTile => baseOutlineTile;
    public Sprite CurrentTurnSprite => currentTurnSprite;
    public Sprite TargetedSprite => targetedSprite;
}