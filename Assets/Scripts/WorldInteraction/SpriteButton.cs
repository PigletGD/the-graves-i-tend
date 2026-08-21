using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteButton : AnimatedHover, IPointerClickHandler
{
    public UnityEvent pressEvents;

    public void OnPointerClick(PointerEventData eventData)
    {
        pressEvents.Invoke();
    }



}
