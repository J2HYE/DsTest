using System;
using System.Collections.Generic;
using UnityEngine;

// 250123 11:20AM sohyeon 모든 캐릭터데이터에 mpRecoveryRate 초기화 부분 추가함!!
// CharacterData 클래스 정의: 캐릭터의 스탯, 레벨, 경험치 등을 관리
[Serializable]
public class CharacterData
{
    public string characterName;  // 캐릭터 이름
    public CharacterType characterType;  // 캐릭터 타입 추가
    public GameObject prefab; // 몬스터 프리팹
    
    // 기본 스텟 (정수형)
    public int strength;     // 힘
    public int agility;      // 민첩
    public int vitality;     // 체력
    public int intelligence; // 지능
    
    // 이동 관련 스텟
    public float speed;      // 스피드 (이동 속도)
    public float attackSpeed; // 어택 스피드 (초당 공격 횟수)

    // 스태미나 관련
    public float stamina;     // 최대 스태미나
    public float staminaCurrent; // 현재 스태미나
    public float staminaRecoveryRate; // 스태미나 회복 속도
    public float mpRecoveryRate;    // 마나 회복 속도
    
    // 레벨과 경험치 관련 변수 추가
    public int level;             // 레벨
    [HideInInspector] public int currentExperience; // 현재 경험치
    [HideInInspector] public int experienceToLevelUp; // 레벨업에 필요한 경험치
    
    // 계산된 값 (정수형)
    [HideInInspector] public int maxHp;        // 최대 HP
    public int currentHp;    // 현재 HP
    [HideInInspector] public int maxMp;        // 최대 HP
    public int currentMp;    // 현재 HP
    [HideInInspector] public int physicalDefense;  // 물리 방어력
    [HideInInspector] public int magicDefense;     // 마법 방어력
    [HideInInspector] public int physicalDamage;   // 물리 공격력
    [HideInInspector] public int magicDamage;      // 마법 공격력
    [HideInInspector] public float criticalChance; // 크리티컬 확률 (float)
    [HideInInspector] public int baseDamage;       // 기본 물리 공격력 (정수형)
    
    // StatModifier 설정 (각 캐릭터마다 별도로 설정)
    [HideInInspector] public StatModifier statModifier;
    
    // 스탯 Min & Max 관련 상수 - 추후 상수만 정리한 클래스 추가 필요
    private const int maxStats = 999;
    private const int minStats = 0;

    private Dictionary<string, int> stats;
    private StatModifier basestatModifier = new StatModifier();
    
    public event Action<Transform> OnTakeDamage; // 피격 했는지 이벤트 전달

    // 생성자: 캐릭터 초기화 및 자동 계산
    public CharacterData(string name, CharacterType type, GameObject prefab, 
        int strength, int agility, int vitality, int intelligence, StatModifier modifier, float speed, 
        float attackSpeed, float stamina, float staminaRecoveryRate, float mpRecoveryRate)
    {
        this.characterName = name;
        this.characterType = type;  // 타입 저장
        this.prefab = prefab; // 프리팹 설정
        this.strength = strength;
        this.agility = agility;
        this.vitality = vitality;
        this.intelligence = intelligence;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.stamina = stamina;
        this.staminaCurrent = stamina; // 초기 스태미나는 최대 스태미나와 동일
        this.staminaRecoveryRate = staminaRecoveryRate;
        this.mpRecoveryRate = mpRecoveryRate;
        this.statModifier = modifier != null ? modifier :basestatModifier;
        this.level = 1; // 초기 레벨은 1
        this.currentExperience = 0; // 초기 경험치는 0
        experienceToLevelUp = CalculateExperienceToLevelUp(); // 첫 번째 레벨업에 필요한 경험치 설정

        // 스텟 값에 따라 자동 계산
        // IncreaseStatsBasedOnLevel();
        UpdateDerivedStats();  // 모든 파생 스탯을 한 번에 계산
        this.currentHp = this.maxHp;
        this.currentMp = this.maxMp;
        this.staminaCurrent = this.stamina;
        InitializeStats();
    }

    public void InitializeStats()
    {
        stats = new Dictionary<string, int>
        {
            { "strength", strength },
            { "vitality", vitality },
            { "agility", agility },
            { "intelligence", intelligence }
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

    // 파생 스탯 계산 (중복을 줄이기 위해 한 번에 계산)
    public void UpdateDerivedStats()
    {
        int previousMaxHp = maxHp; // 이전 최대 체력 저장
        int previousMaxMp = maxMp; // 이전 최대 MP 저장
    
        // 체력, MP 등 파생 스탯 계산
        maxHp = Mathf.RoundToInt(vitality * statModifier.vitalityMultiplier);  // 체력에 비례한 최대 HP
        maxMp = Mathf.RoundToInt((intelligence * statModifier.mpMultiplier) + (level * statModifier.levelMpBonus)); // MP 계산
    
        // 공격력 및 방어력 계산
        physicalDamage = Mathf.RoundToInt(strength * statModifier.strengthMultiplier);  // 힘에 따른 물리 공격력
        physicalDefense = Mathf.RoundToInt((strength + vitality) * statModifier.physicalDefenseMultiplier);  // 힘과 체력에 비례한 물리 방어력
        magicDamage = Mathf.RoundToInt(intelligence * statModifier.intelligenceMultiplier);  // 지능에 따른 마법 공격력
        magicDefense = Mathf.RoundToInt(intelligence * statModifier.magicDefenseMultiplier);  // 지능에 따른 마법 방어력

        // 크리티컬 확률 및 기본 데미지 계산
        criticalChance = Mathf.Min(agility * statModifier.agilityMultiplier, 1f);  // 민첩성에 따른 크리티컬 확률
        baseDamage = physicalDamage + strength;  // 물리 공격력 + 힘

        // 체력 증가 처리
        if (maxHp > previousMaxHp)
        {
            currentHp += maxHp - previousMaxHp;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        }
    
        // MP 증가 처리
        if (maxMp > previousMaxMp)
        {
            currentMp += maxMp - previousMaxMp;
            currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        }
    }
    
    // 경험치를 얻었을 때 호출되는 함수
    public void AddExperience(int amount)
    {
        if (amount < 0) amount = 0; // 경험치가 음수일 경우 0으로 처리

        currentExperience += amount;

        // 경험치가 레벨업에 도달했으면 반복적으로 레벨업 처리
        while (currentExperience >= experienceToLevelUp)
        {
            LevelUp();
        }
    }

    // 레벨업 처리 함수
    private void LevelUp()
    {
        level++;  // 레벨 증가
        currentExperience -= experienceToLevelUp;  // 잔여 경험치 유지
        experienceToLevelUp = CalculateExperienceToLevelUp();  // 새로운 레벨에 맞는 경험치 계산

        // 레벨업 시 능력치 증가 및 파생 스탯 계산
        IncreaseStatsBasedOnLevel();
        UpdateDerivedStats();
        
        currentHp = this.maxHp;
        currentMp = this.maxMp;
        staminaCurrent = this.stamina;

        // 레벨업 시 로그 출력 (디버깅용)
        Debug.Log($"레벨업! 현재 레벨: {level}, 남은 경험치: {currentExperience}");
        ToStringForTMPro();
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
        // 레벨에 따라 각 스탯을 증가
        strength += level <= 10 ? 2 : 1;
        vitality += level <= 10 ? 5 : 3;
        agility += 1;  // 민첩은 항상 1 증가
        intelligence += 1;  // 지능도 항상 1 증가

        // 스탯 변경 후 파생 스탯 업데이트
        UpdateDerivedStats();
    }

    // 크리티컬 여부를 판단하여 데미지 반환
    public int CalculateDamage(bool isCritical)
    {
        return isCritical ? baseDamage * 2 : baseDamage;  // 크리티컬이면 데미지 2배, 아니면 기본 데미지
    }
    
    // 특정 스텟 증가 함수
    public void ModifyStat(string statType, int amount)
    {
        if (!stats.ContainsKey(statType)) return;

        stats[statType] = Mathf.Clamp(stats[statType] + amount, minStats, maxStats);
        strength = stats["strength"];
        vitality = stats["vitality"];
        agility = stats["agility"];
        intelligence = stats["intelligence"];

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
        currentHp += amount;
        currentHp = Mathf.Min(currentHp, maxHp);  // currentHp가 maxHp를 초과하지 않도록 처리
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

    // ToString()을 오버라이드하여 TMP로 출력할 수 있는 형식으로 정보 제공
    // CharacterData 클래스
    public string ToStringForTMPro()
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
            "<color=black>Base Damage:</color> {18}\n" +
            "<color=teal>Speed:</color> {19}\n" +
            "<color=yellow>Attack Speed:</color> {20}\n" +
            "<color=orange>Current Experience:</color> {21}\n" +
            "<color=orange>Experience To Level Up:</color> {22}\n",
            characterName,                      // 0
            level,                              // 1
            strength,                           // 2
            agility,                            // 3
            vitality,                           // 4
            intelligence,                       // 5
            currentHp,                          // 6
            maxHp,                              // 7
            currentMp,                          // 8
            maxMp,                              // 9
            staminaCurrent,                     // 10
            stamina,                            // 11
            staminaRecoveryRate,                // 12
            physicalDamage,                     // 13
            magicDamage,                        // 14
            physicalDefense,                    // 15
            magicDefense,                       // 16
            criticalChance * 100,               // 17
            baseDamage,                         // 18
            speed,                              // 19
            attackSpeed,                        // 20
            currentExperience,                  // 21
            experienceToLevelUp                 // 22
        );

        // 플레이어 전용 정보
        if (this is PlayerData player)
        {
            baseInfo += string.Format(
                "<color=yellow>Gold:</color> {0}\n" +
                "<color=green>Skills:</color> {1}\n",
                player.gold,
                string.Join(", ", player.skills)
            );
        }

        // 몬스터 전용 정보
        if (this is MonsterData monster)
        {
            baseInfo += string.Format(
                "<color=yellow>Drop Items:</color> {0}\n" +
                "<color=orange>Experience Reward:</color> {1}\n" +
                "<color=orange>Gold Reward:</color> {2}\n",
                string.Join(", ", monster.dropItems),
                monster.experienceReward,
                monster.goldReward
            );
        }

        // 보스 전용 정보
        if (this is BossData boss)
        {
            baseInfo += string.Format(
                "<color=red>Special Skills:</color> {0}\n",
                string.Join(", ", boss.specialSkills)
            );
        }

        return baseInfo;
    }
    
    public CharacterData Clone()
    {
        return new CharacterData(
            this.characterName,
            this.characterType,
            this.prefab,
            this.strength,
            this.agility,
            this.vitality,
            this.intelligence,
            this.statModifier != basestatModifier ? statModifier.Clone() : basestatModifier,
            this.speed,
            this.attackSpeed,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate
        );
    }
}

public class StatModifier
{
    public float strengthMultiplier = 2f;     // 힘에 대한 공격력 증가 비율
    public float vitalityMultiplier = 10f;    // 체력에 대한 HP 증가 비율
    public float agilityMultiplier = 0.01f;  // 민첩성에 대한 크리티컬 확률 비율
    public float intelligenceMultiplier = 3f; // 지능에 대한 마법 공격력 비율
    public float mpMultiplier = 10f;          // 지능에 대한 MP 증가 비율
    public float levelMpBonus = 5f;           // 레벨에 따른 추가 MP 보너스

    // 방어력 계산 비율 (동적 설정)
    public float physicalDefenseMultiplier = 1f;  // 물리 방어력 계산 비율
    public float magicDefenseMultiplier = 2f;      // 마법 방어력 계산 비율
    public StatModifier Clone()
    {
        return new StatModifier
        {
            strengthMultiplier = this.strengthMultiplier,
            vitalityMultiplier = this.vitalityMultiplier,
            agilityMultiplier = this.agilityMultiplier,
            intelligenceMultiplier = this.intelligenceMultiplier,
            mpMultiplier = this.mpMultiplier,
            levelMpBonus = this.levelMpBonus,
            physicalDefenseMultiplier = this.physicalDefenseMultiplier,
            magicDefenseMultiplier = this.magicDefenseMultiplier
        };
    }
}

// 플레이어 데이터
[Serializable]
public class PlayerData : CharacterData
{
    public List<string> skills = new List<string>();    // 스킬 목록
    public int gold; // 골드 소지량

    public PlayerData(string name, GameObject prefab, int strength, int vitality, int agility, int intelligence,
        float speed, float attackSpeed, float stamina, float staminaRecoveryRate, float mpRecoveryRate)
        : base(name, CharacterType.Player, prefab, strength, agility, vitality, intelligence, null, speed, attackSpeed, stamina, staminaRecoveryRate, mpRecoveryRate)
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

    public void UseGold(int amount)
    {
        if (amount > gold)
        {
            Debug.LogWarning("골드가 부족합니다!");
            return;
        }
        gold -= amount;
        Debug.Log($"골드 사용: {amount}, 남은 골드: {gold}");
    }
    
    public new PlayerData Clone()
    {
        var clone = new PlayerData (
            this.characterName,
            this.prefab,
            this.strength,
            this.vitality,
            this.agility,
            this.intelligence,
            this.speed,
            this.attackSpeed,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate
            )
        {
            gold = this.gold,  // 골드 복사
            skills = new List<string>(this.skills),  // 스킬 리스트 복사
            level = this.level,  // 레벨 복사
            currentExperience = this.currentExperience,  // 현재 경험치 복사
            experienceToLevelUp = this.experienceToLevelUp,  // 레벨업 경험치 복사
            staminaCurrent = this.staminaCurrent,  // 현재 스태미나 복사
            currentHp = this.currentHp,  // 현재 HP 복사
            currentMp = this.currentMp,  // 현재 MP 복사
            statModifier = this.statModifier != null ? this.statModifier.Clone() : null  // StatModifier 복사
        };

        return clone;
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

    public MonsterData(string name, CharacterType characterType, GameObject prefab, int strength, int vitality, int agility,
        int intelligence, float speed, float attackSpeed, float stamina, float staminaRecoveryRate, float mpRecoveryRate, List<string> dropItems,
        int experienceReward, int goldReward)
        : base(name, CharacterType.Monster, prefab, strength, agility, vitality, intelligence, null, speed, attackSpeed, stamina, staminaRecoveryRate, mpRecoveryRate)
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
            this.prefab,
            this.strength,
            this.vitality,
            this.agility,
            this.intelligence,
            this.speed,
            this.attackSpeed,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate,
            this.dropItems,
            this.experienceReward,
            this.goldReward
        );

        clone.level = this.level; // 레벨 복사
        clone.currentExperience = this.currentExperience; // 현재 경험치 복사
        clone.experienceToLevelUp = this.experienceToLevelUp; // 레벨업 경험치 복사
        return clone;
    }
}

[Serializable]
public class BossData : MonsterData
{
    public List<string> specialSkills; // 보스의 특수 스킬
    public BossData(string name, GameObject prefab, int strength, int vitality, int agility,
        int intelligence, float speed, float attackSpeed, float stamina, float staminaRecoveryRate, float mpRecoveryRate,
        List<string> dropItems, int experienceReward, int goldReward, List<string> specialSkills)
        : base(name, CharacterType.Boss, prefab, strength, vitality, agility, intelligence, 
            speed, attackSpeed, stamina, staminaRecoveryRate, mpRecoveryRate, dropItems, experienceReward, goldReward)
    {
        this.specialSkills = new List<string>(specialSkills);
    }
    
    public new BossData Clone()
    {
        var clone = new BossData(
            this.characterName,
            this.prefab,
            this.strength,
            this.vitality,
            this.agility,
            this.intelligence,
            this.speed,
            this.attackSpeed,
            this.stamina,
            this.staminaRecoveryRate,
            this.mpRecoveryRate,
            new List<string>(this.dropItems), // 드롭 아이템 복사
            this.experienceReward,
            this.goldReward,
            new List<string>(this.specialSkills) // 특수 스킬 복사
        );

        // MonsterData의 파생 속성 복사
        clone.level = this.level; // 레벨 복사
        clone.currentExperience = this.currentExperience; // 현재 경험치 복사
        clone.experienceToLevelUp = this.experienceToLevelUp; // 레벨업 경험치 복사
        clone.currentHp = this.currentHp; // 현재 HP 복사
        clone.currentMp = this.currentMp; // 현재 MP 복사
        clone.staminaCurrent = this.staminaCurrent; // 현재 스태미나 복사

        // StatModifier 복사
        clone.statModifier = this.statModifier != null ? this.statModifier.Clone() : null;

        return clone;
    }
}


