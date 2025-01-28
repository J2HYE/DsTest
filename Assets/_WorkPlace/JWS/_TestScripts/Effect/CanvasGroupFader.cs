using UnityEngine;
using System.Collections;

public class CanvasGroupFader : MonoBehaviour
{
    public CanvasGroup canvasGroup; // 페이드 효과를 적용할 Canvas Group
    public float fadeDuration = 1f; // 페이드 지속 시간

    private void Start()
    {
        // 테스트용: 게임 시작 시 페이드 인
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Alpha 값 증가
            yield return null;
        }
        canvasGroup.alpha = 1f; // 최종적으로 Alpha를 1로 고정
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Alpha 값 감소
            yield return null;
        }
        canvasGroup.alpha = 0f; // 최종적으로 Alpha를 0으로 고정
    }
}
