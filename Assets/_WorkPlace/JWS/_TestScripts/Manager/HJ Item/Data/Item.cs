using System;
using UnityEngine;


/// <summary>
/// 01.21 희정 아이템 이펙트 항목 추가, 아이템 스탯 헤더 수정n
/// 01.22 아이템 복제, 스탯 복제 함수 수정
/// 01.23 아이템 장착위치 enum에 방패 추가, 아이템 등급은 장착 아이템에만 적용, 아이템 생성자에 제작재료 타입 조건 추가,
/// 양손검, 한손검 아이템 추가로 인해서 weapontype 변수 추가
/// 01.24 퀘스트 아이템인지 확인하기 위한 변수 추가
/// </summary>
[Serializable]
public class Item
{
    [Header("공통 속성")]
    public string id;                      // 아이템 고유 ID
    public string name;                    // 아이템 이름
    [TextArea(3, 10)]
    public string description;             // 아이템 설명
    public ItemType type;                  // 아이템 타입
    public int costValue;                  // 아이템 가격
    public int quantity;                   // 현재 소지 개수
    public int maxStack;                   // 최대 소지 가능 개수
    public bool isDiscardable;             // 버릴 수 있는지 여부
    public bool isStackable;               // 중첩 가능 여부
    public Sprite sprite;                  // 아이템 아이콘
    public float dropChance;               // 드랍 확률 (%)
    public ItemEffect effect;              // 아이템 이펙트
    public bool isQuestItem;               // 퀘스트 아이템 여부

    [Header("스탯(장착아이템: 적용할 전체 값,\n 버프 물약: 해당하는 스탯만 값을 1로 설정)")]
    public ItemStat itemStat;              // 스탯 정보 (힘, 민첩 등)
    public Durability durability;          // 내구도
    public ItemGrade grade;                // 아이템 등급
    public EquipmentSlot equipmentSlot;    // 장착 위치 (무기, 방어구 등)
    public WeaponType weaponType;          // 무기 타입
    public bool isEquired;                 // 장착여부

    [Header("소모품 속성")]
    public ConsumableType consumableType;  // 소모품 타입
    public int effectAmount;               // 효과량 (예: 체력 회복량)

    [Header("퀘스트 속성")]
    public string questId;                 // 퀘스트 ID (퀘스트 아이템용)

    // 생성자
    public Item(string id, string name, string description, ItemType type, ItemGrade grade, 
        int quantity = 1, int maxStack = 99, bool isStackable = true, bool isDiscardable = true, int costValue = 0)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.grade = grade;
        this.quantity = quantity;
        this.maxStack = maxStack;
        this.isStackable = isStackable;
        this.isDiscardable = isDiscardable;
        this.costValue = costValue;

        if (type == ItemType.무기 || type == ItemType.방어구)
        {
            // 장착 아이템 초기화
            this.itemStat = new ItemStat(1, 1, 1, 1, 1); // 기본 스탯
            this.durability = new Durability(100);       // 기본 내구도
            this.isEquired = false;
            this.itemStat.Initialize();
        }
        else if (type == ItemType.장신구)
        {
            // 퀘스트 아이템 초기화
            this.isDiscardable = false; // 퀘스트 아이템은 버릴 수 없음
            this.isEquired = false;
        }
        else if (type == ItemType.소모품 || type == ItemType.제작재료)
        {
            // 소모품 초기화
            this.effectAmount = 0;
            this.maxStack = 99;  // 소모품 기본 최대 스택
            this.isStackable = true;
        }
        else if (type == ItemType.퀘스트)
        {
            // 퀘스트 아이템 초기화
            this.isDiscardable = false; // 퀘스트 아이템은 버릴 수 없음
        }
    }

    // 아이템 복제
    public Item Clone()
    {
        Item newItem = new Item(this.id, this.name, this.description, this.type, this.grade,
            this.quantity, this.maxStack, this.isStackable, this.isDiscardable, this.costValue);

        if (this.itemStat != null)
        {
            newItem.itemStat = this.itemStat.Clone();
        }

        if (this.durability != null)
        {
            newItem.durability = this.durability.Clone(); // 내구도도 복제
        }

        newItem.effect = this.effect;
        newItem.isQuestItem = this.isQuestItem;
        newItem.sprite = this.sprite;
        newItem.dropChance = this.dropChance;
        newItem.equipmentSlot = this.equipmentSlot;
        newItem.weaponType = this.weaponType;
        newItem.consumableType = this.consumableType;
        newItem.effectAmount = this.effectAmount;
        newItem.questId = this.questId;

        return newItem;
    }

    // UI에 표시할 아이템 정보 (TextMeshPro 전용)
    public string ToStringTMPro()
    {
        // 등급에 따른 색상 가져오기
        string gradeColor = GetGradeColor(grade);

        // 기본 정보
        // string itemName = $"<b>이름: <color={gradeColor}>{name}</color></b>";
        string itemType = $"종류: <i><color=#87CEEB>{type}</color></i>    등급: <i><color={gradeColor}>{grade}</color></i>\n";
        string itemDescription = $"<color=#FFFFFF>{description}</color>\n";
        string itemQuantity = isStackable
            ? $"수량: <color=#00FF00>{quantity}/{maxStack}</color>"
            : "수량: <color=#FF0000>중첩 불가</color>";

        // 장착 위치 및 무기 타입
        string equipmentInfo = type == ItemType.무기 || type == ItemType.방어구 || type == ItemType.장신구
            ? $"\n장착 위치: <color=#FFD700>{equipmentSlot}</color>" +
              (type == ItemType.무기 ? $"    무기 타입: <color=#FFD700>{weaponType}</color>" : "")
            : "";

        // 소모품 효과량
        string consumableInfo = type == ItemType.소모품
            ? $"효과: <color=#00FF00>{effectAmount}</color>" +
              (consumableType != ConsumableType.없음 ? $"    소모품 타입: <color=#FFD700>{consumableType}</color>" : "")
            : "";

        // 스탯 정보
        string statInfo = itemStat != null
            ? $"\n<b><color=#FFD700>[스탯 정보]</color></b>\n" +
              (itemStat.Strength > 0 ? $"힘 : {itemStat.Strength}    " : "") +
              (itemStat.Dexterity > 0 ? $"민첩 : {itemStat.Dexterity}    " : "") +
              (itemStat.Intelligence > 0 ? $"지능 : {itemStat.Intelligence}    " : "") +
              (itemStat.Vitality > 0 ? $"활력 : {itemStat.Vitality}    " : "") +
              (itemStat.Luck > 0 ? $"운 : {itemStat.Luck}    " : "") +
              (itemStat.PhysicalAttack > 0 ? $"\n물리 공격력 : {itemStat.PhysicalAttack}    " : "") +
              (itemStat.MagicAttack > 0 ? $"마법 공격력 : {itemStat.MagicAttack}    " : "") +
              (itemStat.PhysicalDefense > 0 ? $"물리 방어력 : {itemStat.PhysicalDefense}    " : "") +
              (itemStat.MagicDefense > 0 ? $"\n마법 방어력 : {itemStat.MagicDefense}    " : "") +
              (itemStat.CriticalChance > 0 ? $"치명타 확률 : {itemStat.CriticalChance}%    " : "") +
              (itemStat.AttackSpeed > 0 ? $"공격 속도 : {itemStat.AttackSpeed}    " : "") +
              (itemStat.Evasion > 0 ? $"\n회피율 : {itemStat.Evasion}%    " : "")
            : "";

        // 전체 문자열 조합
        return $"{itemType}\n{itemDescription}\n{itemQuantity}\n{equipmentInfo}\n{consumableInfo}{statInfo}".Trim();
    }

    // 등급에 따른 색상 반환 메서드
    public string GetGradeColor(ItemGrade grade)
    {
        switch (grade)
        {
            case ItemGrade.일반: return "#FFFFFF";     // 흰색
            case ItemGrade.고급: return "#1EFF00";  // 초록색
            case ItemGrade.희귀: return "#0070DD";      // 파랑색
            case ItemGrade.에픽: return "#A335EE";      // 보라색
            case ItemGrade.전설: return "#FF8000"; // 주황색
            case ItemGrade.신화: return "#FF0000";    // 빨간색
            default: return "#FFFFFF";                 // 기본 흰색
        }
    }

}

[Serializable]
public class ItemStat
{
    [Header("기본 스탯")]
    public int Strength;          // 힘
    public int Dexterity;         // 민첩
    public int Intelligence;      // 지능
    public int Vitality;          // 활력
    public int Luck;              // 운

    [Header("전투 스탯")]
    public int MaxHealth;         // 최대 체력
    public int MaxMana;           // 최대 마나
    public int PhysicalAttack;    // 물리 공격력
    public int MagicAttack;       // 마법 공격력
    public int PhysicalDefense;   // 물리 방어력
    public int MagicDefense;      // 마법 방어력

    [Header("보조 스탯")]
    public int CriticalChance;    // 치명타 확률 (%)
    public int AttackSpeed;       // 공격 속도
    public int Evasion;           // 회피율 (%)

    // 생성자
    public ItemStat(int strength, int dexterity, int intelligence, int vitality, int luck)
    {
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Vitality = vitality;
        Luck = luck;
    }

    // 스탯 초기화는 사용할때 사용하는걸로.
    public void Initialize()
    {
        MaxHealth = Vitality * 10;
        MaxMana = Intelligence * 5;
        PhysicalAttack = Strength * 2;
        MagicAttack = Intelligence * 2;
        PhysicalDefense = Vitality * 1;
        MagicDefense = Intelligence * 1;
        CriticalChance = Luck / 2;
        AttackSpeed = Dexterity / 2;
        Evasion = Dexterity / 3;
    }

    // 복제 메서드
    public ItemStat Clone()
    {
        ItemStat newStat = new ItemStat(Strength, Dexterity, Intelligence, Vitality, Luck);

        newStat.MaxHealth = this.MaxHealth;
        newStat.MaxMana = this.MaxMana;
        newStat.PhysicalAttack = this.PhysicalAttack;
        newStat.MagicAttack = this.MagicAttack;
        newStat.PhysicalDefense = this.PhysicalDefense;
        newStat.MagicDefense = this.MagicDefense;
        
        newStat.CriticalChance = this.CriticalChance;
        newStat.AttackSpeed = this.AttackSpeed;
        newStat.Evasion = this.Evasion;

        return newStat;
    }
}

// 아이템 타입 Enum
public enum ItemType
{
    무기,         // Weapon
    방어구,       // Armor
    소모품,       // Consumable
    퀘스트,       // QuestItem
    제작재료,     // Material
    장신구,        // Accessory
}

public enum WeaponType
{
    한손무기,
    양손무기
}

// 장착 위치 Enum
public enum EquipmentSlot
{
    방패,   // Shield
    머리,   // Head
    몸,     // Body
    손,     // Hand
    발,     // Feet
}

// 소모품 타입 Enum
public enum ConsumableType
{
    없음,       // None
    체력포션,   // HealthPotion
    마나포션,   // ManaPotion
    버프        // Buff
}


// 아이템 등급 Enum
public enum ItemGrade
{
    일반,      // Common
    고급,      // Uncommon
    희귀,      // Rare
    에픽,      // Epic
    전설,      // Legendary
    신화       // Mythic
}
