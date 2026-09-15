using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEntry : SpriteButton
{
    private Shop shop;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private Image itemImage;
    [SerializeField] private Color selectedColor;

    public Item item { get; private set; }

    public void SetItemEntry(Item newItem, Shop shopReference)
    { 
        nameDisplay.text = newItem.name;
        costDisplay.text = newItem.value.ToString();
        itemImage.sprite = newItem.icon;
        shop = shopReference;
        item = newItem;
    }

    /// <summary>
    /// Sets the item entry image color to corresponding color based on boolean
    /// </summary>
    public void ItemEntrySelection(bool isSelected)
    {
        if (isSelected)
            image.color = selectedColor;
        else 
            image.color = normalColor;
    }

    /// <summary>
    /// sets the reference of the shop's selected item
    /// </summary>
    public void ItemEntrySelected()
    {
        shop.SetSelectedItem(this);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (shop.selectedItemEntry != this)
            image.color = hoveredColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (shop.selectedItemEntry != this)
            image.color = normalColor;
    }
}
