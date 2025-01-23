using UnityEngine;

    public enum CameraType
    {
        Main,
        Character,
        LookCharacter,
        Follow,
        TopDown,
    }

    [System.Serializable]
    public class CameraPose
    {
        public Vector3 position;     // 위치 오프셋
        public Vector3 rotation;     // 회전 오프셋 (Euler angles)

        public CameraPose(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

public class CameraManagers : MonoBehaviour
{
    public static CameraManagers Instance { get; private set; }
    public static Camera MainCamera => Instance.mainCamera;
    private Camera mainCamera;
    public CameraPose[] cameraPoses;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCamera = Camera.main;
        InitCameraPoses();
    }

    private void InitCameraPoses()
    {
        // CameraType에 따른 CameraPose 초기화
        cameraPoses = new CameraPose[System.Enum.GetValues(typeof(CameraType)).Length];

        cameraPoses[(int)CameraType.Main] = new CameraPose(
            new Vector3(0, 3, 0.6f),
            Vector3.zero
        );

        cameraPoses[(int)CameraType.Character] = new CameraPose(
            new Vector3(0, 4.5f, -8),
            new Vector3(10, 0, 0)
        );

        cameraPoses[(int)CameraType.LookCharacter] = new CameraPose(
            new Vector3(0, 2.1f, 4.2f),
            new Vector3(10, 180, 0)
        );

        cameraPoses[(int)CameraType.Follow] = new CameraPose(
            new Vector3(0, 9, -10),
            new Vector3(30, 0, 0)
        );

        cameraPoses[(int)CameraType.TopDown] = new CameraPose(
            new Vector3(0, 15, 0),
            new Vector3(90, 0, 0)
        );
    }

    public static void SetCameraActive(Transform target, CameraType type)
    {
        if (Instance == null || Instance.mainCamera == null) return;

        int index = (int)type;
        CameraPose pose = Instance.cameraPoses[index];
        Instance.mainCamera.transform.position = target.position + target.rotation * pose.position;
        Instance.mainCamera.transform.rotation = target.rotation * Quaternion.Euler(pose.rotation);
    }

}
