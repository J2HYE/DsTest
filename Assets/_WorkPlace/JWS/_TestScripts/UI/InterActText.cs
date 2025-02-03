using TMPro;
using UnityEngine;

public class InterActText : MonoBehaviour
{
    private Camera _camera;
    private RectTransform[] _rectTransforms;
    private TextMeshProUGUI textMsg;
    private string message = string.Empty;
    private float offsetHeght = 0;
    private int currentIndex = 0;
    private bool isSettings = false;

    private void Settings()
    {
        int childCount = transform.childCount;
        _rectTransforms = new RectTransform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.TryGetComponent<RectTransform>(out RectTransform rectTransform))
            {
                _rectTransforms[i] = rectTransform;
            }
            else
            {
                Debug.LogWarning($"자식 {child.name}에 RectTransform이 없습니다!");
            }
            child.gameObject.SetActive(false);
        }
        _camera = Camera.main;
        textMsg = _rectTransforms[currentIndex].GetChild(1).GetComponent<TextMeshProUGUI>();
        isSettings = true;
        ActivateCurrentIndex();
    }

    private void OnEnable()
    {
        if (isSettings) ActivateCurrentIndex();
    }

    private void OnDisable()
    {
        ChildTransformOff();
    }

    private void ChildTransformOff()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    private void ActivateCurrentIndex()
    {
        if (currentIndex >= 0 && currentIndex < _rectTransforms.Length)
        {
            _rectTransforms[currentIndex].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        textMsg.text = message;

        Vector3 screenPos = Vector3.zero;
        Vector3 offsetPosition = new Vector3(transform.parent.position.x, offsetHeght + 0.5f, transform.parent.position.z);

        if (currentIndex != 0)
        {
            Vector3 direction = (transform.parent.position - GameManager.playerTransform.position).normalized;
            float offset = direction.x > 0 ? 1 : -1;
            screenPos = _camera.WorldToScreenPoint(offsetPosition + Vector3.right * offset
            );
        }
        else
        {
            screenPos = _camera.WorldToScreenPoint(offsetPosition);
        }

        _rectTransforms[currentIndex].position = screenPos;
    }

    public void InteractTextSetting(string text, int index, float height, bool hasQuest = false)
    {
        this.message = text;
        this.currentIndex = index;
        this.offsetHeght = height;
        Settings();
        ChildTransformOff();
        ActivateCurrentIndex();
        if (currentIndex >= 0 && currentIndex < _rectTransforms.Length)
        {
            if (hasQuest && currentIndex == 0)
            {
                _rectTransforms[currentIndex].GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            if (!hasQuest)
            {
                _rectTransforms[currentIndex].GetChild(2).gameObject.SetActive(false);
            }
            textMsg = _rectTransforms[currentIndex].GetChild(1).GetComponent<TextMeshProUGUI>();
        }
    }
}
