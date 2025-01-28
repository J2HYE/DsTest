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
        //InitializeItemDatabase(ItemDatabase.itemList);
    }

    private void InitializeItemDatabase(List<Item> ItemDatabase)
    {
        // 메인퀘스트 아이템
        ItemDatabase.Add(new Item("Main_Quest001", "용사의 검의 조각", "용사가 남긴 검의 한 조각입니다.", ItemType.퀘스트, ItemGrade.희귀));
        ItemDatabase.Add(new Item("Main_Quest002", "신비한 알", "숲에서 발견한 신비로운 생물체의 알입니다.", ItemType.퀘스트, ItemGrade.전설));
        ItemDatabase.Add(new Item("Main_Quest003", "도끼", "나무를 자르는 데 필요한 기본 도구입니다.", ItemType.제작재료, ItemGrade.일반));
        ItemDatabase.Add(new Item("Main_Quest004", "나무완드", "나무로 만든 간단한 마법 지팡이입니다.", ItemType.장신구, ItemGrade.고급));
        ItemDatabase.Add(new Item("Main_Quest005", "수련용 검", "초보 전사가 사용하는 기본 검입니다.", ItemType.무기, ItemGrade.일반));
        ItemDatabase.Add(new Item("Main_Quest006", "빛의 폭발 스킬북", "빛의 폭발 스킬을 해금할 수 있는 아이템입니다.", ItemType.퀘스트, ItemGrade.희귀));
        ItemDatabase.Add(new Item("Main_Quest007", "회복의 반지", "자동 체력 회복 효과를 제공하는 반지입니다.", ItemType.장신구, ItemGrade.전설));
        ItemDatabase.Add(new Item("Main_Quest008", "마왕의 위치 단서", "마왕의 성으로 가는 길에 대한 단서가 담긴 지도입니다.", ItemType.퀘스트, ItemGrade.에픽));
        ItemDatabase.Add(new Item("Main_Quest009", "초급 공격 스크롤", "초급 공격 스킬을 배우는 데 필요한 스크롤입니다.", ItemType.퀘스트, ItemGrade.고급));
        ItemDatabase.Add(new Item("Main_Quest010", "전설의 증표", "전설적인 전사가 남긴 증표입니다.", ItemType.퀘스트, ItemGrade.전설));

        // 퀘스트아이템
        ItemDatabase.Add(new Item("Quest041", "마법 보석", "희귀한 퀘스트 아이템입니다.", ItemType.퀘스트, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest042", "보물 지도", "숨겨진 보물의 위치를 알려줍니다.", ItemType.퀘스트, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest043", "황금 열쇠", "특정한 문을 열 수 있는 열쇠입니다.", ItemType.퀘스트, ItemGrade.고급, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest044", "잃어버린 편지", "누군가에게 전달해야 할 편지입니다.", ItemType.퀘스트, ItemGrade.일반, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest045", "잃어버린 반지", "귀중한 반지로 보입니다.", ItemType.퀘스트, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest046", "고대 유물", "고대의 비밀을 담고 있는 유물입니다.", ItemType.퀘스트, ItemGrade.전설, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest047", "영혼의 수정", "영혼이 깃든 희귀한 수정입니다.", ItemType.퀘스트, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest048", "고대 열쇠", "고대 유적의 문을 여는 열쇠입니다.", ItemType.퀘스트, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest049", "고블린의 손도끼", "고블린에게서 빼앗은 도끼입니다.", ItemType.퀘스트, ItemGrade.고급, 1, 1, false, true));
        ItemDatabase.Add(new Item("Quest050", "전설의 증표", "전설적인 전사의 증표입니다.", ItemType.퀘스트, ItemGrade.전설, 1, 1, false, true));

        // 소비아이템
        ItemDatabase.Add(new Item("Item001", "회복 포션", "체력을 회복합니다.", ItemType.소모품, ItemGrade.일반, 10, 99, true, true));
        ItemDatabase.Add(new Item("Item002", "마나 포션", "마나를 회복합니다.", ItemType.소모품, ItemGrade.일반, 5, 99, true, true));
        ItemDatabase.Add(new Item("Item003", "힘의 물약", "일시적으로 힘을 증가시킵니다.", ItemType.소모품, ItemGrade.고급, 3, 99, true, true));
        ItemDatabase.Add(new Item("Item004", "속도의 물약", "일시적으로 이동 속도를 증가시킵니다.", ItemType.소모품, ItemGrade.고급, 2, 99, true, true));
        ItemDatabase.Add(new Item("Item005", "생명의 물약", "생명력을 회복합니다.", ItemType.소모품, ItemGrade.희귀, 8, 99, true, true));
        ItemDatabase.Add(new Item("Item006", "저항의 물약", "상태 이상 저항력을 증가시킵니다.", ItemType.소모품, ItemGrade.희귀, 4, 99, true, true));
        ItemDatabase.Add(new Item("Item007", "정화의 물약", "중독과 같은 상태 이상을 제거합니다.", ItemType.소모품, ItemGrade.희귀, 3, 99, true, true));
        ItemDatabase.Add(new Item("Item008", "폭발 물약", "적에게 큰 피해를 입히는 물약입니다.", ItemType.소모품, ItemGrade.희귀, 2, 99, true, true));
        ItemDatabase.Add(new Item("Item009", "독 물약", "적을 중독시키는 물약입니다.", ItemType.소모품, ItemGrade.고급, 6, 99, true, true));
        ItemDatabase.Add(new Item("Item010", "체력의 물약", "체력을 대폭 회복합니다.", ItemType.소모품, ItemGrade.희귀, 5, 99, true, true));
        ItemDatabase.Add(new Item("Item011", "힘의 묘약", "힘을 영구적으로 증가시킵니다.", ItemType.소모품, ItemGrade.에픽, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item012", "마력의 묘약", "마력을 영구적으로 증가시킵니다.", ItemType.소모품, ItemGrade.에픽, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item013", "불멸의 묘약", "불멸의 효과를 제공합니다.", ItemType.소모품, ItemGrade.신화, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item014", "투명 물약", "일정 시간 동안 투명 상태를 유지합니다.", ItemType.소모품, ItemGrade.고급, 3, 99, true, true));
        ItemDatabase.Add(new Item("Item015", "공격력 증가 물약", "공격력을 일시적으로 증가시킵니다.", ItemType.소모품, ItemGrade.고급, 4, 99, true, true));
        ItemDatabase.Add(new Item("Item016", "방어력 증가 물약", "방어력을 일시적으로 증가시킵니다.", ItemType.소모품, ItemGrade.고급, 4, 99, true, true));
        ItemDatabase.Add(new Item("Item017", "치유 포션", "즉각적으로 체력을 회복합니다.", ItemType.소모품, ItemGrade.일반, 10, 99, true, true));
        ItemDatabase.Add(new Item("Item018", "속성 강화 물약", "특정 속성을 강화합니다.", ItemType.소모품, ItemGrade.희귀, 3, 99, true, true));
        ItemDatabase.Add(new Item("Item019", "신속 물약", "캐릭터의 행동 속도를 증가시킵니다.", ItemType.소모품, ItemGrade.고급, 2, 99, true, true));
        ItemDatabase.Add(new Item("Item020", "전투 포션", "전투 능력을 강화합니다.", ItemType.소모품, ItemGrade.희귀, 1, 1, false, true));

        // 재료아이템
        ItemDatabase.Add(new Item("Item021", "용의 비늘", "희귀한 용의 비늘입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item022", "수정 조각", "희귀한 수정 조각입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item023", "유령의 눈", "유령으로부터 얻은 희귀한 재료입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item024", "거미의 독", "거미로부터 채취한 독입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item025", "늑대의 송곳니", "늑대의 날카로운 송곳니입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item026", "불꽃의 결정", "불의 정령으로부터 얻은 결정입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item027", "마법의 잉크", "마법 주문을 쓰는 데 필요한 잉크입니다.", ItemType.제작재료, ItemGrade.고급, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item028", "고대 석판", "고대의 비밀이 새겨진 석판입니다.", ItemType.제작재료, ItemGrade.전설, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item029", "은 조각", "은으로 만든 작은 조각입니다.", ItemType.제작재료, ItemGrade.고급, 1, 99, true, true));
        ItemDatabase.Add(new Item("Item030", "황금 가루", "희귀한 황금 가루입니다.", ItemType.제작재료, ItemGrade.희귀, 1, 99, true, true));

        // 장착아이템
        ItemDatabase.Add(new Item("Item031", "철검", "기본적인 검입니다.", ItemType.무기, ItemGrade.일반, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item032", "강철 방패", "적의 공격을 막아주는 강철 방패입니다.", ItemType.방어구, ItemGrade.고급, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item033", "가죽 갑옷", "기본적인 방어구입니다.", ItemType.방어구, ItemGrade.일반, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item034", "은검", "은으로 제작된 검입니다.", ItemType.무기, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item035", "룬검", "마법의 룬이 새겨진 검입니다.", ItemType.무기, ItemGrade.에픽, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item036", "마법 망토", "마법 방어력을 증가시키는 망토입니다.", ItemType.방어구, ItemGrade.에픽, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item037", "기사의 갑옷", "기사들이 사용하는 고급 방어구입니다.", ItemType.방어구, ItemGrade.희귀, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item038", "전설의 검", "전설적인 전사들이 사용했던 검입니다.", ItemType.무기, ItemGrade.전설, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item039", "고대 방패", "고대 문양이 새겨진 방패입니다.", ItemType.방어구, ItemGrade.전설, 1, 1, false, true));
        ItemDatabase.Add(new Item("Item040", "마법 검", "마법 공격력을 가진 검입니다.", ItemType.무기, ItemGrade.희귀, 1, 1, false, true));
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
                    new Reward("Main_Quest001", 1, 10, 10),
                    new Reward("Main_Quest002", 1, 10, 10),
                    new Reward("Main_Quest003", 1, 10, 10),
                    new Reward("Main_Quest004", 1, 10, 10),
                    new Reward("Main_Quest005", 1, 10, 10)
                }),

            // 1장 - 1: 마울로 향해
            new Quest("메인퀘스트", "quest101-1", "마을을 찾아가기",
                "마을을 찾아 이동하기.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_0011", new QuestCondition(QuestConditionType.Explore, "location_0011", "마을을 찾아가기", 1) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 10, 10) }),

            // 2장: 소년과 숲 속의 발견
            new Quest("메인퀘스트", "quest102", "숲 속의 알",
                "숲에서 딸기를 모으고, 이상한 소리를 따라 알을 발견하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_002", new QuestCondition(QuestConditionType.Explore, "location_002", "딸기밭 찾아가기", 1) },
                    { "Item_strawberry", new QuestCondition(QuestConditionType.Collect, "Item_strawberry", "딸기", 10) },
                    { "location_0022", new QuestCondition(QuestConditionType.Explore, "location_0022", "이상한 소리가 나는 장소", 1) },
                    { "Main_Quest001", new QuestCondition(QuestConditionType.Collect, "Main_Quest001", "용의 알", 1) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 15, 15) }),

            // 3장: 알에서 태어난 생명체
            new Quest("메인퀘스트", "quest103", "알의 신비",
                "알에서 깨어난 생명체를 몰래 숨길곳으로 데려가세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_003", new QuestCondition(QuestConditionType.Explore, "location_003", "알의 은신처", 1) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 20, 20) }),

            // 4장: 소년과 생물체의 우정
            new Quest("메인퀘스트", "quest104", "신비한 동물의 도움",
                "숲 속에서 나무를 패고 생물체의 도움을 확인하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_004", new QuestCondition(QuestConditionType.Explore, "location_004", "숲 속의 나무", 1) },
                    { "Quest001", new QuestCondition(QuestConditionType.Collect, "Quest001", "나뭇가지", 5) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 30, 30) }),

            // 5장: 하급 마족의 위협
            new Quest("메인퀘스트", "quest105", "엄마를 지켜라",
                "하급 마족과의 첫 전투에서 승리하고 생존하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Bear", new QuestCondition(QuestConditionType.Kill, "Bear", "하급 마족", 3) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 40, 50) }),

            // 6장: 숲 속에서 힘을 연마하다
            new Quest("메인퀘스트", "quest106", "새로운 힘",
                "숲 속에서 나무와 돌을 공격하며 힘을 연마하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_004", new QuestCondition(QuestConditionType.Explore, "location_004", "숲 속의 나무", 1) },
                    { "Item_rock", new QuestCondition(QuestConditionType.Collect, "Item_rock", "돌", 5) },
                    { "Item_tree", new QuestCondition(QuestConditionType.Collect, "Item_tree", "나무", 5) }
                },
                new List<Reward> { new Reward("Main_Quest001", 0, 50, 20) }),

            // 7장: 엄마의 복수를 실행해라
            new Quest("메인퀘스트", "quest107", "홀로서기",
                "엄마를 살해한 하급 관리자 3명을 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Mushroom", new QuestCondition(QuestConditionType.Kill, "Mushroom", "하급 관리자", 3) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 60, 60) }),

            // 8장: 마을의 위험에 대비하라
            new Quest("메인퀘스트", "quest108", "강해지기 위한 연습",
                "숲 속에서 위험구역을 통해 전투 능력을 강화하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_005", new QuestCondition(QuestConditionType.Explore, "location_005", "위험구역", 1) }
                },
                new List<Reward> { new Reward("Main_Quest001", 0, 70, 70) }),

            // 9장: 마족과의 전투
            new Quest("메인퀘스트", "quest109", "마을을 지켜라",
                "하급 관리자 3명을 처치하고 중급 관리자 모파안과 전투하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Mushroom", new QuestCondition(QuestConditionType.Kill, "Mushroom", "하급 관리자", 3) },
                    { "SlimRabbit", new QuestCondition(QuestConditionType.Kill, "SlimRabbit", "중급 관리자 모파안", 1) }
                },
                new List<Reward> { new Reward("Main_Quest001", 1, 80, 80) }),

            // 10장: 새로운 여정의 시작
            new Quest("메인퀘스트", "quest110", "마왕을 찾아서",
                "마을 사람들과 대화하여 마왕의 정보를 수집하고 여정을 시작하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "location_006", new QuestCondition(QuestConditionType.Explore, "location_006", "마왕의 성", 1) }
                },
                new List<Reward>
                {
                    new Reward("Main_Quest001", 1, 90, 90),
                    new Reward("Main_Quest001", 1, 90, 90)
                })
        };
        return questList;
    }

    public List<Quest> GenerateQuestLists()
    {
        List<Quest> questList = new List<Quest>
        {
            new Quest("서브퀘스트", "quest001", "회복 포션 수집",
                "회복 포션 10개를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Main_Quest001", new QuestCondition(QuestConditionType.Collect, "Item001", "회복 포션", 10) }
                },
                new List<Reward>
                {
                    new Reward("Main_Quest001", 5, 100, 50)
                }),

            new Quest("서브퀘스트", "quest002", "철검 제작 재료 수집",
                "철검 제작을 위한 재료를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item021", new QuestCondition(QuestConditionType.Collect, "Item021", "철광석", 1) }
                },
                new List<Reward>
                {
                    new Reward("Item002", 1, 200, 100)
                }),

            new Quest("서브퀘스트", "quest003", "가죽 갑옷 제작",
                "가죽 갑옷 제작을 위한 재료를 모으세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item025", new QuestCondition(QuestConditionType.Collect, "Item025", "가죽", 3) }
                },
                new List<Reward>
                {
                    new Reward("Item005", 1, 150, 80)
                }),

            new Quest("서브퀘스트", "quest004", "수정 조각 탐사",
                "숲에서 수정 조각을 찾아보세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item022", new QuestCondition(QuestConditionType.Collect, "Item022", "수정 조각", 2) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 300, 200)
                }),

            new Quest("서브퀘스트", "quest006", "늑대 사냥",
                "숲 속에서 늑대 5마리를 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "monster002", new QuestCondition(QuestConditionType.Kill, "monster002", "숲 늑대", 5) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 100, 50)
                }),

            new Quest("서브퀘스트", "quest007", "마나 포션 전달",
                "상인에게 마나 포션 5개를 전달하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item002", new QuestCondition(QuestConditionType.Collect, "Item002", "마나 포션", 5) }
                },
                new List<Reward>
                {
                    new Reward("Item004", 2, 200, 100)
                }),

            new Quest("서브퀘스트", "quest008", "강철 방패 제작",
                "강철 방패를 제작하기 위한 재료를 모으세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item026", new QuestCondition(QuestConditionType.Collect, "Item026", "강철 조각", 1) }
                },
                new List<Reward>
                {
                    new Reward("Item008", 1, 300, 150)
                }),


            new Quest("서브퀘스트", "quest009", "불꽃의 결정 탐사",
                "불꽃의 결정을 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item026", new QuestCondition(QuestConditionType.Collect, "Item026", "불꽃의 결정", 3) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 400, 200)
                }),

            new Quest("서브퀘스트", "quest016", "유령 몬스터 처치",
                "유령 몬스터 10마리를 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "monster006", new QuestCondition(QuestConditionType.Kill, "monster006", "유령 몬스터", 10) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 150, 50)
                }),

            new Quest("서브퀘스트", "quest017", "전설의 증표 회수",
                "전설의 증표를 회수하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item050", new QuestCondition(QuestConditionType.Collect, "Item050", "전설의 증표", 1) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 800, 500)
                }),

            new Quest("서브퀘스트", "quest001", "회복 포션 수집",
                "회복 포션 10개를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item001", new QuestCondition(QuestConditionType.Collect, "Item001", "회복 포션", 10) }
                },
                new List<Reward>
                {
                    new Reward("Item001", 5, 100, 50)
                }),

            new Quest("서브퀘스트", "quest002", "철검 제작 재료 수집",
                "철검 제작을 위한 재료를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item021", new QuestCondition(QuestConditionType.Collect, "Item021", "철광석", 1) }
                },
                new List<Reward>
                {
                    new Reward("Item002", 1, 200, 100)
                }),

            new Quest("서브퀘스트", "quest003", "가죽 갑옷 제작",
                "가죽 갑옷 제작을 위한 재료를 모으세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item025", new QuestCondition(QuestConditionType.Collect, "Item025", "가죽", 3)}
                },
                new List<Reward>
                {
                    new Reward("Item005", 1, 150, 80)
                }),

            new Quest("서브퀘스트", "quest004", "수정 조각 탐사",
                "숲에서 수정 조각을 찾아보세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item022", new QuestCondition(QuestConditionType.Collect, "Item022", "수정 조각", 2) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 300, 200) // 경험치 300, 골드 200 보상
                }),

            new Quest("서브퀘스트", "quest005", "유령의 눈 회수",
                "유령의 눈 2개를 회수하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item023", new QuestCondition(QuestConditionType.Collect, "Item023", "유령의 눈", 2) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 250, 150) // 경험치 250, 골드 150 보상
                }),

            new Quest("서브퀘스트", "quest006", "늑대 사냥",
                "숲 속에서 늑대 5마리를 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "monster002", new QuestCondition(QuestConditionType.Kill, "monster002", "숲 늑대", 5) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 100, 50) // 경험치 100, 골드 50 보상
                }),

            new Quest("서브퀘스트", "quest007", "마나 포션 전달",
                "상인에게 마나 포션 5개를 전달하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item002", new QuestCondition(QuestConditionType.Collect, "Item002", "마나 포션", 5) }
                },
                new List<Reward>
                {
                    new Reward("Item004", 2, 200, 100) // 마나 회복 아이템 2개, 경험치 200, 골드 100 보상
                }),

            new Quest("서브퀘스트", "quest008", "강철 방패 제작",
                "강철 방패를 제작하기 위한 재료를 모으세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item026", new QuestCondition(QuestConditionType.Collect, "Item026", "강철 조각", 1) }
                },
                new List<Reward>
                {
                    new Reward("Item008", 1, 300, 150) // 강철 방패 1개, 경험치 300, 골드 150 보상
                }),

            new Quest("서브퀘스트", "quest009", "불꽃의 결정 탐사",
                "불꽃의 결정을 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item026", new QuestCondition(QuestConditionType.Collect, "Item026", "불꽃의 결정", 3) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 400, 200) // 경험치 400, 골드 200 보상
                }),

            new Quest("서브퀘스트", "quest010", "고블린의 손도끼 회수",
                "고블린의 손도끼를 회수하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item049", new QuestCondition(QuestConditionType.Collect, "Item049", "고블린의 손도끼", 1) }
                },
                new List<Reward>
                {
                    new Reward("Item001", 2, 100, 50) // 회복 포션 2개, 경험치 100, 골드 50 보상
                }),

            new Quest("서브퀘스트", "quest012", "황금 가루 수집",
                "황금 가루를 2개 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item030", new QuestCondition(QuestConditionType.Collect, "Item030", "황금 가루", 2) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 200, 100) // 경험치 200, 골드 100 보상
                }),

            new Quest("서브퀘스트", "quest016", "유령 몬스터 처치",
                "유령 몬스터 10마리를 처치하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "monster006", new QuestCondition(QuestConditionType.Kill, "monster006", "유령 몬스터", 10) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 150, 50) // 경험치 150, 골드 50 보상
                }),

            new Quest("서브퀘스트", "quest020", "약초 수집",
                "치료제를 만들기 위한 약초를 수집하세요.",
                new Dictionary<string, QuestCondition>
                {
                    { "Item_101", new QuestCondition(QuestConditionType.Collect, "Item_101", "약초", 5) }
                },
                new List<Reward>
                {
                    new Reward("", 0, 50, 20) // 경험치 50, 골드 20 보상
                })


        };
        return questList;
    }


    public void GenerateRandomNPCs(int numberOfNPCs, List<Quest> availableQuests, List<Item> availableItems, List<Quest> baseDatabase, NPCList npcDataList)
    {
        List<Quest> mainQuest = GenerateMainQuestLists();

        for (int i = 0; i < numberOfNPCs; i++)
        {
            string npcName = $"NPC_{i + 1}";
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
            string npcName = $"메인퀘스트NPC {i + 1}";
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
            string npcName = $"상점NPC_{i + 1}";
            var shopData = new ShopData
            {
                shopId = npcName,
                shopName = npcName,
                grade = 0, //(ItemGrade)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemGrade)).Length),
                type = (ItemType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemType)).Length),
                //isSpecific = UnityEngine.Random.Range(0, 2) == 1
            };
            shopData.Initialize();

            shopNpcList.Add(new NPCData
            {
                id = npcName,
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
