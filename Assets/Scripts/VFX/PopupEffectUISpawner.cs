using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// TODO: Object Pooling
public class PopupEffectUISpawner : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private PopupEffectUI effectPopupPrefab;
    [SerializeField] private Vector3 popupOffset = new(0, 380f, 0);
    [SerializeField] private float popupStackSpacing = 40f;
    [SerializeField] private float popupDelay = 0.05f;

    private RectTransform parentRect;
    private Dictionary<PopupEffectUI, Combatant> activePopups = new();

    private void Awake()
    {
        parentRect = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Combatant.OnHPChanged += OnHPChanged;
        Combatant.OnSkillUsed += OnSkillUsed;
        Attempt.OnAttemptMissed += OnAttemptMissed;
    }
    
    private void OnDisable()
    {
        Combatant.OnHPChanged -= OnHPChanged;
        Combatant.OnSkillUsed -= OnSkillUsed;
        Attempt.OnAttemptMissed -= OnAttemptMissed;
    }

    private void ShowPopup(ITarget target, string message)
    {
        if (target is Combatant combatant)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(combatant.transform.position) + popupOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, canvas.worldCamera, out Vector2 localPosition);

            int popupCount = 0;
            foreach (Combatant popupTarget in activePopups.Values)
            {
                if (popupTarget == combatant)
                    popupCount++;
            }

            localPosition += Vector2.up * (popupCount * popupStackSpacing);

            PopupEffectUI effectPopup = Instantiate(effectPopupPrefab, parentRect);
            effectPopup.OnDestroyed += OnPopupDestroyed;
            activePopups[effectPopup] = combatant;

            if (popupCount == 0)
            {
                effectPopup.Initialize(localPosition, message);
            }
            else
            {
                effectPopup.gameObject.SetActive(false);
                StartCoroutine(ShowPopupAfterDelay(effectPopup, localPosition, message, popupCount * popupDelay));
            }
        }
    }

    private IEnumerator ShowPopupAfterDelay(PopupEffectUI effectPopup, Vector3 position, string message, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (effectPopup == null)
            yield break;

        effectPopup.Initialize(position, message);
        effectPopup.gameObject.SetActive(true);
    }

    private void OnPopupDestroyed(PopupEffectUI effectPopup)
    {
        effectPopup.OnDestroyed -= OnPopupDestroyed;
        activePopups.Remove(effectPopup);
    }

    private void OnHPChanged(Combatant combatant, float damageAmount)
    {
        ShowPopup(combatant, damageAmount.ToString());
    }

    private void OnAttemptMissed(ITarget _, ITarget target)
    {
        if (target is Combatant combatant)
            ShowPopup(combatant, "Missed!");
    }
    
    private void OnSkillUsed(Combatant combatant, Skill skill)
    {
        if (skill.name == "Skip Turn")
            ShowPopup(combatant, "Skip Turn");
    }
}
