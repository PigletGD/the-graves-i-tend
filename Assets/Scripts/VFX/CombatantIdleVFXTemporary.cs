using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combatant))]
public class CombatantIdleVFXTemporary : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private float amplitude = 0.05f;
    [SerializeField] private float frequency = 0.75f;

    private Transform[] visualTransforms;
    private float elapsed;
    private float appliedOffset;
    private float phaseOffset;
    private bool isCurrentActiveCombatant;

    private void Awake()
    {
        if (combatant == null)
            combatant = GetComponent<Combatant>();

        HashSet<Transform> visualRoots = new HashSet<Transform>();
        Renderer[] renderers = combatant.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            Transform visualRoot = renderer.transform;

            if (visualRoot == combatant.transform)
                continue;

            while (visualRoot.parent != combatant.transform)
                visualRoot = visualRoot.parent;

            visualRoots.Add(visualRoot);
        }

        if (visualRoots.Count == 0)
        {
            Debug.LogError($"{nameof(CombatantIdleVFXTemporary)} could not find a visual child to animate.", this);
            enabled = false;
            return;
        }

        visualTransforms = new Transform[visualRoots.Count];
        visualRoots.CopyTo(visualTransforms);
    }

    private void OnEnable()
    {
        elapsed = 0f;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI);
        Combat.OnTurnStarted += OnTurnStarted;
        Combat.OnTurnEnded += OnTurnEnded;
    }

    private void OnDisable()
    {
        Combat.OnTurnStarted -= OnTurnStarted;
        Combat.OnTurnEnded -= OnTurnEnded;
        isCurrentActiveCombatant = false;

        if (visualTransforms == null)
            return;

        Vector3 appliedOffsetVector = Vector3.up * appliedOffset;

        foreach (Transform visualTransform in visualTransforms)
        {
            if (visualTransform != null)
                visualTransform.localPosition -= appliedOffsetVector;
        }

        appliedOffset = 0f;
    }

    private void LateUpdate()
    {
        elapsed += Time.deltaTime;
        float offset = Mathf.Sin(elapsed * frequency * 2f * Mathf.PI + phaseOffset) * (amplitude * (isCurrentActiveCombatant ? 2f : 1f));
        Vector3 offsetDelta = Vector3.up * (offset - appliedOffset);

        foreach (Transform visualTransform in visualTransforms)
            visualTransform.localPosition += offsetDelta;

        appliedOffset = offset;
    }

    private void OnTurnStarted(Combatant activeCombatant)
    {
        isCurrentActiveCombatant = activeCombatant == combatant;
    }

    private void OnTurnEnded(Combatant endedCombatant)
    {
        isCurrentActiveCombatant = false;
    }
}
