using System.Collections;
using TMPro;
using UnityEngine;

public class PickupItemTextFadeout : MonoBehaviour
{
    public float fadeDuration = 4f;
    private TextMeshProUGUI textComponent;

    void OnEnable()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            Color color = textComponent.color;
            color.a = 1.0f;
            textComponent.color = color;
        }
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            if (textComponent != null)
            {
                Color color = textComponent.color;
                color.a = alpha;
                textComponent.color = color;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
