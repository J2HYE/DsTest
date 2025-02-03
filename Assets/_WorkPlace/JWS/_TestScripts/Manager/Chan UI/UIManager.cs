using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject interactTextUI;
    [SerializeField] private GameObject historyLog;
    [SerializeField] private GameObject historyWindow;
    [SerializeField] private GameObject infoMessageWindow;
    [SerializeField] private GameObject InventorytooltipWindow;
    [SerializeField] private GameObject characterStaus;
    [SerializeField] private GameObject quickSlot;
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
    public static ShopUI ShopUI;
    public static DialogUI dialogUI;
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
        mainTitleButton.SetActive(true);
        dialogWindow.SetActive(false);
        questWindow.SetActive(false);
        shopUI.SetActive(false);
        infoMessageWindow.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
        historyWindow.gameObject.SetActive(false);
        InventorytooltipWindow.SetActive(false);
        characterStaus.SetActive(false);
        HistoryManager = new HistoryManager();
        HistoryUI = historyLog.GetComponent<HistoryUI>();
        HistoryWindowUI = historyWindow.GetComponent<HistoryWindowUI>();
        ShopUI = shopUI.GetComponent<ShopUI>();
        dialogUI = dialogWindow.GetComponent<DialogUI>();
    }

    private void Update()
    {
        CheckInputStat();
    }

    private void CheckInputStat()
    {
        // if (IsUIWindowOpen() || IsPointerOverUI())
        if (IsUIWindowOpen())
        {
            InputManager.InputActions.actions["Interact"].Disable();
        }
        else
        {
            InputManager.InputActions.actions["Interact"].Enable();
            InputManager.InputActions.actions["Move"].Enable();
        }
        if (infoMessageWindow.activeSelf && Input.GetMouseButtonDown(1))
        {
            TogglinfoMessageWindow("");
            GameStateMachine.Instance.ChangeState(GameSystemState.Play);
        }
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

    public void ToggleDialog( bool isOn = false)
    {
        if (isOn)
        {
            dialogWindow.SetActive(true);
            mainCanvas.SetActive(false);
            quickSlot.SetActive(false);
        }
        else
        {
            dialogWindow.SetActive(!dialogWindow.activeSelf);
            mainCanvas.SetActive(!mainCanvas.activeSelf);
            quickSlot.SetActive(mainCanvas.activeSelf ? true : false);
        }
    }

    public void ToggleHistorylog()
    {
        historyWindow.SetActive(!historyWindow.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        quickSlot.SetActive(true);
    }

    public void ToggleInventory()
    {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        quickSlot.SetActive(true);
    }

    public void ToggleQuestWindow()
    {
        questWindow.gameObject.SetActive(!questWindow.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        quickSlot.SetActive(mainCanvas.activeSelf ? true : false);
    }

    public void ToggleStatusWindow()
    {
        characterStaus.gameObject.SetActive(!characterStaus.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        quickSlot.SetActive(mainCanvas.activeSelf ? true : false);
    }

    public void TogglShopWindow(NPCData nPCData)
    {
        shopUI.gameObject.SetActive(!shopUI.gameObject.activeSelf);
        mainCanvas.SetActive(!mainCanvas.activeSelf);
        quickSlot.SetActive(false);
        if (shopUI.gameObject.activeSelf) ShopUI.SetShopInfo(nPCData);
    }

    public void TogglinfoMessageWindow(string message)
    {
        infoMessageWindow.gameObject.SetActive(!infoMessageWindow.gameObject.activeSelf);
        if (infoMessageWindow.gameObject.activeSelf)
        {
            infoMessageWindow.transform.GetComponentInChildren<TextMeshProUGUI>().text = message;
        }
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
            List<Quest> completQuest = new List<Quest>();
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
                if (quest.isCompleted) completQuest.Add(quest);
            }

            foreach (var quest in completQuest)
            {
                if (quest.questType == "메인퀘스트")
                {
                    QuestManager.Instance.CompleteQuest(quest);
                }
            }
        }
    }

    private void DisplayQuestTextAtPrefab(GameObject gameObj, Quest quest)
    {
        QuestDistanceCheck distanceCheck = gameObj.GetComponent<QuestDistanceCheck>();
        TextMeshProUGUI[] subDisplay = gameObj.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        if (distanceCheck == null)
        {
            distanceCheck = gameObj.AddComponent<QuestDistanceCheck>();
        }
        distanceCheck.SetQuest(quest);
        if (subDisplay.Length > 0)
        {
            distanceCheck.SetUIText(subDisplay[0]);
        }
        if (quest.isCompleted && quest.questType != "메인퀘스트")
        {
            if (subDisplay.Length > 3)
                subDisplay[3].gameObject.SetActive(true);
        }
        Canvas.ForceUpdateCanvases();
    }

    public void DisplayDialogText(List<string> commentList, DialogType type)
    {
        if (dialogWindow == null) return;
        ToggleDialog();
        dialogUI.DisplayDialogText(commentList, type);
    }

    public void DisplayDialogWindow(NPCData nPCData)
    {
        if (dialogWindow == null) return;
        ToggleDialog();
        dialogUI.DisplayDialogWindow(nPCData);
    }

    public void DisplayQuestDialogWindow(string title, Quest quest)
    {
        if (dialogWindow == null) return;
        ToggleDialog();
        dialogUI.DisplayQuestDialogWindow(title, quest);
    }

    //public void DisplayQuestDialogWindow(string title, Quest quest)
    //{
    //    if (dialogWindow == null)
    //    {
    //        return;
    //    }

    //    dialogWindow.SetActive(true);
    //    mainCanvas.SetActive(false);

    //    TextMeshProUGUI[] subDisplay = dialogWindow.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
    //    Button acceptButton = dialogWindow.GetComponentInChildren<Button>(includeInactive: true);

    //    if (subDisplay.Length < 2)
    //    {
    //        Debug.LogError("텍스트 박스가 없습니다.");
    //        return;
    //    }

    //    acceptButton.onClick.RemoveAllListeners();
    //    subDisplay[0].text = title;

    //    if (quest.isCompleted)
    //    {
    //        SetupDialog(
    //            acceptButton,
    //            subDisplay[1],
    //            "보상수령",
    //            "감사합니다! 퀘스트 완료에 따른 보상을 지급하겠습니다.",
    //            () => HandleQuest(quest, true)
    //        );
    //    }
    //    else
    //    {
    //        bool isQuestInProgress = QuestManager.QuestDatabase.Contains(quest);
    //        bool canAccept = !isQuestInProgress && quest.acceptCount < 3;

    //        SetupDialog(
    //            acceptButton,
    //            subDisplay[1],
    //            canAccept ? "수락" : "닫기",
    //            canAccept ? quest.description : "퀘스트를 완료하고 다시 찾아주세요.",
    //            () => HandleQuest(quest, canAccept)
    //        );
    //    }
    //}

    //private void SetupDialog(Button button, TextMeshProUGUI display, string buttonText, string message, UnityEngine.Events.UnityAction onClickAction)
    //{
    //    button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
    //    button.onClick.AddListener(onClickAction);
    //    StartCoroutine(AnimateText(button, display, message));
    //}

    //private IEnumerator AnimateText(Button button, TextMeshProUGUI display, string message, float delay = 0.05f)
    //{
    //    button.gameObject.SetActive(false);
    //    display.text = "";
    //    foreach (char c in message)
    //    {
    //        string key = c.ToString().ToUpper();
    //        display.text += c;
    //        yield return new WaitForSeconds(delay);
    //    }
    //    yield return new WaitForSeconds(0.5f);
    //    button.gameObject.SetActive(true);
    //}

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
        quickSlot.SetActive(true);
        dialogWindow.SetActive(false);
        questWindow.SetActive(false);
        characterStaus.SetActive(false);
        shopUI.SetActive(false);
        infoMessageWindow.SetActive(false);
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
        return shopUI.activeSelf || characterStaus.activeSelf || dialogWindow.activeSelf || questWindow.activeSelf || 
            inventoryUI.gameObject.activeSelf || historyWindow.gameObject.activeSelf || infoMessageWindow.gameObject.activeSelf;
    }

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        if (newState != GameSystemState.InfoMessage) UIClose();
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
            case GameSystemState.StatusUI:
                ToggleStatusWindow();
                break;
            case GameSystemState.Shopping:
                NPCData npcData = additionalData as NPCData;
                TogglShopWindow(npcData);
                break;
            case GameSystemState.InfoMessage:
                string message = additionalData as string;
                TogglinfoMessageWindow(message);
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
