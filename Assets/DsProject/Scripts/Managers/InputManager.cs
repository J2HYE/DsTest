using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseManager<InputManager>
{
    public static PlayerInput InputActions;
    protected override void Awake()
    {
        base.Awake();
        InputActions = GetComponent<PlayerInput>();

        #region Delete
        // ui키 연결
        InputActions.actions["Inventory"].performed += OnInventoryKey;
        InputActions.actions["Quest"].performed += OnQuestReview;
        InputActions.actions["StatusUI"].performed += OnStatKey;

        #endregion
    }

    #region Delete
    private void OnInventoryKey(InputAction.CallbackContext context)
    {
        // 상태를 Inventory로 전환
        if (GameStateMachine.Instance.CurrentState != GameSystemState.Inventory)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.Inventory);
            Debug.Log("Inventory 상태로 전환됨.");
        }
        else
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
            Debug.Log("MainMenu 상태로 복귀됨.");
        }
    }
    private void OnQuestReview(InputAction.CallbackContext context)
    {
        // 상태를 Inventory로 전환
        if (GameStateMachine.Instance.CurrentState != GameSystemState.QuestReview)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.QuestReview);
            Debug.Log("Quest 상태로 전환됨.");
        }
        else
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
            Debug.Log("MainMenu 상태로 복귀됨.");
        }
    }
    private void OnStatKey(InputAction.CallbackContext context)
    {
        // 상태를 Inventory로 전환
        if (GameStateMachine.Instance.CurrentState != GameSystemState.StatusUI)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.StatusUI);
            Debug.Log("Quest 상태로 전환됨.");
        }
        else
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
            Debug.Log("MainMenu 상태로 복귀됨.");
        }
    }
    #endregion

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {

    }
}

/* #region 테스트용 추가 메서드 불필요시 삭제

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I키 입력 확인");
            ToggleInventoryState();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleInventoryState2();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleInventoryState3();
        }
    }

    private void ToggleInventoryState()
    {
        var currentState = GameStateMachine.Instance.CurrentState;

        if (currentState != GameSystemState.Inventory)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.Inventory, null);
        }
        else if (currentState == GameSystemState.Inventory)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu, null);
        }
    }
    private void ToggleInventoryState2()
    {
        var currentState = GameStateMachine.Instance.CurrentState;

        if (currentState != GameSystemState.QuestReview)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.QuestReview, null);
        }
        else if (currentState == GameSystemState.QuestReview)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu, null);
        }
    }
    private void ToggleInventoryState3()
    {
        var currentState = GameStateMachine.Instance.CurrentState;

        if (currentState != GameSystemState.Stat)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.Stat, null);
        }
        else if (currentState == GameSystemState.Stat)
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu, null);
        }
    }
    #endregion*/