using TMPro;
using UnityEngine;

public class InterActText : MonoBehaviour
{
    private Camera _camera;
    private RectTransform[] _rectTransforms;
    private TextMeshProUGUI textMsg;
    private string message = string.Empty;
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
        //Material material = textMsg.fontMaterial;
        //float reflectivity = Mathf.PingPong(Time.time * 4, 10) + 5;
        //material.SetFloat("_Reflectivity", reflectivity);
        Vector3 screenPos = Vector3.zero;
        if (currentIndex != 0)
        {
            Vector3 direction = (transform.parent.position - GameManager.playerTransform.position).normalized;
            float offset = direction.x > 0 ? 1: -1;
            screenPos = _camera.WorldToScreenPoint(
                transform.parent.position + Vector3.up * 1.5f + Vector3.right * offset
            );
        }
        else
        {
            screenPos = _camera.WorldToScreenPoint(transform.parent.position + Vector3.up * 1.5f);
        }
        _rectTransforms[currentIndex].position = screenPos;
    }


    public void InteractTextSetting(string text, int index, bool hasQuest = false)
    {
        this.message = text;
        this.currentIndex = index;
        Settings();
        ChildTransformOff();
        ActivateCurrentIndex();
        if (currentIndex >= 0 && currentIndex < _rectTransforms.Length)
        {
            if (hasQuest && currentIndex == 0)
            {
                _rectTransforms[currentIndex].GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            textMsg = _rectTransforms[currentIndex].GetChild(1).GetComponent<TextMeshProUGUI>();
        }
    }
}
