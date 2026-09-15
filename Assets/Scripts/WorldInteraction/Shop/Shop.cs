using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> shopItems;
    [SerializeField] private GameObject itemEntryPrefab;
    [SerializeField] private GameObject shopContent;

    [SerializeField] private TextMeshProUGUI quantityText;
    public ItemEntry selectedItemEntry { get; private set; }

    private int purchaseQuantity = 1;

    public void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject newItemEntry = Instantiate(itemEntryPrefab, shopContent.transform);
            newItemEntry.GetComponent<ItemEntry>().SetItemEntry(shopItems[i], this);
        }
        
    }


    public void IncreasePurchaseQuantity()
    {
        purchaseQuantity++;
        UpdatePurchaseQuantityText();
        // we can expand this further into long press increase quantity to buy more if there is a need to buy double digit quantity of items
    }
    public void DecreasePurchaseQuantity()
    {
        purchaseQuantity--;
        UpdatePurchaseQuantityText();
    }

    public void UpdatePurchaseQuantityText()
    { 
        quantityText.text = purchaseQuantity.ToString();
    }

    public void SetSelectedItem(ItemEntry itemEntry)
    {
        if (selectedItemEntry != null)
            selectedItemEntry.ItemEntrySelection(false);    // clear the previous selection


        selectedItemEntry = itemEntry;
        if (selectedItemEntry != null)
            selectedItemEntry.ItemEntrySelection(true);
        
    }

    public void BuyItem()
    {
        if (selectedItemEntry != null)
        {
            Debug.Log(purchaseQuantity.ToString() + " units of " + selectedItemEntry.item.itemName + " has been purchased");
            purchaseQuantity = 1;
            UpdatePurchaseQuantityText();
            SetSelectedItem(null);
        }
    }
}
