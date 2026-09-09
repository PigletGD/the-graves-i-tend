using System.Collections;
using UnityEngine;

public class CombatantSkillVFXTemporary : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float angle = 30f;
    [SerializeField] private float duration = 0.25f;

    private Coroutine rotationCoroutine;
    private Quaternion originalLocalRotation;

    private void Awake()
    {
        originalLocalRotation = combatant.transform.localRotation;
    }

    private void OnEnable()
    {
        Combatant.OnSkillUsed += RotateOnSkillUsed;
    }

    private void OnDisable()
    {
        Combatant.OnSkillUsed -= RotateOnSkillUsed;

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        combatant.transform.localRotation = originalLocalRotation;
    }

    private void RotateOnSkillUsed(Combatant skillUser)
    {
        if (skillUser != combatant)
            return;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        float elapsed = 0f;
        Vector3 axis = rotationAxis.normalized;
        combatant.transform.localRotation = originalLocalRotation;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float rotationAmount = Mathf.Sin(progress * Mathf.PI) * angle;
            combatant.transform.localRotation = originalLocalRotation * Quaternion.AngleAxis(rotationAmount, axis);
            elapsed += Time.deltaTime;
            yield return null;
        }

        combatant.transform.localRotation = originalLocalRotation;
        rotationCoroutine = null;
    }
}
