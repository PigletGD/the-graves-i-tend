using UnityEngine;

public class EnemyCombatantPanel : CombatantPanel
{
    private RectTransform rectTranform;
    private RectTransform parentRectTransform;
    private Canvas canvas;
    private bool positionPending;

    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        name = $"{combatant.name} On-Field HUD";
        rectTranform = (RectTransform)transform;
        parentRectTransform = (RectTransform)transform.parent;
        canvas = GetComponentInParent<Canvas>();
        positionPending = true;
    }

    // UI does not update properly even though we're positioning Combatants before Initialize() gets called.
    private void LateUpdate()
    {
        if (!positionPending)
            return;

        positionPending = false;
        UpdateScreenPosition();
    }

    private void UpdateScreenPosition()
    {
        Bounds colliderBounds = cachedCombatant.BoxCollider.bounds;
        Vector3 colliderAnchor = new(colliderBounds.center.x, colliderBounds.max.y, colliderBounds.center.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(colliderAnchor);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, canvas.worldCamera, out Vector2 localPosition))
        {
            localPosition -= new Vector2(0, rectTranform.rect.height * 0.75f);
            rectTranform.anchoredPosition = localPosition;
        }
    }
}