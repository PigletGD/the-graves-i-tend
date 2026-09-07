using UnityEngine;

// TODO: Object Pooling
public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DamagePopupUI damagePopupPrefab;
    [SerializeField] private Vector3 popupOffset = new(0, 2f, 0); // Move this elswhere?

    private RectTransform parentRect;

    private void Awake()
    {
        parentRect = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Combatant.OnDamageTaken += ShowDamagePopup;
    }
    
    private void OnDisable()
    {
        Combatant.OnDamageTaken -= ShowDamagePopup;
    }

    private void ShowDamagePopup(Combatant combatant, float damageAmount)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(combatant.transform.position) + popupOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);

        DamagePopupUI damagePopup = Instantiate(damagePopupPrefab, parentRect);
        damagePopup.Initialize((int)damageAmount, localPosition);
    }
}
