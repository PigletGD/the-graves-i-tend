using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> shopItems;
    [SerializeField] private GameObject itemEntryPrefab;
    [SerializeField] private GameObject shopContent;

    public void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject newItemEntry = Instantiate(itemEntryPrefab, shopContent.transform);
            newItemEntry.GetComponent<ItemEntry>().SetItemEntry(shopItems[i], this);
        }
        
    }
}
