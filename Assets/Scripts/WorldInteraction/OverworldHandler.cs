using UnityEngine;

public class OverworldHandler : MonoBehaviour
{
    public enum OverworldContext
    {
        Town = 0,
        Shop = 1,
        Forest = 2,
    }
    
    public static OverworldContext TargetContext = OverworldContext.Town;
    
    [SerializeField] private AnimatedPanel townPanel;
    [SerializeField] private AnimatedPanel shopPanel;
    [SerializeField] private AnimatedPanel forestPanel;

    private void Awake()
    {
        DisableAllAnimatedPanels();

        switch (TargetContext)
        {
            case OverworldContext.Town:
                townPanel.gameObject.SetActive(true);
                break;
            case OverworldContext.Shop:
                shopPanel.gameObject.SetActive(true);
                break;
            case OverworldContext.Forest:
                forestPanel.gameObject.SetActive(true);
                break;
        }
    }

    private void DisableAllAnimatedPanels()
    {
        townPanel.gameObject.gameObject.SetActive(false);
        shopPanel.gameObject.gameObject.SetActive(false);
        forestPanel.gameObject.gameObject.SetActive(false);
    }
}
