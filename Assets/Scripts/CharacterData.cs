using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private string characterTitle;
    [SerializeField] private Sprite portrait;
    [SerializeField] private Skill basicAttack;
    [SerializeField] private Skill skipTurn;
    [SerializeField] private Skill[] skills;

    [Header("Base Stats")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float maxMP = 100f;
    [SerializeField] private float speed = 100f;

    public string CharacterName => characterName;
    public string CharacterTitle => characterTitle;
    public Sprite Portrait => portrait;
    public Skill BasicAttack => basicAttack;
    public Skill SkipTurn => skipTurn;
    public Skill[] Skills => skills;

    public float MaxHP => maxHP;
    public float MaxMP => maxMP;
    public float Speed => speed;
}
