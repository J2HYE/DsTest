using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[Serializable]
public class Quest
{
    [Header ("퀘스트 기본정보")]
    public string id;
    public string questType;
    public string name;
    public string description;
    public string npcid;
    public int acceptCount;
    [Header("퀘스트 상태정보")]
    public bool isCompleted;
    public Dictionary<string, QuestCondition> requiredConditions; // 퀘스트 조건
    public Dictionary<string, int> progress; // 진행 상태
    [Header("퀘스트 보상정보")]
    public List<Reward> rewards; // 보상

    public Quest(string type, string id, string name, string description, Dictionary<string, QuestCondition> requiredConditions, List<Reward> rewards)
    {
        this.id = id;
        this.questType = type;
        this.name = name;
        this.description = description;
        this.isCompleted = false;
        this.requiredConditions = requiredConditions;
        this.progress = new Dictionary<string, int>();
        this.rewards = rewards;
        this.acceptCount = 0;
    }

    public string ToStringTMPro()
    {
        StringBuilder detailedText = new StringBuilder();
        detailedText.AppendLine($"<b><color=#00FF00>{name}</color></b>");
        detailedText.AppendLine($"<b><color=#FFD700>{description}</color></b>");
        detailedText.AppendLine($"");
        detailedText.AppendLine($"<color=yellow>진행 상황:</color>");
        foreach (var condition in requiredConditions)
        {
            int currentProgress = progress.ContainsKey(condition.Key) ? progress[condition.Key] : 0;
            detailedText.AppendLine($"  <color=#00FF00>{condition.Key}</color>: {currentProgress}/{condition.Value.requiredQuantity}");
        }
        detailedText.AppendLine($"");
        detailedText.AppendLine("<color=green>보 상:</color>");
        foreach (var reward in rewards)
        {
            if (!string.IsNullOrEmpty(reward.itemId))
            {
                detailedText.AppendLine($"  아이템: <color=#87CEEB>{reward.itemId}</color> x{reward.quantity}");
            }
            if (reward.experience > 0)
            {
                detailedText.AppendLine($"  경험치: <color=#FFD700>{reward.experience}</color>");
            }
            if (reward.gold > 0)
            {
                detailedText.AppendLine($"  골  드: <color=#FFD700>{reward.gold}</color>");
            }
        }

        return detailedText.ToString();
    }

    public string ToStringTMProforList()
    {
        StringBuilder listText = new StringBuilder();
        listText.AppendLine($"<b><color=#00FF00>{name}</color></b>");
        string truncatedDescription = description.Length > 20
            ? description.Substring(0, 20) + "..."
            : description;
        if (requiredConditions.Count > 0)
        {
            var firstCondition = requiredConditions.First();
            int currentProgress = progress.ContainsKey(firstCondition.Key) ? progress[firstCondition.Key] : 0;
            listText.AppendLine($"<i><color=#FFFFFF>{truncatedDescription}</color></i> <color=#FFFF00>({currentProgress}/{firstCondition.Value.requiredQuantity})</color>");
        }
        else
        {
            listText.AppendLine($"<i><color=#FFFFFF>{truncatedDescription}</color></i> <color=#FF6347>(진행 조건 없음)</color>");
        }
        return listText.ToString();
    }

}

[Serializable]
public class Reward
{
    public string itemId; // 보상 아이템 ID
    public int quantity; // 아이템 개수
    public int experience; // 경험치 보상
    public int gold; // 골드 보상
    public bool isReward;

    public Reward(string itemId, int quantity, int experience, int gold)
    {
        this.itemId = itemId;
        this.quantity = quantity;
        this.experience = experience;
        this.gold = gold;
        this.isReward = false;
    }
}

[Serializable]
public class QuestCondition
{
    [Header("퀘스트 상태정보")]
    public QuestConditionType type; // 조건 유형
    public string targetId;         // 목표 ID (예: 몬스터 ID, 지역 ID, NPC ID)
    public string targetName;       // 목표 이름 (예: 몬스터 이름, 지역 이름, NPC 이름)
    public int requiredQuantity;    // 요구 수량 (예: 처치 수, 수집 수)
    public bool isCompleted;        // 해당 조건 완료 여부

    public QuestCondition(QuestConditionType type, string targetId, string targetName, int requiredQuantity)
    {
        this.type = type;
        this.targetId = targetId;
        this.targetName = targetName;
        this.requiredQuantity = requiredQuantity;
        this.isCompleted = false;
    }
}


public enum QuestConditionType
{
    Collect,  // 아이템 수집
    Explore,  // 지역 탐험
    Kill,     // 몬스터 처치
    Meet      // NPC 만남
}

[CreateAssetMenu(fileName = "QuestList", menuName = "Ds Project/QuestList")]
public class QuestList : ScriptableObject
{
    public List<Quest> questList = new List<Quest>();
}