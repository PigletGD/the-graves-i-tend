using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DamagePopupUI damagePopupPrefab;
    [SerializeField] private Vector3 popupOffset = new(0, 2f, 0); // Temporary. Move this elswhere?

    private RectTransform parentRect;

    private void Awake()
    {
        parentRect = (RectTransform)transform;
    }

    public void ShowDamagePopup(int damageAmount, Combatant combatant)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(combatant.transform.position) + popupOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);

        DamagePopupUI damagePopup = Instantiate(damagePopupPrefab, parentRect);
        damagePopup.Initialize(damageAmount, localPosition);
    }
}
