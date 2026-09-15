using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Image image;

    [SerializeField] protected Color normalColor;
    [SerializeField] protected Color hoveredColor;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // play animation
        // we can use this even if we don't have an animation if we just make if statements
        if (spriteRenderer != null) 
            spriteRenderer.color = hoveredColor;
        
        if (image != null)
            image.color = hoveredColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //stop animation
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
        
        if (image != null)
            image.color = normalColor;
    }
}
