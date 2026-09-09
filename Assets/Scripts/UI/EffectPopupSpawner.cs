using UnityEngine;

// TODO: Object Pooling
public class EffectPopupUISpawner : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private EffectPopupUI effectPopupPrefab;
    [SerializeField] private Vector3 popupOffset = new(0, 380f, 0); // Move this elswhere?

    private RectTransform parentRect;

    private void Awake()
    {
        parentRect = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Combatant.OnHPChanged += ShowDamagePopup;
        Attempt.OnAttemptMissed += ShowDamagePopup;
    }
    
    private void OnDisable()
    {
        Combatant.OnHPChanged -= ShowDamagePopup;
        Attempt.OnAttemptMissed -= ShowDamagePopup;
    }

    private void ShowDamagePopup(ITarget target, string message)
    {
        if (target is Combatant combatant)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(combatant.transform.position) + popupOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);

            EffectPopupUI effectPopup = Instantiate(effectPopupPrefab, parentRect);
            effectPopup.Initialize(localPosition, message);
        }
    }

    private void ShowDamagePopup(Combatant combatant, float damageAmount)
    {
        ShowDamagePopup(combatant, damageAmount.ToString());
    }

    private void ShowDamagePopup(ITarget invoker, ITarget target)
    {
        if (target is Combatant combatant)
            ShowDamagePopup(combatant, "Missed!");
    }
}
