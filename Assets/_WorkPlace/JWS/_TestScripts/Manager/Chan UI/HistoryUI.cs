using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryUI : MonoBehaviour
{
    private Button logButton; // 버튼 참조
    private TextMeshProUGUI logText; // 텍스트 참조
    private ScrollRect scrollRect; // 스크롤 참조
    private Animator animator;

    private void Awake()
    {
        // 필드 초기화
        if (logText == null)
        {
            logText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (scrollRect == null)
        {
            scrollRect = GetComponentInChildren<ScrollRect>();
        }

        if (logButton == null)
        {
            logButton = GetComponent<Button>();
            logButton.onClick.AddListener(() => ToggleHistoryLog());
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // 디버그 확인
        if (logText == null || scrollRect == null || logButton == null || animator == null)
        {
            Debug.LogError("HistoryUI의 필드가 제대로 연결되지 않았습니다.");
        }
    }

    public void DisplayHistory(MessageTag tags = MessageTag.전체)
    {
        logText.text = UIManager.HistoryManager.DisplayHistory(tags);
        Canvas.ForceUpdateCanvases(); // UI 강제 업데이트
        scrollRect.verticalNormalizedPosition = 0f; // 스크롤 위치 초기화 (아래로 이동)
    }

    private void ToggleHistoryLog()
    {
        animator.SetBool("Close", !animator.GetBool("Close"));
        UIManager.Instance.ToggleHistorylog();
    }
}
