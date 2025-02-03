using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogType
{
    StorySequenceDialog,
    MainQuestDialog,
    NormalDialog,
    SceneDialog,
}

public class DialogUI : MonoBehaviour
{
    private TextMeshProUGUI[] subDisplay;
    private Button acceptButton;

    public void Awake()
    {
        subDisplay = GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
        acceptButton = GetComponentInChildren<Button>(includeInactive: true);
    }


    public void DisplayDialogText(List<string> textList, DialogType dType)
    {
        subDisplay[0].text = textList[0];
        UIManager.Instance.ToggleDialog();
        SetupDialog("스페이스", textList[1], ()=>HandleDialog(dType));
    }

    public void DisplayDialogWindow(NPCData nPCData)
    {

    }

    public void DisplayQuestDialogWindow(string title, Quest quest)
    {
        if (subDisplay.Length < 2)
        {
            Debug.LogError("텍스트 박스가 없습니다.");
            return;
        }

        acceptButton.onClick.RemoveAllListeners();
        subDisplay[0].text = title;

        if (quest.isCompleted)
        {
            SetupDialog(
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
                canAccept ? "수락" : "닫기",
                canAccept ? quest.description : "퀘스트를 완료하고 다시 찾아주세요.",
                () => HandleQuest(quest, canAccept)
            );
        }
    }

    private void SetupDialog(string buttonText, string message, UnityEngine.Events.UnityAction onClickAction)
    {
        acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        acceptButton.onClick.AddListener(onClickAction);
        StartCoroutine(AnimateText(message));
    }

    private IEnumerator AnimateText(string message, float delay = 0.05f)
    {
        acceptButton.gameObject.SetActive(false);
        subDisplay[1].text = "";
        foreach (char c in message)
        {
            string key = c.ToString().ToUpper();
            subDisplay[1].text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(0.5f);
        acceptButton.gameObject.SetActive(true);
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
        UIManager.Instance.ToggleDialog();
    }

    private void HandleDialog(DialogType dType)
    {
        switch (dType)
        {
            case DialogType.NormalDialog:
                UIManager.Instance.ToggleDialog();
                break;
            case DialogType.SceneDialog:
                UIManager.Instance.ToggleDialog();
                break;
            case DialogType.MainQuestDialog:
                break;
            case DialogType.StorySequenceDialog:
                break;
        }

    }
}
