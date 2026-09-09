using System.Collections;
using UnityEngine;

// TEMPORARY: DELETE LATER ONLY FOR DEMO
public class CombatantDamagedVFXTemporary : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float strength = 0.075f;
    [SerializeField] private float shakeFrequency = 50f;
    [SerializeField] private float flickerFrequency = 10f;

    private Coroutine flickerCoroutine;
    private Coroutine shakeCoroutine;
    private Renderer[] renderers;
    private bool[] originalRendererStates;
    private Vector3 originalLocalPosition;

    private void Awake()
    {
        renderers = combatant.GetComponentsInChildren<Renderer>();
        originalRendererStates = new bool[renderers.Length];
        originalLocalPosition = combatant.transform.localPosition;

        for (int i = 0; i < renderers.Length; i++)
            originalRendererStates[i] = renderers[i].enabled;
    }

    private void OnEnable()
    {
        Combatant.OnHPChanged += FlickerOnDamage;
    }

    private void OnDisable()
    {
        Combatant.OnHPChanged -= FlickerOnDamage;

        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        RestoreRendererStates();
        combatant.transform.localPosition = originalLocalPosition;
    }

    private void FlickerOnDamage(Combatant damagedCombatant, float damage)
    {
        if (damagedCombatant != combatant || damage >= 0)
            return;

        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        flickerCoroutine = StartCoroutine(Flicker());
        shakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Flicker()
    {
        float elapsed = 0f;
        float toggleTimer = 0f;
        bool renderersEnabled = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            toggleTimer += Time.deltaTime;

            if (toggleTimer >= 1f / Mathf.Max(flickerFrequency, 1f))
            {
                renderersEnabled = !renderersEnabled;
                SetRenderersEnabled(renderersEnabled);
                toggleTimer = 0f;
            }

            yield return null;
        }

        RestoreRendererStates();
        flickerCoroutine = null;
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;
        float shakeTimer = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            shakeTimer += Time.deltaTime;

            if (shakeTimer >= 1f / Mathf.Max(shakeFrequency, 1f))
            {
                float fade = 1f - elapsed / duration;
                combatant.transform.localPosition = originalLocalPosition + fade * strength * Random.insideUnitSphere;
                shakeTimer = 0f;
            }

            yield return null;
        }

        combatant.transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }

    private void SetRenderersEnabled(bool enabled)
    {
        foreach (Renderer renderer in renderers)
            renderer.enabled = enabled;
    }

    private void RestoreRendererStates()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].enabled = originalRendererStates[i];
    }
}
