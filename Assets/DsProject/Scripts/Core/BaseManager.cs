using UnityEngine;

public abstract class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
{
    public static T Instance { get; private set; }
    
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogError($"{typeof(T).Name} 싱글톤 인스턴스가 이미 존재합니다.");
            Destroy(gameObject);
        }
    }
    
    protected virtual void Start()
    {
        RegisterManager();
    }
    
    // 매니저를 게임 매니저에 등록
    private void RegisterManager()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterManager(typeof(T).Name);
        }
    }
   
    protected virtual void OnEnable()
    {
        GameManager.OnAllManagersReadyEvent += OnAllManagersReady;
        GameStateMachine.OnGameStateChanged += HandleGameStateChange;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnAllManagersReadyEvent -= OnAllManagersReady;
        GameStateMachine.OnGameStateChanged -= HandleGameStateChange;
    }
    
    protected virtual void OnAllManagersReady()
    {
        // 필요한 매니저에서만 구현
    }

    protected abstract void HandleGameStateChange(GameSystemState newState, object additionalData);
}