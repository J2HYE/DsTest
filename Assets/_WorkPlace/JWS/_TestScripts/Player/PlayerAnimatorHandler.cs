using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerAnimatorHandler : MonoBehaviour
{
    private FaceShapeType currentType = FaceShapeType.Blink;
    private FaceShapeType preType;
    private Animator animator;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private float headIKWeight = 1f;
    // private float eyesIKWeight = 1f;
    private SpeechDataBase alphabetDictionary;
    private Transform lookAtTarget;
    private bool isLookAt = false;

    private Dictionary<FaceShapeType, float> faceDurations = new Dictionary<FaceShapeType, float>
    {
        { FaceShapeType.Blink, 0.1f },
        { FaceShapeType.Joy, 1f },
        { FaceShapeType.Angry, 0.8f },
        { FaceShapeType.Sorrow, 0.6f },
        { FaceShapeType.Fun, 0.5f }
    };

    private void OnEnable()
    {
        if (alphabetDictionary == null) LoadGameStateData();
    }

    private void Start()
    {
        animator = GameManager.playerTransform.GetComponentInChildren<Animator>();
        skinnedMeshRenderer = animator.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        preType = FaceShapeType.Fun;
        Invoke(nameof(BlinkPlayer), 3f);
    }

    private void Update()
    {
        if (currentType != preType)
        {
            preType = currentType;
            ChangeFace(currentType, 100f);
        }
    }

    private void LoadGameStateData()
    {
        Addressables.LoadAssetAsync<SpeechDataBase>("SpeechData").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                alphabetDictionary = handle.Result;
            }
            else
            {
                Debug.LogError("SpeechData 로드 실패.");
            }
        };
    }

    private void BlinkPlayer()
    {
        float Interval = Random.Range(2.5f, 5f);
        Invoke(nameof(BlinkPlayer), Interval);
        ChangeFace(FaceShapeType.Blink, 100f);
    }

    private void ChangeBlendShape(FaceShapeType type, float value)
    {
        int index = (int)type;
        value = Mathf.Clamp(value, 0f, 100f);
        skinnedMeshRenderer.SetBlendShapeWeight(index, value);
    }

    private void SetSpeechShape(float a, float i, float u, float e, float o)
    {
        ChangeBlendShape(FaceShapeType.Mouth_A, a);
        ChangeBlendShape(FaceShapeType.Mouth_I, i);
        ChangeBlendShape(FaceShapeType.Mouth_U, u);
        ChangeBlendShape(FaceShapeType.Mouth_E, e);
        ChangeBlendShape(FaceShapeType.Mouth_O, o);
    }

    private IEnumerator AnimateText(string text, float delay)
    {
        TextMeshProUGUI[] subDisplay = UIManager.Instance.DisplaySpeechWindow.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        Button acceptButton = UIManager.Instance.DisplaySpeechWindow.GetComponentInChildren<Button>(includeInactive: true);
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(onClickAction);
        acceptButton.gameObject.SetActive(false);
        subDisplay[0].text = "Player";
        subDisplay[1].text = "";
        subDisplay[2].text = "닫기";
        ChangeBlendShape(FaceShapeType.Joy, 100);
        foreach (char c in text)
        {
            string key = c.ToString().ToUpper();
            if (alphabetDictionary.TryGetBlendShape(key, out float[] weights))
            {
                SetSpeechShape(weights[0], weights[1], weights[2], weights[3], weights[4]);
            }
            subDisplay[1].text += c;
            yield return new WaitForSeconds(delay);
        }
        SetSpeechShape(0f, 0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(0.5f);
        acceptButton.gameObject.SetActive(true);
        ChangeBlendShape(FaceShapeType.Joy, 0);
    }

    public IEnumerator SpeechAnimateText(TextMeshProUGUI subDisplay, string comment, FaceShapeType faceType = FaceShapeType.Blink, float delay = 0.01f)
    {
        subDisplay.text = "";
        SetLookAt(Camera.main.transform, true);
        foreach (char c in comment)
        {
            string key = c.ToString().ToUpper();
            if (alphabetDictionary.TryGetBlendShape(key, out float[] weights))
            {
                SetSpeechShape(weights[0], weights[1], weights[2], weights[3], weights[4]);
            }
            subDisplay.text += c;
            yield return new WaitForSeconds(delay);
        }
        SetSpeechShape(0f, 0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(0.5f);
        SetLookAt(null, false);
    }

    private IEnumerator AnimateFace(FaceShapeType type, float value, float durate = 0f)
    {
        float duration = faceDurations.ContainsKey(type) ? faceDurations[type] : 0.2f;
        if (durate > duration) duration = durate;
        ChangeBlendShape(type, value);
        yield return new WaitForSeconds(duration);
        ChangeBlendShape(type, 0f);
    }

    private void onClickAction()
    {
        UIManager.Instance.ToggleDialog();
    }

    public void SpeakText(string text, float delay)
    {
        UIManager.Instance.ToggleDialog(true);
        StartCoroutine(AnimateText(text, delay));
    }

    public void ChangeFace(FaceShapeType type, float value = 100f)
    {
        currentType = type;
        StartCoroutine(AnimateFace(type, value));
    }

    public void SetLookAt(Transform target, bool value)
    {
        isLookAt = value;
        lookAtTarget = target;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("들어왔나?");
        if (animator == null ) return;

        Debug.Log("들어왔나?");
        if (isLookAt && lookAtTarget != null)
        {
            animator.SetLookAtWeight(headIKWeight, 0.5f, 0.6f, 0.8f, 0.5f);
            animator.SetLookAtPosition(lookAtTarget.position);
        }
        else
        {
            isLookAt = false;
            lookAtTarget = null;
            animator.SetLookAtWeight(Mathf.Lerp(animator.GetFloat("LookAtWeight"), 0, Time.deltaTime * 2.0f));
        }
    }

}
