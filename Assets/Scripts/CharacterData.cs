using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private string characterTitle;
    [SerializeField] private Sprite portrait;

    public string CharacterName => characterName;
    public string CharacterTitle => characterTitle;
    public Sprite Portrait => portrait;
}
