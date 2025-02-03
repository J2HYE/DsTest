using UnityEngine;
using System.Collections.Generic;

public class QuestManager : BaseManager<QuestManager>
{
    [SerializeField] private NPCList npcDataList;
    // [SerializeField] private QuestList questList;
    [SerializeField] private List<NPCData> npcDatabase = new List<NPCData>();
    [SerializeField] private List<Quest> questDatabase = new List<Quest>();
    [SerializeField] private List<Quest> mainQuestDatabase = new List<Quest>();
    [SerializeField] private List<Quest> subQuestDatabase = new List<Quest>();
    [SerializeField] private List<Quest> completedQuests = new List<Quest>();
    [SerializeField] private Dictionary<string, Transform> questConditionPoint = new Dictionary<string, Transform>();

    public static List<Quest> QuestDatabase => Instance.questDatabase;
    public static List<Quest> CompletedQuests => Instance.completedQuests;
    public static Dictionary<string, Transform> QuestConditionPoint => Instance.questConditionPoint;

    public static Transform GetQuestConditionPoint(string point) => Instance.questConditionPoint[point];

    public static NPCList NpcDatabase => Instance.npcDataList;

    private int currentMainQuestIndex = 0;
    public static int CurrentMainQuestIndex => Instance.currentMainQuestIndex;


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        GenerateData generater = new GenerateData();
        mainQuestDatabase = generater.GenerateMainQuestLists();
        subQuestDatabase = generater.GenerateQuestLists();
        generater.GenerateRandomNPCs(20, subQuestDatabase, ItemManager.ItemDatabase, subQuestDatabase, npcDataList);
        npcDatabase = npcDataList.npcLists;
        GameStateMachine.Instance.ChangeState(GameSystemState.MainQuestPlay);
    }

    public Quest GiveQuests()
    {
        Quest quest = GenerateQuests();
        return quest;
    }

    public void AssignQuestToNPC(string questId, string npcId)
    {
        var quest = questDatabase.Find(q => q.id == questId);
        var npc = npcDatabase.Find(n => n.id == npcId.ToString());
        if (quest != null && npc != null)
        {
            quest.targetID = npcId;
        }
    }

    public void AddQuest(Quest quest)
    {
        if (questDatabase.Exists(q => q.questType == "메인퀘스트"))
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, $"이미 진행 중인 메인 퀘스트가 있습니다.");
            return;
        }
        if (questDatabase.Exists(q => q.id == quest.id))
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, $"퀘스트 '{quest.id}'는 이미 등록되어 있습니다.");
            return;
        }
        if (completedQuests.Exists(q => q.id == quest.id) && quest.questType == "메인퀘스트")
        {
            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, $"완료된 메인 퀘스트 '{quest.id}'는 추가할 수 없습니다.");
            return;
        }
        questDatabase.Add(quest);
        UIManager.Instance.QuestUpdate();
    }

    public void RemoveQuest(string questId)
    {
        Quest quest = questDatabase.Find(q => q.id == questId);
        if (quest != null)
        {
            questDatabase.Remove(quest);
            UIManager.SystemGameMessage($"퀘스트 '{quest.name}' 삭제됨", MessageTag.아이템_획득);
        }
        else
        {
            Debug.LogWarning($"[QuestManager] ID: {questId} 퀘스트를 찾을 수 없습니다.");
        }
    }

    public Quest GetQuestById(string questId)
    {
        Quest quest = questDatabase.Find(q => q.id == questId);
        if (quest == null)
        {
            Debug.LogWarning($"[QuestManager] ID: {questId} 퀘스트를 찾을 수 없습니다.");
        }
        return quest;
    }

    //TODO
    //모험, 처치, 만남떄 이곳을 호출하는부분 추가해줘야함.
    public void UpdateQuestProgress(QuestConditionType conditionType, string targetId, int quantity = 1)
    {
        for (int i = questDatabase.Count - 1; i >= 0; i--)
        {
            if (i < 0 || i >= questDatabase.Count) continue;
            Quest quest = questDatabase[i];
            if (quest.isCompleted) continue;
            foreach (var conditionKeyValue in quest.requiredConditions)
            {
                var conditionId = conditionKeyValue.Key;
                var condition = conditionKeyValue.Value;
                if (condition.type == conditionType && condition.targetId == targetId)
                {
                    if (!quest.progress.ContainsKey(conditionId))
                    {
                        quest.progress[conditionId] = 0;
                    }
                    quest.progress[conditionId] += quantity;
                    if (quest.progress[conditionId] >= condition.requiredQuantity)
                    {
                        quest.progress[conditionId] = condition.requiredQuantity;
                        condition.isCompleted = true;
                        UIManager.SystemGameMessage($"퀘스트 조건 '{condition.targetName}' 완료!", MessageTag.아이템_획득);
                    }
                }
            }
            if (IsQuestCompleted(quest))
            {
                quest.isCompleted = true;
                UIManager.SystemGameMessage($"퀘스트 '{quest.name}' 완료!", MessageTag.아이템_획득);
                UIManager.Instance.QuestUpdate();
            }
        }
    }

    private bool IsQuestCompleted(Quest quest)
    {
        foreach (var condition in quest.requiredConditions)
        {
            string conditionId = condition.Key;
            QuestCondition questCondition = condition.Value;

            if (!quest.progress.ContainsKey(conditionId) || quest.progress[conditionId] < questCondition.requiredQuantity)
            {
                return false;
            }
        }
        return true;
    }

    public void CompleteQuest(Quest quest)
    {
        if (questDatabase.Contains(quest))
        {
            questDatabase.Remove(quest);
        }
        completedQuests.Add(quest);

        foreach (var condition in quest.requiredConditions)
        {
            switch (condition.Value.type)
            {
                case QuestConditionType.Collect:
                    InventoryManager.Instance.RemoveItemLogic(condition.Key, condition.Value.requiredQuantity);
                    break;

                case QuestConditionType.Kill:
                    UIManager.SystemGameMessage($"처치 조건 '{condition.Value.targetName}' 완료 처리.", MessageTag.퀘스트);
                    break;

                case QuestConditionType.Explore:
                    UIManager.SystemGameMessage($"탐험 조건 '{condition.Value.targetName}' 완료 처리.", MessageTag.퀘스트);
                    break;

                case QuestConditionType.Meet:
                    UIManager.SystemGameMessage($"만남 조건 '{condition.Value.targetName}' 완료 처리.", MessageTag.퀘스트);
                    break;

                default:
                    Debug.LogWarning($"[QuestManager] 알 수 없는 퀘스트 조건: {condition.Value.type}");
                    break;
            }
        }

        foreach (Reward reward in quest.rewards)
        {
            if (!string.IsNullOrEmpty(reward.itemId))
            {
                ItemManager.Instance.AddItemLogic(reward.itemId, reward.quantity);
                UIManager.SystemGameMessage($"보상 아이템 '{reward.itemId}' {reward.quantity}개 지급됨.", MessageTag.아이템_획득);
            }

            if (reward.experience > 0)
            {
                CharacterManager.PlayerCharacterData.AddExperience(reward.experience);
                UIManager.SystemGameMessage($"경험치 {reward.experience} 지급됨.", MessageTag.퀘스트);
            }

            if (reward.gold > 0)
            {
                CharacterManager.PlayerCharacterData.AddGold(reward.gold);
                UIManager.SystemGameMessage($"골드 {reward.gold} 지급됨.", MessageTag.금화_획득);
            }
        }
        if (quest.questType == "메인퀘스트" && mainQuestDatabase.Count > currentMainQuestIndex)
        {
            currentMainQuestIndex++;
            GameStateMachine.Instance.ChangeState(GameSystemState.MainQuestPlay);
        }
        if (quest.questType != "메인퀘스트")
        {
            quest.isCompleted = false;
        }
        UIManager.SystemGameMessage($"퀘스트 '{quest.name}' 보상이 지급되었습니다.", MessageTag.퀘스트);
        UIManager.Instance.QuestUpdate();
    }

    public Quest GenerateQuests()
    {
        return subQuestDatabase[Random.Range(0, subQuestDatabase.Count)];
    }

    private void MainQuestSequenceStart(int index)
    {
        if (index == 0 || (index > 0 && mainQuestDatabase[index-1].isCompleted))
        {
            mainQuestDatabase[index].questGiver = "메인퀘스터";
            AddQuest(mainQuestDatabase[index]);
            currentMainQuestIndex = index;
        }
    }

    public static string GetDistanceColor(float distance)
    {
        if (distance < 60) return "green";
        if (distance < 100) return "yellow";
        return "red";
    }

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        switch(newState)
        {
            case GameSystemState.MainQuestPlay:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                MainQuestSequenceStart(currentMainQuestIndex);
                break;
        }
    }
}