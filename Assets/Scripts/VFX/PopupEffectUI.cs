using System;
using TMPro;
using UnityEngine;

public class PopupEffectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeDuration = 1f;

    public event Action<PopupEffectUI> OnDestroyed;

    public void Initialize(Vector3 position, string message)
    {
        damageText.rectTransform.anchoredPosition = position;
        damageText.SetText(message);
    }

    public void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.up;

        Color color = damageText.color;
        color.a -= Time.deltaTime / fadeDuration;
        damageText.color = color;

        if (color.a <= 0f)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
