using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Database", menuName = "Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private Combatant[] enemyPrefabs;

    public Combatant GetRandomEnemy()
    {
        Combatant enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        return Instantiate(enemyPrefab);
    }
}
