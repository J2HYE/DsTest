using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateData
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 퀘스트 관련 제너레이터.
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void InitializeItems(ItemList ItemDatabase)
    {
    }

       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 퀘스트 관련 제너레이터.
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public List<Quest> GenerateMainQuestLists()
    { 
        List<Quest> questList = new List<Quest>
        {
            // 1장: 용사의 마지막 숨결
            new Quest("메인퀘스트", "quest101", "용사의 마지막 숨결",
                "용사의 마지막 전투를 목격하고, 남긴 검의 빛을 확인하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_001", new QuestCondition(QuestConditionType.Explore, "location_001", "용사의 마지막 전투 장소", 1) }
                },
                new List<Reward> 
                {
                    new Reward("용의알", 1, 10, 10),
                }),

            // 1장 - 1: 마울로 향해
            new Quest("메인퀘스트", "quest101-1", "마을을 찾아가기",
                "마을을 찾아 이동하기.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_0011", new QuestCondition(QuestConditionType.Explore, "location_0011", "마을을 찾아가기", 1) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 10, 10) }),

            // 2장: 소년과 숲 속의 발견
            new Quest("메인퀘스트", "quest102", "숲 속의 알",
                "숲에서 딸기를 모으고, 이상한 소리를 따라 알을 발견하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_002", new QuestCondition(QuestConditionType.Explore, "location_002", "딸기밭 찾아가기", 1) },
                    { "딸기", new QuestCondition(QuestConditionType.Collect, "딸기", "딸기", 10) },
                    { "location_0022", new QuestCondition(QuestConditionType.Explore, "location_0022", "이상한 소리가 나는 장소", 1) },
                    { "용의알", new QuestCondition(QuestConditionType.Collect, "용의알", "용의 알", 1) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 15, 15) }),

            // 3장: 알에서 태어난 생명체
            new Quest("메인퀘스트", "quest103", "알의 신비",
                "알에서 깨어난 생명체를 몰래 숨길곳으로 데려가세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_003", new QuestCondition(QuestConditionType.Explore, "location_003", "알의 은신처", 1) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 20, 20) }),

            // 4장: 소년과 생물체의 우정
            new Quest("메인퀘스트", "quest104", "신비한 동물의 도움",
                "숲 속에서 나무를 패고 생물체의 도움을 확인하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_004", new QuestCondition(QuestConditionType.Explore, "location_004", "숲 속의 나무", 1) },
                    { "나뭇가지", new QuestCondition(QuestConditionType.Collect, "Item_branch", "나뭇가지", 5) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 30, 30) }),

            // 5장: 하급 마족의 위협
            new Quest("메인퀘스트", "quest105", "엄마를 지켜라",
                "하급 마족과의 첫 전투에서 승리하고 생존하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Bear", new QuestCondition(QuestConditionType.Kill, "Bear", "하급 마족", 3) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 40, 50) }),

            // 6장: 숲 속에서 힘을 연마하다
            new Quest("메인퀘스트", "quest106", "새로운 힘",
                "숲 속에서 나무와 돌을 공격하며 힘을 연마하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_004", new QuestCondition(QuestConditionType.Explore, "location_004", "숲 속의 나무", 1) },
                    { "돌", new QuestCondition(QuestConditionType.Collect, "돌", "돌", 5) },
                    { "나무", new QuestCondition(QuestConditionType.Collect, "나무", "나무", 5) }
                },
                new List<Reward> { new Reward("소형 체력포션", 0, 50, 20) }),

            // 7장: 엄마의 복수를 실행해라
            new Quest("메인퀘스트", "quest107", "홀로서기",
                "엄마를 살해한 하급 관리자 3명을 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Mushroom", new QuestCondition(QuestConditionType.Kill, "Mushroom", "하급 관리자", 3) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 60, 60) }),

            // 8장: 마을의 위험에 대비하라
            new Quest("메인퀘스트", "quest108", "강해지기 위한 연습",
                "숲 속에서 위험구역을 통해 전투 능력을 강화하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_005", new QuestCondition(QuestConditionType.Explore, "location_005", "위험구역", 1) }
                },
                new List<Reward> { new Reward("소형 체력포션", 0, 70, 70) }),

            // 9장: 마족과의 전투
            new Quest("메인퀘스트", "quest109", "마을을 지켜라",
                "하급 관리자 3명을 처치하고 중급 관리자 모파안과 전투하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Mushroom", new QuestCondition(QuestConditionType.Kill, "Mushroom", "하급 관리자", 3) },
                    { "SlimRabbit", new QuestCondition(QuestConditionType.Kill, "SlimRabbit", "중급 관리자 모파안", 1) }
                },
                new List<Reward> { new Reward("소형 체력포션", 1, 80, 80) }),

            // 10장: 새로운 여정의 시작
            new Quest("메인퀘스트", "quest110", "마왕을 찾아서",
                "마을 사람들과 대화하여 마왕의 정보를 수집하고 여정을 시작하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_006", new QuestCondition(QuestConditionType.Explore, "location_006", "마왕의 성", 1) }
                },
                new List<Reward>
                {
                    new Reward("소형 체력포션", 1, 90, 90),
                    new Reward("소형 체력포션", 1, 90, 90)
                })
        };
        return questList;
    }

    public List<Quest> GenerateQuestLists()
    {
        List<Quest> questList = new List<Quest>
        {
            new Quest("서브퀘스트", "소형체력포션", "회복 포션 수집",
                "회복 포션 10개를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "소형체력포션", new QuestCondition(QuestConditionType.Collect, "소형체력포션", "회복 포션", 10) }
                },
                new List<Reward>
                {
                    new Reward("소형 체력포션", 10, 100, 50)
                }),

        };
        return questList;
    }

    private static RandomName[] cachedNames = (RandomName[])Enum.GetValues(typeof(RandomName));

    private string GetRandomName()
    {
        RandomName name = cachedNames[UnityEngine.Random.Range(0, cachedNames.Length)];
        return name.ToString();
    }

    public void GenerateRandomNPCs(int numberOfNPCs, List<Quest> availableQuests, List<Item> availableItems, List<Quest> baseDatabase, NPCList npcDataList)
    {
        List<Quest> mainQuest = GenerateMainQuestLists();

        for (int i = 0; i < numberOfNPCs; i++)
        {
            string npcName = GetRandomName();
            NPCType npcType = (NPCType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(NPCType)).Length);

            NPCData npc = new NPCData
            {
                id = $"NPC_{i + 1}",
                name = npcName,
                npcType = npcType,
                currentState = NPCState.중립
            };

            ConfigureNPC(npc, npcType, availableQuests, availableItems);
            npcDataList.npcLists.Add(npc);
        }

        npcDataList.mainQuestNpcLists = GenerateMainQuestNPCs(mainQuest);
        npcDataList.shopNpcLists = CreateShopNPC();
    }

    private List<NPCData> GenerateMainQuestNPCs(List<Quest> baseDatabase)
    {
        List<NPCData> mainNpcList = new List<NPCData>();
        for (int i = 0; i < baseDatabase.Count; i++)
        {
            string npcName = GetRandomName();
            NPCData mainnpc = new NPCData
            {
                id = $"MainNPC_{i + 1}",
                name = npcName,
                npcType = NPCType.퀘스트,
                currentState = NPCState.중립,
                quests = new[] { baseDatabase[i] },
                isQuestGiver = true,
                isInteractable = true,
                description = "메인 퀘스트를 제공하는 NPC입니다."
            };
            mainNpcList.Add(mainnpc);
        }
        return mainNpcList;
    }

    private List<NPCData> CreateShopNPC()
    {
        List<NPCData> shopNpcList = new List<NPCData>();
        for (int i = 0; i < 20 ; i++)
        {
            string npcName = GetRandomName();
            var shopData = new ShopData
            {
                shopId = $"ShopNPC_{i + 1}",
                shopName = npcName,
                grade = 0, //(ItemGrade)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemGrade)).Length),
                type = (ItemType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemType)).Length),
                //isSpecific = UnityEngine.Random.Range(0, 2) == 1
            };
            shopData.Initialize();

            shopNpcList.Add(new NPCData
            {
                id = shopData.shopId,
                name = npcName,
                npcType = NPCType.상점,
                currentState = NPCState.중립,
                isShop = true,
                isInteractable = true,
                shopData = shopData,
                description = "아이템을 판매하는 상점 NPC입니다."
            });
        }
        return shopNpcList;
    }

    // NPC 구성
    private void ConfigureNPC(NPCData npc, NPCType npcType, List<Quest> availableQuests, List<Item> availableItems)
    {
        switch (npcType)
        {
            case NPCType.퀘스트:
                npc.isQuestGiver = true;
                npc.isInteractable = true;
                npc.quests = GenerateRandomQuests(availableQuests, 1, 3); // 1~3개의 랜덤 퀘스트
                npc.description = "퀘스트를 제공하는 NPC입니다.";
                break;

            case NPCType.정보제공:
                npc.isInteractable = true;
                npc.description = "게임 정보를 제공하는 NPC입니다.";
                npc.dialogue = new[] { "저기 동굴에 보물이 있습니다.", "이 지역의 몬스터는 불에 약합니다." };
                break;

            case NPCType.상호작용:
                npc.description = "특정 장치를 작동시키는 NPC입니다.";
                npc.isInteractable = true;
                npc.interactionCondition = "특정 아이템 필요";
                break;

            case NPCType.힐러:
                npc.isInteractable = true;
                npc.description = "플레이어를 회복시켜주는 NPC입니다.";
                npc.dialogue = new[] { "필요한 치료가 있으신가요?", "안전을 기원합니다." };
                break;

            case NPCType.적NPC:
                npc.currentState = NPCState.적;
                npc.description = "플레이어를 공격할 수 있는 적대적 NPC입니다.";
                break;

            case NPCType.동료:
                npc.currentState = NPCState.동료;
                npc.description = "플레이어의 동료가 될 수 있는 NPC입니다.";
                break;

            default:
                npc.isInteractable = true;
                npc.description = "일반 대화만 가능한 NPC입니다.";
                break;
        }
    }


    private Item[] GenerateRandomItems(List<Item> availableItems, int min, int max)
    {
        if (availableItems == null || availableItems.Count == 0)
        {
            Debug.LogWarning("아이템 목록이 비어 있습니다. 아이템을 생성할 수 없습니다.");
            return new Item[0]; // 빈 배열 반환
        }
        int itemCount = Mathf.Clamp(UnityEngine.Random.Range(min, max + 1), 0, availableItems.Count);
        Item[] items = new Item[itemCount];

        for (int i = 0; i < itemCount; i++)
        {
            items[i] = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
        }

        return items;
    }

    //private Item[] GenerateRandomItems(List<Item> availableItems, int min, int max)
    //{
    //    int itemCount = Random.Range(min, max + 1);
    //    Item[] items = new Item[itemCount];
    //    for (int i = 0; i < itemCount; i++)
    //    {
    //        Item randomItem;
    //        do
    //        {
    //            randomItem = availableItems[Random.Range(0, availableItems.Count)];
    //        }
    //        while (randomItem.type != ItemType.소비형); // 소비형 아이템만 선택
    //        items[i] = randomItem;
    //    }
    //    return items;
    //}

    private Quest[] GenerateRandomQuests(List<Quest> availableQuests, int min, int max)
    {
        int questCount = UnityEngine.Random.Range(min, max + 1);
        Quest[] quests = new Quest[questCount];

        bool hasMainQuest = false;

        for (int i = 0; i < questCount; i++)
        {
            Quest randomQuest = availableQuests[UnityEngine.Random.Range(0, availableQuests.Count)];
            if (randomQuest.questType == "메인퀘스트")
            {
                if (hasMainQuest)
                {
                    i--;
                    continue;
                }
                else
                {
                    hasMainQuest = true;
                }
            }
            quests[i] = randomQuest;
        }
        return quests;
    }

    private Quest[] GenerateRandomQuests(List<Quest> availableQuests, int min, int max, string questTypeFilter = null)
    {
        int questCount = UnityEngine.Random.Range(min, max + 1);
        Quest[] quests = new Quest[questCount];

        for (int i = 0; i < questCount; i++)
        {
            Quest randomQuest = availableQuests[UnityEngine.Random.Range(0, availableQuests.Count)];

            if (!string.IsNullOrEmpty(questTypeFilter) && randomQuest.questType != questTypeFilter)
            {
                i--; // 필터에 맞지 않는 퀘스트를 다시 선택
                continue;
            }

            quests[i] = randomQuest;
        }
        return quests;
    }
}