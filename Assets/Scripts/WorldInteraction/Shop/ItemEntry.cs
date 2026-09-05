using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    private Shop shop;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private Image image;

    public void SetItemEntry(Item item, Shop shopReference)
    { 
        nameDisplay.text = item.name;
        costDisplay.text = item.value.ToString();
        image.sprite = item.icon;
        shop = shopReference;
    }
}
