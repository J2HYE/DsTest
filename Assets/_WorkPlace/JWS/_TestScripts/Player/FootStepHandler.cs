using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JWS
{
    public enum FaceShapeType
    {
        Blink,
        Mouth_A,
        Mouth_I,
        Mouth_U,
        Mouth_E,
        Mouth_O,
        Joy,
        Angry,
        Sorrow,
        Fun
    }

    public class AnimatorHandler : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private string conversation;
        // [SerializeField][Range(0, 1)] private float headIKWeight = 1f;
        // [SerializeField][Range(0, 1)] private float eyesIKWeight = 1f;
        private SpeechDataBase alphabetDictionary;
        private bool isLookAt = false;
        public bool isConversation = false;

        private Dictionary<FaceShapeType, float> faceDurations = new Dictionary<FaceShapeType, float>
    {
        { FaceShapeType.Blink, 0.2f },
        { FaceShapeType.Joy, 1f },
        { FaceShapeType.Angry, 0.8f },
        { FaceShapeType.Sorrow, 0.6f },
        { FaceShapeType.Fun, 0.5f }
    };

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (alphabetDictionary == null) LoadGameStateData();
        }

        private void Update()
        {
            if (isConversation)
            {
                SpeakText(conversation, 0.05f);
                isConversation = false;
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

        private IEnumerator AnimateFace(FaceShapeType type, float value, float durate = 0f)
        {
            float duration = faceDurations.ContainsKey(type) ? faceDurations[type] : 0.2f;
            if (durate > duration) duration = durate;
            ChangeBlendShape(type, value);
            yield return new WaitForSeconds(duration);
            ChangeBlendShape(type, 0f); // 초기화
        }

        private void onClickAction()
        {
            UIManager.Instance.ToggleDialog();
        }

        public void SpeakText(string text, float delay)
        {
            UIManager.Instance.ToggleDialog();
            StartCoroutine(AnimateText(text, delay));
        }

        public void ChangeFace(FaceShapeType type, float value)
        {
            StartCoroutine(AnimateFace(type, value));
        }

        public void OnFootstep(AnimationEvent animationEvent)
        {
            SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Run_02", animator.transform.position, 10f, false);
        }

        public void OnLand(AnimationEvent animationEvent)
        {
            animator.SetBool("Jump", false);
            SoundManager.Instance.PlayClipAtPoint("Ellen_Footsteps_Earth_Land_Walk_Forward_Landing_01", animator.transform.position, 0.2f, false);
        }

        public void SetLookAt(bool value)
        {
            isLookAt = value;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (isLookAt)
            {
                //Vector3 position = Vector3.zero;
                //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                //if (Physics.Raycast(ray, out RaycastHit hit))
                //{
                //    position = hit.point + Camera.main.transform.forward * 14f;
                //    animator.SetLookAtWeight(headIKWeight, 0.5f, 0.6f, 0.8f, 0.5f);
                //    animator.SetLookAtPosition(position);
                //}
                //else
                //{
                //    position = Camera.main.transform.forward * 14f;
                //    animator.SetLookAtWeight(headIKWeight, 0.5f, 0.6f, 0.8f, 0.5f);
                //}
                //animator.SetLookAtPosition(position);
            }
        }
    }
}