using UnityEngine;

public class EnemyCombatantPanel : CombatantPanel
{
    private RectTransform rectTranform;
    private RectTransform parentRectTransform;
    private Canvas canvas;
    //private bool positionPending;
    private int positionPendingFrames;
    
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        name = $"{combatant.name} On-Field HUD";
        rectTranform = (RectTransform)transform;
        parentRectTransform = (RectTransform)transform.parent;
        canvas = GetComponentInParent<Canvas>();
        //positionPending = true;
        
        // TODO: Fix this once we figure out the UI enemy HP bar centering issue. That issue is most likely caused by the bounds not getting initalized yet on scene awake, so the bounds are still defined as (0, 0, 0)
        positionPendingFrames = 5;
    }

    // UI does not update properly even though we're positioning Combatants before Initialize() gets called.
    private void LateUpdate()
    {
        /*if (!positionPending)
            return;

        positionPending = false;*/

        if (positionPendingFrames <= 0)
            return;
        
        positionPendingFrames--;
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