using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private int totalManagers = 10;
    private HashSet<string> readyManagers = new HashSet<string>();
    private bool allManagersReady = false;
    
    public static Transform playerTransform;
    public static Transform DragonTransform;
    
    public static event Action OnAllManagersReadyEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Debug.Log($"GameManager 초기화 완료. 총 필요 매니저 수: {totalManagers}");
    }
    
    // 각 매니저가 준비 상태를 등록할 때 필요한 메서드
    public void RegisterManager(string managerName)
    {
        if (!readyManagers.Contains(managerName))
        {
            readyManagers.Add(managerName);
            Debug.Log($"{managerName} 준비 완료 ({readyManagers.Count}/{totalManagers})");

            if (readyManagers.Count == totalManagers)
            {
                OnAllManagersReady();
            }
        }
    }
    
    //모든 매니저가 준비되었을 때 호출되는 후속 작업
    private void OnAllManagersReady()
    {
        Debug.Log("모든 매니저가 준비 완료되었습니다. 게임 시작!");
        allManagersReady = true;
        OnAllManagersReadyEvent?.Invoke();
        //GameStateMachine.Instance.ChangeState(GameSystemState.MainQuestPlay);
    }
    
    public bool CanChangeState()
    {
        return allManagersReady;
    }


    ////////////////////////////////////////////////////////////
    //  플레이어 숨기기 위한 코드 추가
    /// JWS 2025.01.28 10:00 수정
    ////////////////////////////////////////////////////////////
    public static bool PlayerVisible(bool active)
    {
        if (playerTransform == null) return false;
        playerTransform.GetChild(0).GetChild(1).gameObject.SetActive(active);
        return true;
    }
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////

}