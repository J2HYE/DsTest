using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerater : MonoBehaviour
{
    [Header("퀘스트 관련 데이터")]
    public List<string> questNames; // 퀘스트 이름 풀
    public List<string> questDescriptions; // 퀘스트 설명 풀

    [Header("조건 관련 데이터")]
    public List<string> itemList; // 아이템 ID 리스트
    public List<Transform> locationList; // 탐험 목표 위치 리스트
    public List<string> npcList; // 만남 목표 NPC ID 리스트
    public List<MonsterData> monsterList; // 몬스터 정보 리스트 (ID, 난이도 포함)

    [Header("보상 관련 데이터")]
    public int baseGoldReward = 10; // 기본 골드 보상
    public int baseExpReward = 5; // 기본 경험치 보상
    public int difficultyMultiplier = 10; // 난이도당 보상 증가치


    public Quest GenerateQuest(int difficulty, int conditionCount = 1)
    {
        // 1. 퀘스트 기본 정보 설정
        string questId = Guid.NewGuid().ToString(); // 유니크 ID
        string questName = questNames[UnityEngine.Random.Range(0, questNames.Count)];
        string questDescription = questDescriptions[UnityEngine.Random.Range(0, questDescriptions.Count)];

        // 2. 퀘스트 조건 생성
        Dictionary<string, QuestCondition> conditions = GenerateMultipleConditions(difficulty, conditionCount);

        // 3. 퀘스트 보상 생성
        List<Reward> rewards = GenerateRewards(difficulty);

        // 4. 퀘스트 생성 및 반환
        return new Quest("SubQuest", questId, questName, questDescription, conditions, rewards);
    }

    /// <summary>
    /// 복합 조건 생성 로직 (여러 조건 조합)
    /// </summary>
    private Dictionary<string, QuestCondition> GenerateMultipleConditions(int difficulty, int conditionCount)
    {
        Dictionary<string, QuestCondition> conditions = new Dictionary<string, QuestCondition>();

        // 복합 조건 생성
        for (int i = 0; i < conditionCount; i++)
        {
            QuestConditionType conditionType = (QuestConditionType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(QuestConditionType)).Length);
            QuestCondition condition = GenerateSingleCondition(conditionType, difficulty);

            // 조건을 추가 (key: targetId)
            conditions[condition.targetId] = condition;
        }

        return conditions;
    }

    /// <summary>
    /// 단일 조건 생성 로직
    /// </summary>
    private QuestCondition GenerateSingleCondition(QuestConditionType conditionType, int difficulty)
    {
        switch (conditionType)
        {
            case QuestConditionType.Kill:
                // 몬스터 처치 조건 생성
                MonsterData monster = SelectMonsterByDifficulty(difficulty);
                int killCount = difficulty * 2; // 난이도에 따라 처치 수 증가
                return new QuestCondition(conditionType, monster.characterName, monster.characterName, killCount);

            case QuestConditionType.Collect:
                // 아이템 수집 조건 생성
                string itemId = SelectItemByDifficulty(difficulty);
                int collectCount = difficulty + UnityEngine.Random.Range(1, 5); // 난이도 기반 수량
                return new QuestCondition(conditionType, itemId, "수집 아이템", collectCount);

            case QuestConditionType.Explore:
                // 탐험 조건 생성
                Transform location = locationList[UnityEngine.Random.Range(0, locationList.Count)];
                return new QuestCondition(conditionType, location.name, "탐험 위치", 1);

            case QuestConditionType.Meet:
                // NPC 만남 조건 생성
                string npcId = npcList[UnityEngine.Random.Range(0, npcList.Count)];
                return new QuestCondition(conditionType, npcId, "NPC 만나기", 1);

            default:
                throw new Exception("알 수 없는 조건 유형");
        }
    }

    /// <summary>
    /// 난이도에 따라 몬스터 선택
    /// </summary>
    private MonsterData SelectMonsterByDifficulty(int difficulty)
    {
        // 난이도 범위에 맞는 몬스터 필터링
        List<MonsterData> filteredMonsters = monsterList;

        // 적합한 몬스터가 없으면 랜덤 선택
        if (filteredMonsters.Count == 0) return monsterList[UnityEngine.Random.Range(0, monsterList.Count)];

        // 필터링된 몬스터 중 랜덤 선택
        return filteredMonsters[UnityEngine.Random.Range(0, filteredMonsters.Count)];
    }

    /// <summary>
    /// 난이도에 따라 아이템 선택
    /// </summary>
    private string SelectItemByDifficulty(int difficulty)
    {
        // 간단히 난이도를 기반으로 아이템을 선택 (예: 아이템 인덱스를 난이도로 나눔)
        int index = difficulty % itemList.Count; // 아이템 리스트 내에서 순환
        return itemList[index];
    }

    /// <summary>
    /// 보상 생성 로직
    /// </summary>
    private List<Reward> GenerateRewards(int difficulty)
    {
        List<Reward> rewards = new List<Reward>();

        // 1. 기본 골드 및 경험치 보상
        int goldReward = baseGoldReward + (difficulty * difficultyMultiplier);
        int expReward = baseExpReward + (difficulty * difficultyMultiplier);

        rewards.Add(new Reward(null, 0, expReward, goldReward)); // 경험치와 골드 추가

        for (int i = 0; i < difficulty + 1; i++)
        {
            // 2. 아이템 보상 (확률적으로 추가)
            if (UnityEngine.Random.value < 0.5f) // 50% 확률로 아이템 보상 추가
            {
                string rewardItemId = SelectItemByDifficulty(difficulty);
                int rewardItemCount = 1;
                if (ItemManager.Instance.GetItemById(rewardItemId).isStackable) 
                    rewardItemCount += UnityEngine.Random.Range(1, 3); // 1~3개
                rewards.Add(new Reward(rewardItemId, rewardItemCount, 0, 0));
            }
        }

        return rewards;
    }
}
