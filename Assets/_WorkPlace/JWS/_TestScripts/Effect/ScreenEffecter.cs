using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenEffecter : MonoBehaviour
{
    public Image fadeImage; // UI의 Image 컴포넌트 연결
    public float fadeDuration = 1f; // 페이드 지속 시간
    public Material dissolveMaterial;
    public float dissolveSpeed = 1f;
    private float dissolveAmount = 0f;
    private bool isDissolving = false;
    private void Start()
    {
        // 게임 시작 시 페이드 인
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (isDissolving)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            dissolveAmount = Mathf.Clamp01(dissolveAmount);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            if (dissolveAmount >= 1f)
            {
                isDissolving = false;
            }
        }
    }

    public void StartDissolve()
    {
        dissolveAmount = 0f;
        isDissolving = true;
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Alpha 값을 점점 줄임
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Alpha 값을 점점 증가
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
