using System.Collections;
using UnityEngine;

public class CombatantSkillVFXTemporary : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float forwardDistance = 1f;
    [SerializeField] private float stunShakeDistance = 0.1f;

    private Coroutine movementCoroutine;
    private Vector3 returnLocalPosition;

    private void OnEnable()
    {
        Combatant.OnSkillUsed += OnSkillUsed;
        Combatant.OnTurnSkipped += OnTurnSkipped;
    }

    private void OnDisable()
    {
        Combatant.OnSkillUsed -= OnSkillUsed;
        Combatant.OnTurnSkipped -= OnTurnSkipped;

        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
            combatant.transform.localPosition = returnLocalPosition;
        }
    }

    private void OnSkillUsed(Combatant invoker, Skill skill)
    {
        if (invoker != combatant || skill.name == "Skip Turn")
            return;

        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            combatant.transform.localPosition = returnLocalPosition;
        }

        returnLocalPosition = combatant.transform.localPosition;
        movementCoroutine = StartCoroutine(MoveForwardAndBack());
    }

    private void OnTurnSkipped(Combatant disabledCombatant, StatusEffectType reason)
    {
        if (disabledCombatant != combatant)
            return;

        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            combatant.transform.localPosition = returnLocalPosition;
        }

        returnLocalPosition = combatant.transform.localPosition;
        movementCoroutine = StartCoroutine(ShakeWhileDisabled());
    }

    private IEnumerator ShakeWhileDisabled()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float fade = Mathf.Sin(Mathf.PI * elapsed / duration);
            float offset = Mathf.Sin(elapsed * 40f) * stunShakeDistance * fade;
            combatant.transform.localPosition = returnLocalPosition + Vector3.right * offset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        combatant.transform.localPosition = returnLocalPosition;
        movementCoroutine = null;
    }

    private IEnumerator MoveForwardAndBack()
    {
        float direction = Mathf.Sign(combatant.transform.localScale.x);
        Vector3 forwardPosition = returnLocalPosition + Vector3.right * (forwardDistance * direction);

        yield return MoveTo(forwardPosition);
        yield return MoveTo(returnLocalPosition);

        movementCoroutine = null;
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        Vector3 startPosition = combatant.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float progress = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            combatant.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            elapsed += Time.deltaTime;
            yield return null;
        }

        combatant.transform.localPosition = targetPosition;
    }
}
