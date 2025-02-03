using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StorySceneManager : MonoBehaviour
{
    #region ✅ 변수선언
    public StorySequenceCollection storySequenceCollection;
    public List<Transform> storyCharNPCList = new List<Transform>();
    public Image fadeImage;
    public bool test = false;

    private PlayerAnimatorHandler playerAnimatorHandler;
    private TextMeshProUGUI[] display;
    private List<SequenceData> sequenceList;
    private StoryState currentState = StoryState.Idle;
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private Coroutine currentCorutine;
    private Transform sequenceTarget;
    private CameraType sequenceCameraType;
    private int currentSequenceIndex = 0;
    private bool isProcessing = false;
    #endregion

    private void Start()
    {
        currentCorutine = null;
        sequenceTarget = null;
        sequenceCameraType = CameraType.Orbit;
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sequenceList = new List<SequenceData>(storySequenceCollection.sequences);
        playerAnimatorHandler = CameraManager.Instance.transform.GetComponent<PlayerAnimatorHandler>();
        display = UIManager.Instance.DisplaySpeechWindow.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            StartStory(1);
        }
    }
    public void StartStory(int index)
    {
        if (currentCorutine != null) return;
        currentCorutine = StartCoroutine(RunStoryScene());
    }

    private void SetState(StoryState newState)
    {
        Debug.Log($"스토리 상태 변경: {currentState} -> {newState}");
        currentState = newState;
    }

    #region ✅ RunStoryScene()
    private IEnumerator RunStoryScene()
    {
        if (isProcessing) yield break;
        isProcessing = true;

        Dictionary<StoryEventType, System.Func<SequenceData, IEnumerator>> eventHandlers =
            new Dictionary<StoryEventType, System.Func<SequenceData, IEnumerator>>
            {
            { StoryEventType.FadeScreen, (seq) => FadeScreen(true, seq.screenEffect.fadeDuration, seq.screenEffect.fadeColor) },
            { StoryEventType.PlaySound, (seq) => { PlaySound(seq.soundData); return null; } },
            { StoryEventType.CameraShake, (seq) => CameraShake(seq.cameraEffect.shakeIntensity, seq.cameraEffect.shakeDuration) },
            { StoryEventType.CameraTransit, (seq) => CameraTransit(seq.cameraPoseList) },
            { StoryEventType.DisplayDialog, (seq) => DisplayDialog(seq.sequencesDialog) },
            { StoryEventType.MoveCharacter, (seq) => MoveCharacter(seq.moveData) },
            { StoryEventType.PlayerSpeeches, (seq) => PlayerSpeechDialog(seq.sequencesDialog) }
            };

        while (currentSequenceIndex < sequenceList.Count)
        {
            SequenceData currentSequence = sequenceList[currentSequenceIndex];
            sequenceTarget = GameManager.playerTransform;
            sequenceCameraType = currentSequence.cameraType;
            SetState(StoryState.Playing);

            foreach (var eventType in currentSequence.events)
            {
                if (eventHandlers.ContainsKey(eventType))
                {
                    var eventCoroutine = eventHandlers[eventType](currentSequence);
                    if (eventCoroutine != null) yield return StartCoroutine(eventCoroutine);
                }
            }

            currentSequenceIndex++;
            SetState(StoryState.Waiting);
        }

        SetState(StoryState.Completed);
        OnStoryComplete();
    }
    #endregion

    #region ✅ CameraTransit()
    private IEnumerator CameraTransit(CameraPoseList cameraPoselist)
    {
        if (cameraPoselist == null) yield break;

        isProcessing = true;
        SetState(StoryState.Transition);
        Transform character = sequenceTarget;

        CameraManager.SetCameraActive(GameManager.playerTransform, sequenceCameraType);
        foreach (var eventType in cameraPoselist.poseList)
        {
            //Camera.main.transform.position = cameraPose.position; 
            //Camera.main.transform.rotation = Quaternion.Euler(cameraPose.rotation);

            yield return new WaitForSeconds(eventType.transitionTime);
        }
        yield return new WaitForSeconds(1f);
        isProcessing = false;
        SetState(StoryState.Waiting);
    }
    #endregion

    #region ✅ DisplayDialog()
    private IEnumerator DisplayDialog(List<DialogData> dialogs)
    {
        UIManager.Instance.ToggleDialog(true);
        foreach (var dialog in dialogs)
        {
            if (isProcessing) yield break;
            isProcessing = true;
            display[0].text = dialog.speakerName;
            yield return StartCoroutine(AnimateText(display[1], dialog.dialogText));
            SetState(StoryState.Waiting);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            isProcessing = false;
        }
        yield return StartCoroutine(AnimateText(display[1], "📌 스페이스를 눌러주세요."));
        SetState(StoryState.Waiting);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        UIManager.Instance.ToggleDialog();
    }
    #endregion

    #region ✅ PlayerSpeechDialog()
    private IEnumerator PlayerSpeechDialog(List<DialogData> dialogs)
    {
        if (isProcessing) yield break;
        isProcessing = true;
        SetState(StoryState.Transition);
        CameraManager.SetCameraActive(GameManager.playerTransform, CameraType.UIview);
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ToggleDialog(true);
        SetState(StoryState.Waiting);
        foreach (var dialog in dialogs)
        {
            display[0].text = GameManager.playerTransform.name;
            yield return playerAnimatorHandler.SpeechAnimateText(display[1], dialog.dialogText);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        yield return StartCoroutine(AnimateText(display[1], "스페이스를 눌러주세요."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        UIManager.Instance.ToggleDialog();
        isProcessing = false;
    }
    #endregion

    #region ✅ AnimateText()
    private IEnumerator AnimateText(TextMeshProUGUI subDisplay, string message, float delay = 0.01f)
    {
        subDisplay.text = "";
        foreach (char c in message)
        {
            string key = c.ToString().ToUpper();
            subDisplay.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region ✅ MoveCharacter()
    private IEnumerator MoveCharacter(CharacterMoveData moveData)
    {
        if (isProcessing) yield break;
        isProcessing = true;
        SetState(StoryState.Playing);

        Transform character = sequenceTarget;

        CharacterController controller = character.GetComponent<CharacterController>();
        Animator animator = character.GetComponentInChildren<Animator>();

        Vector3 startPos = (moveData.startPosition == Vector3.zero && character.position != Vector3.zero)
              ? character.position
              : moveData.startPosition;

        Vector3 endPos = moveData.endPosition;
        float speed = moveData.moveSpeed;

        Vector3 moveDirection = (endPos - startPos).normalized;

        float distanceToTarget = Vector3.Distance(character.position, endPos);
        while (distanceToTarget > 0.3f)
        {
            animator.SetFloat("MotionSpeed", 1);
            animator.SetFloat("Speed", speed * 1.5f);
            animator.SetBool("Grounded", true);
            animator.SetBool("Freefall", false);
            controller.Move(moveDirection * speed * Time.deltaTime);
            distanceToTarget = Vector3.Distance(character.position, endPos);
            character.rotation = Quaternion.Slerp(character.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 5f);
            yield return null;
        }
        character.position = endPos;
        animator.SetFloat("MotionSpeed", 0);
        animator.SetFloat("Speed", 0);

        isProcessing = false;
        SetState(StoryState.Waiting);
    }
    #endregion

    #region ✅ CameraShake()
    private IEnumerator CameraShake(float intensity, float duration)
    {
        if (isProcessing) yield break;
        isProcessing = true;
        SetState(StoryState.Playing);
        Vector3 originalPosition = Camera.main.transform.position;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float shakeAmount = Mathf.Sin(t * Mathf.PI) * intensity;
            Camera.main.transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }
        Camera.main.transform.position = originalPosition;
        isProcessing = false;
        SetState(StoryState.Waiting);
    }
    #endregion

    #region ✅ FadeScreen()
    private IEnumerator FadeScreen(bool fadeIn, float duration, Color color)
    {
        if (isProcessing) yield break;
        isProcessing = true;
        SetState(StoryState.Transition);
        float startAlpha = fadeIn ? 1f : 0f;
        float targetAlpha = fadeIn ? 0f : 1f;
        float time = 0f;
        if (fadeImage == null)
        {
            Debug.LogError("FadeScreen() 오류: fadeImage가 설정되지 않았습니다!");
            isProcessing = false;
            SetState(StoryState.Waiting);
            yield break;
        }
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, targetAlpha);

        isProcessing = false;
        SetState(StoryState.Waiting);
    }
    #endregion

    #region ✅ PlaySound()
    private void PlaySound(SoundData soundData)
    {
        if (soundData.bgmClip != null)
        {
            bgmSource.clip = soundData.bgmClip;
            bgmSource.volume = soundData.bgmVolume;
            bgmSource.loop = soundData.loopBgm;
            bgmSource.Play();
        }
        if (soundData.sfxClip != null)
        {
            sfxSource.PlayOneShot(soundData.sfxClip, soundData.sfxVolume);
        }
    }
    #endregion

    #region ✅ OnStoryComplete()
    private void OnStoryComplete()
    {
        currentCorutine = null;
        Debug.Log("스토리씬이 끝났습니다.");
        //SceneManager.LoadScene("NextScene");
    }
    #endregion
}
