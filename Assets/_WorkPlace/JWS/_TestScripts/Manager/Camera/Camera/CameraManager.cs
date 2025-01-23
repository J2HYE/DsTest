using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{
    private Camera main_Camera;
    private CameraTarget playerCameraTarget = new CameraTarget();
    private CinemachineBrain cinemaBrain;
    private CinemachineCamera cinemaCamera;
    private CinemachineOrbitalFollow cinemaOrbitalFollow;
    private CinemachineInputAxisController cinemaInputAxis;

    [SerializeField] private AnimationCurve defaultBlendCurve;
    [SerializeField] private float defaultBlendTime = 2.0f;
    [SerializeField] private CinemachineBlenderSettings blenderSettings;

    private bool defaultCameraTarget = true;

    public static void MainCameraInputToggle(bool isOn)
    {
        Instance.cinemaInputAxis.enabled = isOn;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void InitCameraCinema()
    {

        main_Camera = Camera.main;
        if (main_Camera == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다.");
            return;
        }
        cinemaBrain = main_Camera.GetComponent<CinemachineBrain>();
        if (cinemaBrain == null)
        {
            Debug.LogError("CinemachineBrain 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        var activeVirtualCamera = cinemaBrain.ActiveVirtualCamera;
        if (activeVirtualCamera == null)
        {
            Debug.LogError("활성화된 Virtual Camera가 없습니다.");
            return;
        }
        cinemaCamera = activeVirtualCamera as CinemachineCamera;
        if (cinemaCamera == null)
        {
            Debug.LogError("활성화된 Virtual Camera가 CinemachineCamera 타입이 아닙니다.");
            return;
        }

        cinemaOrbitalFollow = cinemaCamera.GetComponentInChildren<CinemachineOrbitalFollow>();
        cinemaInputAxis = cinemaCamera.GetComponentInChildren<CinemachineInputAxisController>();
    }

    protected override void Start()
    {
        base.Start();
        InitCameraCinema();
        playerCameraTarget = new CameraTarget
        {
            TrackingTarget = GameManager.playerTransform,
            LookAtTarget = GameManager.playerTransform,
            CustomLookAtTarget = false
        };
        SetDefaultCameraTarget();
    }

    private void Update()
    {
        if (InputManager.InputActions.actions["ESC"].triggered)
            SetDefaultCameraTarget();
    }

    public static void SetDefaultCameraTarget()
    {
        if (Instance.cinemaCamera == null)
        {
            Debug.LogError("CinemachineCamera가 설정되지 않았습니다.");
            return;
        }

        // 현재 카메라 상태 저장
        ICinemachineCamera currentCamera = Instance.cinemaBrain.ActiveVirtualCamera;

        // 디폴트 타겟 설정
        Instance.cinemaCamera.Target = Instance.playerCameraTarget;

        // 카메라 설정
        Instance.cinemaCamera.Lens.FieldOfView = 60f;
        Instance.cinemaOrbitalFollow.VerticalAxis.Value = 17f;
        Instance.cinemaOrbitalFollow.HorizontalAxis.Value = 0f;
        Instance.cinemaCamera.transform.GetComponent<CinemachineRotationComposer>().TargetOffset = new Vector3(0, 2, 0);

        // 입력 활성화
        MainCameraInputToggle(true);

        // Blend 추가
        //Instance.ApplyBlend(currentCamera, Instance.cinemaCamera);

        Debug.Log("카메라가 디폴트 타겟으로 복귀되었습니다.");
    }


    private void ApplyBlend(ICinemachineCamera fromCamera, ICinemachineCamera toCamera)
    {
        if (fromCamera == null || toCamera == null)
        {
            Debug.LogWarning("Blend를 적용할 수 없습니다. 유효한 카메라가 없습니다.");
            return;
        }

        // BlenderSettings가 없는 경우 기본 Blend 설정 적용
        if (blenderSettings == null)
        {
            Debug.LogWarning("CinemachineBlenderSettings가 설정되지 않았습니다. 기본 Blend를 사용합니다.");
            var blendDefinition = new CinemachineBlendDefinition
            {
                Style = CinemachineBlendDefinition.Styles.EaseInOut, // 기본 EaseInOut 스타일
                Time = defaultBlendTime // 기본 Blend 시간
            };

            // 임시로 Blend 생성 및 적용
            var customBlend = new CinemachineBlend
            {
                CamA = fromCamera,
                CamB = toCamera,
                BlendCurve = blendDefinition.BlendCurve,
                Duration = blendDefinition.Time,
                TimeInBlend = 0
            };

            cinemaBrain.CustomBlends = null; // 기본 CustomBlends 해제
            Debug.Log("기본 Blend로 전환합니다.");
        }
        else
        {
            // CinemachineBrain에 BlenderSettings 적용
            cinemaBrain.CustomBlends = blenderSettings;
            Debug.Log("BlenderSettings에 정의된 Custom Blend가 적용되었습니다.");
        }
    }


    private void ApplyChangeLook()
    { 
    }

    public static void SetCameraTarget(Transform trackingTarget, Transform lookAtTarget = null, bool useCustomLookAt = true)
    {
        if (Instance.cinemaCamera == null)
        {
            Debug.LogError("CinemachineCamera가 설정되지 않았습니다.");
            return;
        }

        // 현재 카메라 상태 저장
        ICinemachineCamera currentCamera = Instance.cinemaBrain.ActiveVirtualCamera;

        // Follow 및 LookAt 설정
        Instance.cinemaCamera.Follow = trackingTarget;
        if (useCustomLookAt && lookAtTarget != null)
        {
            Instance.cinemaCamera.LookAt = lookAtTarget;
        }
        else
        {
            Instance.cinemaCamera.LookAt = trackingTarget;
        }

        // 카메라 설정
        Instance.cinemaCamera.Lens.FieldOfView = 30f;
        Instance.cinemaOrbitalFollow.VerticalAxis.Value = 0f;
        Instance.cinemaOrbitalFollow.HorizontalAxis.Value = 0f;
        Instance.cinemaCamera.transform.GetComponent<CinemachineRotationComposer>().TargetOffset = new Vector3(-2, 1, 0);

        // 입력 비활성화
        MainCameraInputToggle(false);

        // Blend 추가
        //Instance.ApplyBlend(currentCamera, Instance.cinemaCamera);

        Debug.Log($"카메라 Follow: {trackingTarget.name}, LookAt: {(lookAtTarget != null ? lookAtTarget.name : trackingTarget.name)}로 변경되었습니다.");
    }



    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {

    }
}
