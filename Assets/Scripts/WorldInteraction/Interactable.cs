using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : AnimatedHover, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject store;

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    
}
