using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatModifier
{
    public float vitalityMultiplier = 10f;       // 체력 배율
    public float mpMultiplier = 5f;              // MP 배율
    public float levelMpBonus = 10f;             // 레벨당 추가 MP 보너스
    public float strengthMultiplier = 1.5f;      // 힘에 의한 물리 공격력 배율
    public float agilityMultiplier = 0.01f;      // 민첩성에 의한 크리티컬 확률 배율
    public float physicalResistanceMultiplier = 1f; // 물리 방어력 배율
    public float magicResistanceMultiplier = 2f;   // 마법 방어력 배율
    public float intelligenceMultiplier = 1.5f;  // 지능에 의한 마법 공격력 배율

    // 필요시, 복사 메서드를 구현할 수 있음 (이 예시에서는 간단하게 구현)
    public StatModifier Clone() => (StatModifier)this.MemberwiseClone();
}

public enum StatType
{
    Strength,
    Vitality,
    Agility,
    Intelligence
}

// 250123 11:20AM sohyeon 모든 캐릭터데이터에 mpRecoveryRate 초기화 부분 추가함!!
// CharacterData 클래스 정의: 캐릭터의 스탯, 레벨, 경험치 등을 관리
[Serializable]
public class CharacterData
{
    public event Action<Transform> OnTakeDamage; // 피격 했는지 이벤트 전달

    private const int maxStats = 999;
    private const int minStats = 0;
    private const float MaxDodgeChance = 0.75f;
    private const float MaxBlockChance = 0.5f;

    public string characterName; // 캐릭터 이름
    public CharacterType characterType; // 캐릭터 타입
    public GameObject characterPrefab; // 캐릭터 프리팹

    public int level; // 레벨
    [HideInInspector] public float currentExperience; // 현재 경험치
    [HideInInspector] public float experienceToLevelUp; // 레벨업에 필요한 경험치

    // 기본 스텟 (정수형)
    public int strength; // 힘 - 물리 공격력, 물리 방어력, 피해량 감소, 방어 확률 
    public int agility; // 민첩 - 회피, 치명타 확률, 공격속도, 적중률
    public int vitality; // 체력 - HP, 물리 방어력, 피해량 감소
    public int intelligence; // 지능 - 마법 데미지, 마법 방어력, MP, MP 회복 속도

    // 리소스 (체력, 마나, 스태미너 등)
    public int maxHp; // 최대 체력
    public int currentHp; // 현재 체력
    public float hpRecoveryRate; // 체력 회복 속도

    public int maxMp; // 최대 MP
    public int currentMp; // 현재 MP
    public float mpRecoveryRate; // MP 회복 속도

    public float stamina; // 최대 스태미너
    public float staminaCurrent; // 현재 스테미너
    public float staminaRecoveryRate; // 스태미너 회복 속도

    // 이동 및 공격 관련
    public float moveSpeed; // 이동 속도
    public float attackSpeed; // 공격 속도 (초당 공격 횟수)
    public float attackRange; // 공격 범위 (근거리/원거리 구분)

    // 전투 관련 스탯
    [HideInInspector] public float physicalDefense; // 물리 방어력
    [HideInInspector] public float magicDefense; // 마법 방어력
    [HideInInspector] public float physicalDamage; // 물리 공격력
    [HideInInspector] public float magicDamage; // 마법 공격력
    [HideInInspector] public float criticalChance; // 크리티컬 확률
    [HideInInspector] public float criticalDamage; // 크리티컬 데미지 배율
    [HideInInspector] public float dodgeChance; // 회피 확률
    public bool hasShield; // 방패 착용 여부
    [HideInInspector] public float blockChance = 0f; // 방어 확률
    [HideInInspector] public float physicalDamageReduction; // 피해량 감소율 (ex. 받는 피해 -%)
    [HideInInspector] public float magicDamageReduction; // 피해량 감소율 (ex. 받는 피해 -%)

    
    public StatModifier statModifier;
    private StatModifier baseStatModifier = new StatModifier();

    private Dictionary<StatType, int> stats;

    private bool isLevelingUp = false; // 레벨업 진행 중 여부

    // 생성자: 캐릭터 초기화 및 자동 계산
    public CharacterData(
        string name,
        CharacterType type,
        GameObject prefab,
        int strength,
        int agility,
        int vitality,
        int intelligence,
        StatModifier modifier,
        float speed,
        float attackSpeed,
        float attackRange,
        float stamina,
        float staminaRecoveryRate,
        float mpRecoveryRate)
    {
        this.characterName = name;
        this.characterType = type;
        this.characterPrefab = prefab;

        this.strength = strength;
        this.agility = agility;
        this.vitality = vitality;
        this.intelligence = intelligence;

        this.statModifier = modifier ?? baseStatModifier;

        // 파생 스탯 초기화
        UpdateDerivedStats();

        // 리소스 초기화
        this.stamina = stamina;
        this.hpRecoveryRate = 1f; // 기본 체력 회복 속도 예시
        this.mpRecoveryRate = mpRecoveryRate;
        this.staminaRecoveryRate = staminaRecoveryRate;

        // 이동 및 공격 관련 초기화
        this.moveSpeed = speed;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;

        // 레벨 및 경험치 초기화
        this.level = 1;
        this.currentExperience = 0;
        this.experienceToLevelUp = CalculateExperienceToLevelUp();

        InitializeStats();
    }

    public void InitializeStats()
    {
        stats = new Dictionary<StatType, int>
        {
            { StatType.Strength, strength },
            { StatType.Vitality, vitality },
            { StatType.Agility, agility },
            { StatType.Intelligence, intelligence }
        };
    }

    // 데이터 초기화 함수
    public void ResetDataByLevel()
    {
        // 기본적인 스탯과 상태 초기화
        currentHp = maxHp;
        currentMp = maxMp;
        staminaCurrent = stamina;

        // 레벨과 관련된 동적 데이터 재계산
        UpdateDerivedStats();

        Debug.Log($"Monster data reset. Level: {level}, HP: {currentHp}/{maxHp}");
    }

    // 파생 스탯 계산
    public void UpdateDerivedStats()
    {
        int previousMaxHp = maxHp; // 이전 최대 체력 저장
        int previousMaxMp = maxMp; // 이전 최대 MP 저장

        // 체력, MP 등 파생 스탯 계산
        maxHp = Mathf.RoundToInt(vitality * statModifier.vitalityMultiplier); // 체력에 비례한 최대 HP
        maxMp = Mathf.RoundToInt((intelligence * statModifier.mpMultiplier) +
                                 (level * statModifier.levelMpBonus)); // MP 계산

        // 물리 공격력 및 방어력 계산
        physicalDamage = Mathf.RoundToInt(Mathf.Max(strength * statModifier.strengthMultiplier, 1f)); // 기본값을 1f로 설정하여 0 방지
        physicalDefense = Mathf.RoundToInt(Mathf.Max((strength + vitality) * statModifier.physicalResistanceMultiplier, 1f)); // 기본값을 1f로 설정
        
        // 마법 공격력 및 방어력 계산
        magicDamage = Mathf.RoundToInt(Mathf.Max(intelligence * statModifier.intelligenceMultiplier, 1f)); // 기본값을 1f로 설정
        magicDefense = Mathf.RoundToInt(Mathf.Max(intelligence * statModifier.magicResistanceMultiplier, 1f)); // 기본값을 1f로 설정


        // 크리티컬 확률 계산
        criticalChance = Mathf.Min(agility * statModifier.agilityMultiplier, 1f); // 민첩성에 따른 크리티컬 확률
        criticalDamage = 1.5f; // 크리티컬 데미지 배율 예시 (게임에 맞게 수정 가능)

        // 회피 확률 및 방어 확률 (예시로 설정)
        dodgeChance = Mathf.Min(agility * 0.01f, MaxDodgeChance); // 회피 확률 (최대 75%)
        
        // 2025-01-27 HYO 블록 확률 추후 아이템으로 빼고 제거 해야함 -------------------------------------------
        blockChance = Mathf.Min(strength * 0.01f, MaxBlockChance); // 방어 확률 (최대 50%)
        // ---------------------------------------------------------------------------------------

        // 2025-01-27 HYO 피해 감소율 계산 이후 로직 수정 필요 ----------------------------------------
        // 피해 감소율 계산 (50%가 넘지 않도록 제한)
        physicalDamageReduction = Mathf.Min(physicalDefense / (physicalDefense + 100), 0.5f);
        magicDamageReduction = Mathf.Min(magicDefense / (magicDefense + 100), 0.5f);
        // ----------------------------------------------------------------------------------

        // 체력 증가 처리
        if (maxHp > previousMaxHp)
        {
            currentHp += maxHp - previousMaxHp;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp); // 현재 체력은 최대 체력보다 크지 않게 설정
        }

        // MP 증가 처리
        if (maxMp > previousMaxMp)
        {
            currentMp += maxMp - previousMaxMp;
            currentMp = Mathf.Clamp(currentMp, 0, maxMp); // 현재 MP는 최대 MP보다 크지 않게 설정
        }
    }
    
    // 경험치를 얻었을 때 호출되는 함수
    public void AddExperience(int amount)
    {
        if (amount < 0) return;

        currentExperience += amount;

        // 경험치가 레벨업 요구치를 초과하면 레벨업 처리
        while (currentExperience >= experienceToLevelUp)
        {
            if (!isLevelingUp)
            {
                currentExperience -= experienceToLevelUp;
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        isLevelingUp = true; // 레벨업 시작

        // 레벨 증가
        level++;

        // 새로운 레벨에 맞는 경험치 계산
        experienceToLevelUp = CalculateExperienceToLevelUp();

        // 레벨업 시 능력치 증가 및 파생 스탯 계산
        IncreaseStatsBasedOnLevel();

        // 레벨업 시 모든 리소스 회복
        currentHp = maxHp;
        currentMp = maxMp;
        staminaCurrent = stamina;

        // 디버깅 로그
        Debug.Log($"레벨업! 현재 레벨: {level}, 남은 경험치: {currentExperience}");

        isLevelingUp = false; // 레벨업 종료
    }

    // 경험치 계산 함수
    public int CalculateExperienceToLevelUp()
    {
        // 레벨 9 -> 10, 19 -> 20, 29 -> 30 구간에서 경험치를 완화
        return (level % 10 == 9) ? Mathf.RoundToInt(level * 80f) : Mathf.RoundToInt(level * 100f);
    }

    // 능력치 증가 처리 함수 (레벨에 따른 규칙을 설정)
    private void IncreaseStatsBasedOnLevel()
    {
        strength++;
        vitality++;
        agility++;
        intelligence++;

        UpdateDerivedStats();
    }
    
    // 특정 스텟 증가 함수
    public void ModifyStat(StatType statType, int amount)
    {
        if (!stats.ContainsKey(statType)) return;

        stats[statType] = Mathf.Clamp(stats[statType] + amount, minStats, maxStats);

        strength = stats[StatType.Strength];
        vitality = stats[StatType.Vitality];
        agility = stats[StatType.Agility];
        intelligence = stats[StatType.Intelligence];

        UpdateDerivedStats();
    }

    // 피해를 입었을 때
    public void TakeDamage(int damage, Transform attacker = null)
    {
        currentHp = Mathf.Max(0, currentHp - damage);
        OnTakeDamage?.Invoke(attacker);
    }

    // 회복
    public void Heal(int amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
    }

    private void AdjustMp(float amount)
    {
        // currentMp에 적용할 때만 float를 int로 변환
        currentMp = Mathf.Clamp(currentMp + Mathf.RoundToInt(amount), 0, maxMp);
    }

    public void RegenerateMp()
    {
        AdjustMp(mpRecoveryRate); // mpRecoveryRate는 float로 유지
    }

    public void UseItemForMp(float recoveryAmount)
    {
        AdjustMp(recoveryAmount); // recoveryAmount는 float로 유지
    }

    public void UseMp(float amount)
    {
        AdjustMp(-amount); // amount는 float로 유지
    }

    private void AdjustStamina(float amount)
    {
        staminaCurrent = Mathf.Clamp(staminaCurrent + amount, 0, stamina);
    }

    public void RegenerateStamina()
    {
        AdjustStamina(staminaRecoveryRate);
    }

    public void UseItemForStamina(float recoveryAmount)
    {
        AdjustStamina(recoveryAmount);
    }

    public void UseStamina(float amount)
    {
        AdjustStamina(-amount);
    }

    public CharacterData Clone()
    {
        return new CharacterData(
            this.characterName,
            this.characterType,
            this.characterPrefab,
            this.strength,
            this.agility,
            this.vitality,
            this.intelligence,
            this.statModifier != baseStatModifier ? statModifier.Clone() : baseStatModifier,
            this.moveSpeed,
            this.attackSpeed,
            this.attackRange,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate
        );
    }

    public virtual string ToStringForTMPro()
    {
        string baseInfo = string.Format(
            "<color=red>Name:</color> {0}\n" +
            "<color=red>Level:</color> {1}\n" +
            "<color=red>Strength:</color> {2}\n" +
            "<color=blue>Agility:</color> {3}\n" +
            "<color=green>Vitality:</color> {4}\n" +
            "<color=yellow>Intelligence:</color> {5}\n" +
            "<color=lime>Current HP:</color> {6}/{7}\n" +
            "<color=cyan>Current MP:</color> {8}/{9}\n" +
            "<color=lime>Stamina:</color> {10}/{11} (Recovery: {12}/s)\n" +
            "<color=purple>Physical Damage:</color> {13}\n" +
            "<color=cyan>Magic Damage:</color> {14}\n" +
            "<color=orange>Physical Defense:</color> {15}\n" +
            "<color=orange>Magic Defense:</color> {16}\n" +
            "<color=magenta>Critical Chance:</color> {17}%\n" +
            "<color=teal>Dodge Chance:</color> {18}%\n" + // 회피율 추가
            "<color=teal>Block Chance:</color> {19}%\n" + // 방어율 추가
            "<color=black>Base Damage:</color> {20}\n" +
            "<color=teal>Speed:</color> {21}\n" +
            "<color=yellow>Attack Speed:</color> {22}\n" +
            "<color=orange>Current Experience:</color> {23}\n" +
            "<color=orange>Experience To Level Up:</color> {24}\n" +
            "<color=cyan>Shield Status:</color> {25}\n", // 방패 착용 여부 추가
            characterName, // 0
            level, // 1
            strength, // 2
            agility, // 3
            vitality, // 4
            intelligence, // 5
            currentHp, // 6
            maxHp, // 7
            currentMp, // 8
            maxMp, // 9
            staminaCurrent, // 10
            stamina, // 11
            staminaRecoveryRate, // 12
            physicalDamage, // 13
            magicDamage, // 14
            physicalDefense, // 15
            magicDefense, // 16
            criticalChance * 100, // 17
            dodgeChance * 100, // 18 회피율 %
            blockChance * 100, // 19 방어율 %
            physicalDamage, // 20 (or another base damage if needed)
            moveSpeed, // 21
            attackSpeed, // 22
            currentExperience, // 23
            experienceToLevelUp, // 24
            hasShield ? "<color=green>Equipped</color>" : "<color=red>Not Equipped</color>" // 25
        );
        return baseInfo;
    }
}

// 플레이어 데이터
[Serializable]
public class PlayerData : CharacterData
{
    public List<string> skills = new List<string>();    // 스킬 목록
    public int gold; // 골드 소지량

    // 생성자
    public PlayerData(string name, GameObject prefab, int strength, int vitality, int agility, int intelligence,
        float speed, float attackSpeed, float attackRange, float stamina, float staminaRecoveryRate, float mpRecoveryRate)
        : base(name, CharacterType.Player, prefab, strength, agility, vitality, intelligence, null, speed, attackSpeed, attackRange, stamina, staminaRecoveryRate, mpRecoveryRate)
    {
        gold = 0; // 초기 골드는 0
    }

    public void AddSkill(string skill) => skills.Add(skill);

    public void AddGold(int amount)
    {
        if (amount < 0) return;
        gold += amount;
        UIManager.Instance.PickUpItemTextDisplay?.AddItem($"{amount}골드를 획득했습니다.", ItemManager.Instance.GetItemSprite("금화"));
        Debug.Log($"골드 추가: {amount}, 현재 골드: {gold}");
    }

    public bool UseGold(int amount)
    {
        if (amount > gold)
        {
            Debug.LogWarning("골드가 부족합니다!");
            return false;
        }
        gold -= amount;
        Debug.Log($"골드 사용: {amount}, 남은 골드: {gold}");
        return true;
    }
    
    public new PlayerData Clone()
    {
        var baseClone = (PlayerData)base.Clone(); // CharacterData.Clone을 호출

        baseClone.gold = this.gold;  // 골드 복사
        baseClone.skills = new List<string>(this.skills);  // 스킬 리스트 복사

        return baseClone;
    }
    public override string ToStringForTMPro()
    {
        string baseInfo = base.ToStringForTMPro();
        string skillsInfo = skills.Count > 0 ? string.Join(", ", skills) : "No skills acquired";
        string playerInfo = $"<color=gold>Gold:</color> {gold}\n" +
                            $"<color=cyan>Skills:</color> {skillsInfo}\n";
        return baseInfo + playerInfo;
    }
}

// 몬스터 데이터
[Serializable]
public class MonsterData : CharacterData
{
    public List<string> dropItems = new List<string>(); // 드롭 아이템
    public int experienceReward; // 경험치 보상
    public int goldReward;       // 골드 보상
    [HideInInspector] public GameObject instance;  // 생성된 몬스터 인스턴스를 저장할 필드

    public MonsterData(
        string name, 
        CharacterType characterType, 
        GameObject prefab, 
        int strength, 
        int vitality, 
        int agility,
        int intelligence, 
        float speed, 
        float attackSpeed, 
        float attackRange, 
        float stamina, 
        float staminaRecoveryRate, 
        float mpRecoveryRate, 
        List<string> dropItems,
        int experienceReward, 
        int goldReward)
        : base(name, CharacterType.Monster, prefab, strength, agility, vitality, intelligence, null, speed, attackSpeed, attackRange, stamina, staminaRecoveryRate, mpRecoveryRate)
    {
        this.dropItems = new List<string>(dropItems);
        this.experienceReward = experienceReward;
        this.goldReward = goldReward;
    }
    public new MonsterData Clone()
    {
        var clone = new MonsterData(
            this.characterName,
            this.characterType,
            this.characterPrefab,
            this.strength,
            this.vitality,
            this.agility,
            this.intelligence,
            this.moveSpeed,
            this.attackSpeed,
            this.attackRange,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate,
            new List<string>(this.dropItems), // 드롭 아이템 복사
            this.experienceReward,
            this.goldReward
        );

        return clone;
    }
    
    public override string ToStringForTMPro()
    {
        string baseInfo = base.ToStringForTMPro();
        string monsterInfo = string.Format(
            "<color=red>Experience Reward:</color> {0}\n" +
            "<color=yellow>Gold Reward:</color> {1}\n" +
            "<color=lime>Drop Items:</color> {2}\n",
            experienceReward,                // 0: 경험치 보상
            goldReward,                      // 1: 골드 보상
            string.Join(", ", dropItems)     // 2: 드롭 아이템 목록
        );

        return baseInfo + monsterInfo;
    }

}

[Serializable]
public class BossData : MonsterData
{
    public List<string> SpecialSkills { get; private set; } // 보스의 특수 스킬

    public BossData(
        string name,
        GameObject prefab,
        int strength,
        int vitality,
        int agility,
        int intelligence,
        float speed,
        float attackSpeed,
        float attackRange,
        float stamina,
        float staminaRecoveryRate,
        float mpRecoveryRate,
        List<string> dropItems,
        int experienceReward,
        int goldReward,
        List<string> specialSkills)
        : base(name, CharacterType.Boss, prefab, strength, vitality, agility, intelligence, speed, attackSpeed,
            attackRange, stamina, staminaRecoveryRate, mpRecoveryRate, dropItems, experienceReward, goldReward)
    {
        SpecialSkills = new List<string>(specialSkills);
    }


    public new BossData Clone()
    {
        // BossData의 고유 속성 및 MonsterData의 공통 속성을 포함한 클론 생성
        var clone = new BossData(
            this.characterName,
            this.characterPrefab,
            this.strength,
            this.vitality,
            this.agility,
            this.intelligence,
            this.moveSpeed,
            this.attackSpeed,
            this.attackRange,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate,
            new List<string>(this.dropItems), // 드롭 아이템 복사
            this.experienceReward,
            this.goldReward,
            new List<string>(this.SpecialSkills) // 특수 스킬 복사
        );

        // MonsterData에서 관리되는 속성 복사
        clone.level = this.level; // 레벨 복사
        clone.currentExperience = this.currentExperience; // 현재 경험치 복사
        clone.experienceToLevelUp = this.experienceToLevelUp; // 레벨업 경험치 복사
        clone.currentHp = this.currentHp; // 현재 HP 복사
        clone.currentMp = this.currentMp; // 현재 MP 복사
        clone.staminaCurrent = this.staminaCurrent; // 현재 스태미나 복사

        // StatModifier 복사
        clone.statModifier = this.statModifier?.Clone();

        return clone;
    }
    
    public override string ToStringForTMPro()
    {
        // 부모 클래스의 ToStringForTMPro 호출
        string baseInfo = base.ToStringForTMPro();
    
        // 보스 고유의 특수 스킬 추가
        string bossInfo = string.Format(
            "<color=red>Special Skills:</color> {0}\n",
            SpecialSkills.Count > 0 ? string.Join(", ", SpecialSkills) : "None" // 특수 스킬 목록 표시
        );
    
        return baseInfo + bossInfo;
    }

}

// 드래곤 관련
[Serializable]
public class DragonData
{
    public string characterName;
    public CharacterType characterType;
    public GameObject prefab;
    
    public int strength;       // 힘
    public int agility;        // 민첩
    public int vitality;       // 건강
    public int intelligence;   // 지능

    public float speed;        // 이동 속도
    public float attackSpeed;  // 공격 속도
    public float attackRange;  // 기본 공격 범위

    public int bondLevel;      // 유대 레벨
    public int bondExperience; // 유대 경험치
    public int[] bondThresholds; // 레벨 업에 필요한 유대 경험치
    
    private bool isLevelingUp = false; // 레벨업 진행 중 여부

    // 모디파이어
    public DragonStatModifier statModifier;
    private DragonStatModifier baseStatModifier = new DragonStatModifier();
    
    // 물리 데미지와 마법 데미지 계산을 위한 필드 추가
    public int physicalDamage;  // 물리 공격력
    public int magicDamage;     // 마법 공격력
    public float criticalChance; // 크리티컬 확률
    public float criticalDamage; // 크리티컬 데미지 배율


    public DragonData(
        string name, 
        CharacterType type,
        GameObject prefab, 
        int strength, 
        int agility, 
        int vitality, 
        int intelligence, 
        float speed, 
        float attackSpeed,
        float attackRange,
        int[] bondThresholds, 
        float criticalChance, 
        DragonStatModifier modifier = null)
    {
        this.characterName = name;
        this.characterType = type;
        this.prefab = prefab;
        this.strength = strength;
        this.agility = agility;
        this.vitality = vitality;
        this.intelligence = intelligence;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.bondLevel = 1;
        this.bondExperience = 0;
        this.bondThresholds = bondThresholds;
        this.criticalChance = criticalChance;
        this.statModifier = modifier ?? new DragonStatModifier();
        
        // 물리 데미지와 마법 데미지 계산
        UpdateDerivedStats();
    }
    
    // 물리 데미지와 마법 데미지 계산
    public void UpdateDerivedStats()
    {
        physicalDamage = Mathf.RoundToInt(Mathf.Max(strength * statModifier.strengthMultiplier, 1f)); // 기본값을 1f로 설정하여 0 방지
        magicDamage = Mathf.RoundToInt(Mathf.Max(intelligence * statModifier.intelligenceMultiplier, 1f)); // 기본값을 1f로 설정
        criticalChance = Mathf.Min(agility * statModifier.agilityMultiplier, 1f); // 민첩성에 따른 크리티컬 확률
        criticalDamage = 1.5f; // 크리티컬 데미지 배율 예시 (게임에 맞게 수정 가능)
    }
    
    // 유대 경험치 증가
    public void AddBondExperience(int amount)
    {
        if (amount < 0) return;
        
        bondExperience += amount;

        while (bondLevel - 1 < bondThresholds.Length && bondExperience >= bondThresholds[bondLevel - 1])
        {
            bondExperience -= bondThresholds[bondLevel - 1];
            LevelUpBond();
        }
    }

    private void LevelUpBond()
    {
        isLevelingUp = true; // 레벨업 시작
        
        // 레벨 증가
        bondLevel++;
        
        strength++;
        vitality++;
        agility++;
        intelligence++;
        
        UpdateDerivedStats();
        Debug.Log($"{characterName}의 유대 레벨이 {bondLevel}로 상승했습니다!");
    }
}

[Serializable]
public class DragonStatModifier
{
    public float strengthMultiplier = 1.5f;      // 힘에 의한 물리 공격력 배율
    public float agilityMultiplier = 0.01f;      // 민첩성에 의한 크리티컬 확률 배율
    public float intelligenceMultiplier = 1.5f;  // 지능에 의한 마법 공격력 배율
    
    // 클론 메서드
    public DragonStatModifier Clone()
    {
        return new DragonStatModifier
        {
            strengthMultiplier = this.strengthMultiplier,
            agilityMultiplier = this.agilityMultiplier,
            intelligenceMultiplier = this.intelligenceMultiplier,
        };
    }
}


