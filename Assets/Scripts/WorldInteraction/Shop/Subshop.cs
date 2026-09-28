using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public enum SubshopType
{
    All, Consumable, Equipment, Reagent
}

/// <summary>
/// Used for different shop panels of various types
/// </summary>
public class Subshop : MonoBehaviour
{
    [SerializeField] private List<ItemEntry> itemList;
    [SerializeField] public GameObject subShopPanel;

    [field:SerializeField] public SubshopType subshopType { get; private set; }
    [SerializeField] private GameObject itemEntryPrefab;

    public void SetSubShopItems(List<Item> fullItemList, Shop shop)
    {

        for (int i = 0; i < fullItemList.Count; i++)
        {
            if (ItemToTypeToSubshopType(fullItemList[i]) == this.subshopType || subshopType == SubshopType.All)
            {
                GameObject newItemEntry = Instantiate(itemEntryPrefab, subShopPanel.transform);
                itemList.Add(newItemEntry.GetComponent<ItemEntry>());
                newItemEntry.GetComponent<ItemEntry>().SetItemEntry(fullItemList[i], shop);
            }
        }
    }

    public void DestroySubshopList()
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            Object.Destroy(itemList[i].gameObject);
        }
    }

    public SubshopType ItemToTypeToSubshopType(Item item)
    {
        // not sure if I'll start using type of <class> later on but for now since those classes are not implement, we'll be using this
        switch (item.itemType)
        {
            case ItemType.Consumable:
                return SubshopType.Consumable;
            case ItemType.Weapon:
                return SubshopType.Equipment;
            case ItemType.Trinket:
                return SubshopType.Equipment;
            case ItemType.Miscellaneous:
                return SubshopType.Reagent;
            case ItemType.Reagent:
                return SubshopType.Reagent;
        }

        // this last return should never happen
        return SubshopType.All;
    }
}
