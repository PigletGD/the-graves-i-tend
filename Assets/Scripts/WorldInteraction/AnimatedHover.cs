using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color hoveredColor;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // play animation
        // we can use this even if we don't have an animation if we just make if statements
        spriteRenderer.color = hoveredColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //stop animation
        spriteRenderer.color = normalColor;
    }
}
