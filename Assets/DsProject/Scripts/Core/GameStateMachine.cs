using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static GameStateMachine Instance { get; private set; }

    [SerializeField]
    private GameSystemState currentState = GameSystemState.MainMenu; // 인스펙터에서 초기 상태 설정 가능 기본은 메인 메뉴
    public GameSystemState CurrentState
    {
        get => currentState;
        private set => currentState = value;
    }
    
    public static event Action<GameSystemState, object> OnGameStateChanged;

    private void Awake()
    {
        // 싱글턴 패턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 상태 유지
            
            // 게임 시작 상태 전달
            OnGameStateChanged?.Invoke(currentState, null);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 상태 변경 메서드
    public void ChangeState(GameSystemState newState, object additionalData = null, bool forceTrigger = false)
    {
        if (!GameManager.Instance.CanChangeState())
        {
            Debug.LogWarning($"GameManager: 모든 매니저가 준비되지 않아 상태 변경 {newState}를 무시합니다.");
            return;
        }
        
        if (!forceTrigger && CurrentState == newState)
        {
            Debug.Log($"이미 상태가 {newState} 입니다.");
            return;
        }
        
        Debug.Log($"게임 상태 변경: {CurrentState} → {newState}");
        CurrentState = newState;

        // 상태 변경 이벤트 발행
        OnGameStateChanged?.Invoke(newState, additionalData);
    }
}