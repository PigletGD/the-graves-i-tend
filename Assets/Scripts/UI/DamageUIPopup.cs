using TMPro;
using UnityEngine;

public class DamagePopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeDuration = 1f;

    public void Initialize(int damageAmount, Vector3 position)
    {
        damageText.rectTransform.anchoredPosition = position;
        damageText.text = damageAmount.ToString();
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
}
