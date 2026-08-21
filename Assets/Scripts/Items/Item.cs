using UnityEngine;


[System.Serializable]
public abstract class Item
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public string description { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int value { get; private set; }
}
