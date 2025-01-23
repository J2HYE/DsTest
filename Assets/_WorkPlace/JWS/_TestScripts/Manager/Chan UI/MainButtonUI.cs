using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonUI : MonoBehaviour
{
    private Button[] mainButtons;
    private TextMeshProUGUI playerGold;
    private enum MainButtonType { Main, StatusUI, QuestUI, Exit }
    private MainButtonType clickedButtonType = MainButtonType.Main;

    private void Awake()
    {
        mainButtons = transform.GetComponentsInChildren<Button>();
        playerGold = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void Update()
    {
        playerGold.text = CharacterManager.PlayerCharacterData.gold.ToString();
    }

    public void AddButtonListeners()
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            int index = i;
            mainButtons[i].onClick.RemoveAllListeners();
            mainButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void RemoveButtonListeners()
    {
        foreach (var button in mainButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        clickedButtonType = (MainButtonType)buttonIndex;
        switch(clickedButtonType)
        {
            case MainButtonType.Main:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;
            case MainButtonType.StatusUI:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;
            case MainButtonType.QuestUI:
                GameStateMachine.Instance.ChangeState(GameSystemState.QuestReview);
                break;
            case MainButtonType.Exit:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;
            default:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;
        }
    }
}
