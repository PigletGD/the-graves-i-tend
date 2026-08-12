using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : AnimatedHover, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject store;

    public void OnPointerDown(PointerEventData eventData)
    {
        // behaviour testing
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Pointer down on " + gameObject.name);
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Pointer up on " + gameObject.name);
        }
    }

    
    
}
