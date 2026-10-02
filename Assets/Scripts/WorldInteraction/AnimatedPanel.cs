using UnityEngine;

public class AnimatedPanel : MonoBehaviour
{
    public virtual void OpenPanel()
    {
        // in case we add animations to opening and closing panels. Whether it be lerp or full blown animation
        this.gameObject.SetActive(true);
    }

    public virtual void ClosePanel() 
    {
        this.gameObject.SetActive(false);
    }
}
