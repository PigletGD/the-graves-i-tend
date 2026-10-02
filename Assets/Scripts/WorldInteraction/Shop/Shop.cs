using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> shopItems;

    [SerializeField] private GameObject itemEntryPrefab;
    [SerializeField] private GameObject shopContent;

    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    public ItemEntry selectedItemEntry { get; private set; }

    private int purchaseQuantity = 1;

    [SerializeField] private List<Subshop> subshops;

    public void Start()
    {
        for (int i = 0; i < subshops.Count; i++)
        {
            subshops[i].SetSubShopItems(shopItems, this);
            if (subshops[i].subshopType != SubshopType.All)
                subshops[i].gameObject.SetActive(false);
        }
    }

    #region Purchase Quantity Functions

    public void SetPurchaseQuantity(int quantity)
    {
        purchaseQuantity = quantity;
        UpdatePurchaseQuantityText();
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

    #endregion

    public void UpdatePurchaseQuantityText()
    { 
        quantityText.text = purchaseQuantity.ToString();
    }

    public void SetSelectedItem(ItemEntry itemEntry)
    {
        if (selectedItemEntry != null)
            selectedItemEntry.ItemEntrySelection(false);    // clear the previous selection


        selectedItemEntry = itemEntry;
        if (selectedItemEntry != null)  // set new itemEntry
        {
            selectedItemEntry.ItemEntrySelection(true);
            itemDescriptionText.transform.parent.gameObject.SetActive(true);        // set whole description panel to be inactive. parent because the text is just the child
            itemDescriptionText.text = selectedItemEntry.item.description;
        }
        else
        {
            itemDescriptionText.transform.parent.gameObject.SetActive(false);       // set whole description panel to be inactive. parent because the text is just the child
        }
        
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

    /*
     * To be implemented as a way to update subshops whenever we add something to it while browsing the shop
    public void ResetSubshops()
    {
        for (int i = 0; i < subshops.Count; i++)
        { 
            
        }
    }
    */

    public void ChangeSubShop(int subshopType)
    {
        for (int i = 0; i < subshops.Count; i++)
        {
            if (subshopType == i)
                subshops[i].gameObject.SetActive(true);
            else
                subshops[i].gameObject.SetActive(false);
        }
        ResetShopDisplay();
    }

    public void ResetShopDisplay()
    {
        SetSelectedItem(null);
        SetPurchaseQuantity(1);
    }
}
