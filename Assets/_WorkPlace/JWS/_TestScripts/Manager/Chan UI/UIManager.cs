using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField] private GameObject questTextPrefab;
    [SerializeField] private GameObject mainQuestTextPrefab;
    [SerializeField] private GameObject damagePopUpPrefab;
    [SerializeField] private GameObject mainTitleButton;
    [SerializeField] private GameObject baseCanvas;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject dialogWindow;
    [SerializeField] private GameObject questWindow;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject interactTextUI;
    [SerializeField] private GameObject historyLog;
    [SerializeField] private GameObject historyWindow;
    [SerializeField] private Transform questListParent;
    private PickUpItemTextDisplay pickUpItemTextDisplay;
    public GameObject DisplaySpeechWindow => dialogWindow;
    public GameObject MainTitleButton => mainTitleButton;
    public static HistoryManager HistoryManager;

    public PickUpItemTextDisplay PickUpItemTextDisplay => pickUpItemTextDisplay;
    public static InventoryUI InventoryUI;
    public static QuestUI Quest_UI;
    public static MainButtonUI MainButtonUI;
    public static InteractPopupUI InteractPopupUI;
    public static HistoryUI HistoryUI;
    public static HistoryWindowUI HistoryWindowUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        pickUpItemTextDisplay = GetComponent<PickUpItemTextDisplay>();
        InventoryUI = inventoryUI.GetComponent<InventoryUI>();
        Quest_UI = questWindow.GetComponent<QuestUI>();
        MainButtonUI = mainTitleButton.GetComponent<MainButtonUI>();
        InteractPopupUI = interactTextUI.GetComponent<InteractPopupUI>();
        mainCanvas.SetActive(true);
        dialogWindow.SetActive(false);
        questWindow.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
        historyWindow.gameObject.SetActive(false);
        HistoryManager = new HistoryManager();
        HistoryUI = historyLog.GetComponent<HistoryUI>();
        HistoryWindowUI = historyWindow.GetComponent<HistoryWindowUI>();
    }

    private void Update()
    {
        if (IsUIWindowOpen()/* || IsPointerOverUI()*/)
        {
            InputManager.InputActions.actions["Attack"].Disable(); // UI가 열려 있을 때 공격 비활성화
            InputManager.InputActions.actions["Interact"].Disable(); // UI가 열려 있을 때 공격 비활성화
        }
        else
        {
            InputManager.InputActions.actions["Interact"].Enable(); // UI가 열려 있을 때 공격 비활성화
            InputManager.InputActions.actions["Attack"].Enable(); // UI가 닫혀 있을 때 공격 활성화
        }
        InputManager.InputActions.actions["Inventory"].Enable();
        InputManager.InputActions.actions["Quest"].Enable();
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    public void ToggleDialog()
    {
        dialogWindow.SetActive(!dialogWindow.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }

    public void ToggleHistorylog()
    {
        historyWindow.SetActive(!historyWindow.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }

    public void ToggleInventory()
    {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }

    public void ToggleQuestWindow()
    {
        questWindow.gameObject.SetActive(!questWindow.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }

    public void InventoryUpdate()
    {
        if (InventoryUI != null && InventoryUI.gameObject.activeSelf)
        {
            InventoryUI.UpdateUI();
        }
    }

    public void QuestUpdate()
    {
        if (Quest_UI != null && Quest_UI.gameObject.activeSelf)
        {
            Quest_UI.UpdateUI();
        }
        QuestUI();
        InventoryUpdate();
    }

    public void QuestUI()
    {
        if (QuestManager.QuestDatabase.Count > 0 || QuestManager.QuestDatabase.Count != questListParent.transform.childCount)
        {
            foreach (Transform child in questListParent.transform)
            {
                Destroy(child.gameObject);
            }

            bool isMainQuestAdded = false;

            foreach (var quest in QuestManager.QuestDatabase)
            {
                GameObject questObjPrefab;
                if (quest.questType == "메인퀘스트")
                {
                    if (isMainQuestAdded) continue;
                    questObjPrefab = mainQuestTextPrefab;
                    isMainQuestAdded = true;
                }
                else
                {
                    questObjPrefab = questTextPrefab;
                }
                var questObj = Instantiate(questObjPrefab, questListParent);
                questObj.transform.SetSiblingIndex(quest.questType == "메인퀘스트" ? 0 : questListParent.childCount);
                DisplayQuestTextAtPrefab(questObj, quest);
            }
        }
    }

    private void DisplayQuestTextAtPrefab(GameObject gameObj, Quest quest)
    {
        TextMeshProUGUI[] subDisplay = gameObj.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        string text = quest.description;
        if (text.Length > 20)
        {
            text = text.Substring(0, 20) + "...";
        }
        subDisplay[0].text = text;
        subDisplay[2].text = quest.ToStringTMPro();
        if (quest.isCompleted) { subDisplay[3].gameObject.SetActive(true); }
    }

    public void DisplayDialogWindow(NPCData nPCData)
    {
        if (dialogWindow == null) return;
        ToggleDialog();
    }

    public void DisplayQuestDialogWindow(string title, Quest quest)
    {
        if (dialogWindow == null)
        {
            return;
        }

        dialogWindow.SetActive(true);
        mainCanvas.SetActive(false);

        TextMeshProUGUI[] subDisplay = dialogWindow.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        Button acceptButton = dialogWindow.GetComponentInChildren<Button>(includeInactive: true);

        if (subDisplay.Length < 2)
        {
            Debug.LogError("텍스트 박스가 없습니다.");
            return;
        }

        acceptButton.onClick.RemoveAllListeners();
        subDisplay[0].text = title;

        // Check if it is a completed main quest
        bool isCompletedMainQuest = quest.questType == "메인퀘스트" && QuestManager.CompletedQuests.Exists(q => q.id == quest.id);

        if (isCompletedMainQuest)
        {
            SetupDialog(
                acceptButton,
                subDisplay[1],
                "닫기",
                "완료된 메인 퀘스트입니다. 더 이상 진행할 수 없습니다.",
                ToggleDialog
            );
        }
        else if (quest.isCompleted)
        {
            SetupDialog(
                acceptButton,
                subDisplay[1],
                "보상수령",
                "감사합니다! 퀘스트 완료에 따른 보상을 지급하겠습니다.",
                () => HandleQuest(quest, true)
            );
        }
        else
        {
            bool isQuestInProgress = QuestManager.QuestDatabase.Contains(quest);
            bool canAccept = !isQuestInProgress && quest.acceptCount < 3;

            SetupDialog(
                acceptButton,
                subDisplay[1],
                canAccept ? "수락" : "닫기",
                canAccept ? quest.description : "퀘스트를 완료하고 다시 찾아주세요.",
                () => HandleQuest(quest, canAccept)
            );
        }
        Debug.Log("Quest dialog setup complete.");
    }

    private void SetupDialog(Button button, TextMeshProUGUI display, string buttonText, string message, UnityEngine.Events.UnityAction onClickAction)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button.onClick.AddListener(onClickAction);
        StartCoroutine(AnimateText(button, display, message));
    }

    private IEnumerator AnimateText(Button button, TextMeshProUGUI display, string message, float delay = 0.05f)
    {
        button.gameObject.SetActive(false);
        display.text = "";
        foreach (char c in message)
        {
            string key = c.ToString().ToUpper();
            display.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(0.5f);
        button.gameObject.SetActive(true);
    }

    private void HandleQuest(Quest quest, bool state)
    {
        if (state)
        {
            if (!quest.isCompleted)
            {
                QuestManager.Instance.AddQuest(quest);
            }
            else
            {
                QuestManager.Instance.CompleteQuest(quest);
            }
        }
        dialogWindow.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void DisplayHistory()
    {
        if (historyWindow.activeSelf) HistoryWindowUI.DisplayHistory();
        else HistoryUI.DisplayHistory();
    }

    public static void SystemMessage(string msg)
    {
        HistoryManager.AddHistory(msg, MessageTag.시스템_알림, 1);
    }

    public static void SystemGameMessage(string msg, MessageTag tag)
    {
        HistoryManager.AddHistory(msg, tag, 1);
    }

    public void UIClose()
    {
        EventSystem.current.SetSelectedGameObject(null);
        mainCanvas.SetActive(true);
        dialogWindow.SetActive(false);
        questWindow.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
        historyWindow.gameObject.SetActive(false);
    }

    public void InteractTextPopup(string keyname, string comment, bool isOn)
    {
        InteractPopupUI.UpdateInteractText(keyname, comment, isOn);
    }

    public static void DisplayPopupText(string text, Vector3 targetPosition, MessageTag msgTag)
    {
        Vector3 finalPosition = targetPosition + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.5f), Random.Range(-0.3f, 0.3f));
        var go = Instantiate(Instance.damagePopUpPrefab, finalPosition, Quaternion.identity);
        TextMeshProUGUI displayText = go.GetComponentInChildren<TextMeshProUGUI>();
        displayText.text = text;

        Debug.Log("출력위치 : " + targetPosition);
        if (Instance.tagColors.TryGetValue(msgTag, out string colorCode))
        {
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
            {
                displayText.color = color;
            }
        }
    }

    public void UIWindowClose()
    {
        InputManager.InputActions.enabled = !IsUIWindowOpen();
    }

    public bool IsUIWindowOpen()
    {
        return dialogWindow.activeSelf || questWindow.activeSelf || inventoryUI.gameObject.activeSelf || historyWindow.gameObject.activeSelf;
    }

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        UIClose();
        #region Delete

        switch (newState)
        {
            case GameSystemState.MainMenu:
                UIClose();
                break;
            case GameSystemState.Inventory:

                ToggleInventory();
                break;
            case GameSystemState.QuestReview:

                ToggleQuestWindow();
                break;
        }
        #endregion
    }

    private readonly Dictionary<MessageTag, string> tagColors = new Dictionary<MessageTag, string>
    {
        { MessageTag.퀘스트, "#FFD700" },          // Gold
        { MessageTag.플레이어_피해, "#FF0000" },   // Red
        { MessageTag.적_피해, "#00FF00" },         // Green
        { MessageTag.기타, "#1E90FF" },            // Blue
        { MessageTag.플레이어_회복, "#32CD32" },   // Lime Green
        { MessageTag.플레이어_버프, "#87CEEB" },   // Sky Blue
        { MessageTag.플레이어_디버프, "#8B0000" }, // Dark Red
        { MessageTag.치명타, "#FF4500" },          // Orange Red
        { MessageTag.빗나감, "#708090" },          // Slate Gray
        { MessageTag.회피, "#00CED1" },            // Dark Turquoise
        { MessageTag.적_등장, "#8A2BE2" },         // Purple
        { MessageTag.적_처치, "#FFDAB9" },         // Peach
        { MessageTag.경고, "#FFA500" },            // Orange
        { MessageTag.오류, "#DC143C" },            // Crimson
        { MessageTag.시스템_알림, "#ADD8E6" },     // Light Blue
        { MessageTag.이벤트, "#FFC0CB" },          // Pink
        { MessageTag.아이템_획득, "#FFD700" },     // Gold
        { MessageTag.희귀_아이템, "#9400D3" },     // Dark Violet
        { MessageTag.금화_획득, "#FFFF00" },       // Yellow
        { MessageTag.아군_회복, "#00FA9A" },       // Medium Spring Green
        { MessageTag.아군_피해, "#FF6347" },       // Tomato Red
        { MessageTag.팀_버프, "#4169E1" },         // Royal Blue
        { MessageTag.스킬_사용, "#6495ED" },       // Cornflower Blue
        { MessageTag.마법_사용, "#9932CC" }        // Dark Orchid
    };

    public string GetColorByTag(MessageTag tag)
    {
        if (tagColors.TryGetValue(tag, out string color))
        {
            return color;
        }
        return "#FFFFFF"; // 기본 흰색
    }
}
