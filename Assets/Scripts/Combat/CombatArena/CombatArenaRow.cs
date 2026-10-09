using UnityEngine;

public class CombatArenaRow : MonoBehaviour
{
    [SerializeField] private CombatArenaTile[] combatantTiles;

    public CombatArenaTile[] CombatantTiles => combatantTiles;
}
