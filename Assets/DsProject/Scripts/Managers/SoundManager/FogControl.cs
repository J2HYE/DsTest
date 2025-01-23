using UnityEngine;

public class FogControl : MonoBehaviour
{
    [Header("한 사이클의 지속 시간 (초)")]
    [SerializeField] private float cycleDuration = 60f; // 한 사이클의 지속 시간 (초)
    [Header("Fog 밀도의 최대값")]
    [SerializeField] private float maxFogDensity = 0.15f;  // Fog 밀도의 최대값
    [Header("Fog 밀도의 최소값")]
    [SerializeField] private float minFogDensity = 0f;  // Fog 밀도의 최소값

    private float timer = 0f; // 내부 타이머

    void Start()
    {
        RenderSettings.fog = true; // Fog 활성화
                                   // HTML 색상 코드(HEX 코드) -> Color 변환
        Color fogColor;
        if (ColorUtility.TryParseHtmlString("#808080", out fogColor))
        {
            RenderSettings.fogColor = fogColor; // Fog 색상 설정
            Debug.Log("Fog 색상이 #808080로 설정되었습니다.");
        }
        else
        {
            Debug.LogError("잘못된 색상 코드입니다.");
        }
    }

    void Update()
    {
        // 타이머를 시간에 따라 증가시키며 주기적으로 반복
        timer += Time.deltaTime;

        // 주기적으로 0 ~ cycleDuration 사이의 값으로 제한
        float cycleTime = timer % cycleDuration;

        // 주기를 0 ~ 1 사이의 비율로 변환
        float t = cycleTime / cycleDuration;

        // Fog 밀도를 Sine 파형으로 조정 (부드럽게 왔다 갔다)
        float fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, Mathf.Sin(t * Mathf.PI));

        // Fog 밀도 설정
        RenderSettings.fogDensity = fogDensity;
    }
}