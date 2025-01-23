using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;
    private Button[] buttons;
    private int currentButtonIndex = 0;
    private Dictionary<string, List<Quest>> categorizedQuest = new Dictionary<string, List<Quest>>();

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        AddButtonListeners();
        UpdateUI();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void CategorizeItems()
    {
        categorizedQuest.Clear();

        foreach (var quest in QuestManager.QuestDatabase)
        {
            string questtag = quest.questType.ToString();
            if (!categorizedQuest.ContainsKey(questtag))
            {
                categorizedQuest[questtag] = new List<Quest>();
            }
            categorizedQuest[questtag].Add(quest);
        }
    }

    public void UpdateUI()
    {
        CategorizeItems();
        ClearUI();
        Debug.Log("ClearUI 완료");

        // 기본 검증
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("버튼이 설정되지 않았거나 버튼 배열이 비어있습니다.");
            return;
        }

        if (!System.Enum.IsDefined(typeof(CategotyQuestType), currentButtonIndex))
        {
            Debug.LogError($"유효하지 않은 카테고리 인덱스: {currentButtonIndex}");
            return;
        }

        string selectedTag = ((CategotyQuestType)currentButtonIndex).ToString();

        if (selectedTag == CategotyQuestType.전체퀘스트.ToString())
        {
            Debug.Log("전체퀘스트 선택됨: 모든 퀘스트 출력");
            foreach (var category in categorizedQuest.Values)
            {
                foreach (var quest in category)
                {
                    CreateItemUI(quest);
                }
            }
        }
        else if (selectedTag == CategotyQuestType.완료퀘스트.ToString())
        {
            Debug.Log("완료된 퀘스트 출력");
            foreach (var completedQuest in QuestManager.CompletedQuests)
            {
                CreateItemUI(completedQuest);
            }
        }
        else if (categorizedQuest.ContainsKey(selectedTag))
        {
            Debug.Log($"선택된 태그 '{selectedTag}'에 포함된 퀘스트 개수: {categorizedQuest[selectedTag].Count}");
            foreach (var quest in categorizedQuest[selectedTag])
            {
                CreateItemUI(quest);
            }
        }
        else
        {
            Debug.LogWarning($"선택된 태그 '{selectedTag}'에 해당하는 퀘스트가 없습니다.");
        }

        Debug.Log("UpdateUI 완료");
    }

    private void ClearUI()
    {
        foreach (Transform child in questParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateItemUI(Quest quest)
    {
        var questItem = Instantiate(questPrefab, questParent);
        questItem.GetComponentInChildren<TextMeshProUGUI>().text = quest.ToStringTMProforList();
        Debug.Log($"퀘스트 UI 생성: {quest.ToStringTMPro()}");
    }

    public void AddButtonListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void RemoveButtonListeners()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        currentButtonIndex = buttonIndex;
        UpdateUI();
    }
}

public enum CategotyQuestType
{
    전체퀘스트,
    메인퀘스트,   // 메인스토리 퀘스트
    서브퀘스트,   // 서브 퀘스트
    특수퀘스트,  // 특수 임무 퀘스트
    완료퀘스트    // 완료된 퀘스트
}