using UnityEngine;

[CreateAssetMenu(fileName = "Base Item", menuName = "Item/Base Item", order = 1)]
[System.Serializable]
public class Item
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public string description { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int value { get; private set; }
}
