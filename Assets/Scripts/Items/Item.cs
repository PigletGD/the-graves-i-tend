using UnityEngine;

public enum ItemType
{ 
    Reagent, Miscellaneous, Weapon, Trinket, Consumable
}

[CreateAssetMenu(fileName = "Base Item", menuName = "Scriptable Objects/Item/Base Item", order = 1)]
[System.Serializable]
public class Item : ScriptableObject
{
    ItemType type;

    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField] public ItemType itemType { get; private set; }
    [field: SerializeField, TextArea(3, 12)] public string description { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int value { get; private set; }
}
