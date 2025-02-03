using UnityEngine;

[System.Serializable]
public class DialogData
{
    public string speakerName;  // 대사를 말하는 캐릭터 이름
    [TextArea] public string dialogText;  // 대사 내용
}

[System.Serializable]
public class CharacterMoveData
{
    public string charStoryNPC;  // 타겟 트랜스폼
    public Vector3 startPosition; // 시작 위치
    public Vector3 endPosition;   // 목표 위치
    public float moveSpeed = 0.2f;  // 이동 속도
    public bool shouldMove;       // 이동 여부
}

[System.Serializable]
public class CameraEffectData
{
    public bool enableShake;    // 카메라 흔들기 효과
    public float shakeIntensity = 0.5f; // 흔들림 강도
    public float shakeDuration = 0.5f;  // 지속 시간

    public bool enableZoom;  // 줌 효과 사용 여부
    public float zoomAmount = 5f; // 줌 강도
    public float zoomSpeed = 5f;  // 줌 속도
}

[System.Serializable]
public class SoundData
{
    public AudioClip bgmClip;   // 배경음악
    public float bgmVolume = 1.0f;
    public bool loopBgm = false;

    public AudioClip sfxClip;   // 효과음
    public float sfxVolume = 1.0f;
}

[System.Serializable]
public class ScreenEffectData
{
    public bool enableFade;  // 페이드 효과 사용 여부
    public float fadeDuration = 1.0f; // 페이드 속도
    public Color fadeColor = Color.black;  // 페이드 색상
}
