using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 상위 타입을 정의
public enum CharacterType
{
    Player,   // 플레이어
    Monster,  // 일반 몬스터
    Boss,     // 보스
    Drogon,   // 펫(용)
    Animal    // 동물 (특정 생태계 NPC 또는 몬스터)
}

// 동물(NPC 또는 비전투 생물)의 세부 유형을 정의
public enum AnimalType
{
    Rabbit,
    Deer,
    Boar,
    Dog
}

// 일반 몬스터의 세부 유형을 정의
public enum MonsterType
{
    Mushroom, // 초보용
    MooGlet, // 초보용
    Slime, // 초보용
    Bear, // 일반 
    ZombieGorilla, // 일반 
    Devil, // 중간급
    Golem,  // 중간급
    Bulldog,  // 중간급
    MophanS,  // 보스
    MophanSub1, // 보스부하
    MophanSub2 // 보스부하
}

// 보스의 세부 유형을 정의
public enum BossType
{
    Mophan      // 중간 보스: 모파안
}

public class CharacterManager : BaseManager<CharacterManager>
{
    // 캐릭터 데이터 리스트 (플레이어 + 몬스터/보스)
    [SerializeField] private List<CharacterData> characterList = new List<CharacterData>();
    public IReadOnlyList<CharacterData> CharacterList => Instance.characterList.AsReadOnly();
    
    // 플레이어 캐릭터 (온리 원)
    public static PlayerData PlayerCharacterData;
    [SerializeField] private PlayerData playercharacterData;
    
    // 용 데이터 관리
    public static DragonData DragonData;
    [SerializeField] private DragonData dragonData; // 활성 드래곤 데이터
    
    // ScriptableObject로부터 로드된 캐릭터 템플릿 리스트
    [SerializeField]
    private CharacterList characterTemplates;
    
    [SerializeField]
    private bool isInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        PlayerCharacterData = characterTemplates.players[0];
        playercharacterData = PlayerCharacterData;
        
        DragonData = characterTemplates.dragons[0];
        dragonData = DragonData;
        
        // 게임 시작시 강제 초기화 원하면 활성화
        InitialCharacter();
        InitialDragon();
    }

    private void Update()
    {
        if (isInitialized)
        {
            isInitialized = false;
            
            InitialCharacter();
            InitialDragon();
            
            playercharacterData = PlayerCharacterData;
            dragonData = DragonData;
        }
    }

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        
    }
    
    public void InitialCharacter()
    {
        PlayerCharacterData.statModifier = new StatModifier();
        
        PlayerCharacterData.characterName = "Hero";
        PlayerCharacterData.characterPrefab = null; // 플레이어 프리팹 (Unity 에디터에서 할당 가능)
        PlayerCharacterData.strength = 10;
        PlayerCharacterData.vitality = 15;
        PlayerCharacterData.agility = 8;
        PlayerCharacterData.intelligence = 12;
        PlayerCharacterData.moveSpeed = 5.0f;
        PlayerCharacterData.attackSpeed = 1.2f;
        PlayerCharacterData.stamina = 100f;
        PlayerCharacterData.staminaRecoveryRate = 0.1f;
        PlayerCharacterData.mpRecoveryRate = 1;
        
        // 파생 스탯 계산
        PlayerCharacterData.UpdateDerivedStats();

        // 레벨 초기화
        PlayerCharacterData.level = 1;
        PlayerCharacterData.currentExperience = 0;
        PlayerCharacterData.experienceToLevelUp = PlayerCharacterData.CalculateExperienceToLevelUp();

        // 골드 초기화
        PlayerCharacterData.gold = 0;
        
        PlayerCharacterData.currentHp = PlayerCharacterData.maxHp;
        playercharacterData.currentMp = PlayerCharacterData.maxMp;
        PlayerCharacterData.staminaCurrent = PlayerCharacterData.stamina;

        
        // Debug.Log(PlayerCharacterData.ToStringForTMPro());
    }
    
    public void InitialDragon()
    {
        dragonData.statModifier = new DragonStatModifier();

        dragonData.characterName = "BabyDragon";
        dragonData.characterType = CharacterType.Drogon;
        dragonData.prefab = null; // 드래곤 프리팹 (Unity 에디터에서 할당 가능)
        dragonData.strength = 5;
        dragonData.vitality = 8;
        dragonData.agility = 4;
        dragonData.intelligence = 6;
        dragonData.speed = 5.0f;
        dragonData.attackSpeed = 3f;
        dragonData.attackRange = 2.5f;

        // 유대 레벨 초기화
        dragonData.bondLevel = 1;
        dragonData.bondExperience = 0;
        dragonData.bondThresholds = new[] { 100, 200, 300, 400, 500 }; // 유대 경험치 필요값 설정
        
        dragonData.UpdateDerivedStats();
        Debug.Log("드래곤 초기화 완료: " + dragonData.characterName);
    }

    // 특정 인덱스의 캐릭터 반환 (플레이어 또는 몬스터)
    public CharacterData GetCharacterFromTemplate(int index)
    {
        if (index >= 0 && index < characterList.Count)
        {
            return characterList[index];
        }
        else
        {
            Debug.LogError("잘못된 캐릭터 인덱스");
            return null;
        }
    }

    // 모든 캐릭터 리스트 반환 (디버깅용)
    public List<CharacterData> GetAllCharacters()
    {
        return characterList;
    }
    
    // 캐릭터 생성 함수: 템플릿을 복사하여 새로운 캐릭터 생성
    public MonsterData CreateCharacterFromTemplate(string Name, string characterName = null)
    {
        // 템플릿에서 이름에 해당하는 캐릭터 데이터 검색
        MonsterData template = characterTemplates.monsters.Find(c => c.characterName == Name);
        // Debug.Log(template.ToStringForTMPro() + "몬스터 생성 전 스텟");

        // 템플릿을 메모리에서 복제 (독립적인 인스턴스 생성)
        MonsterData cloned = template.Clone();
        cloned.InitializeStats();
        cloned.UpdateDerivedStats();

        // 복제된 템플릿의 이름을 변경 (옵션)
        if (!string.IsNullOrEmpty(characterName))
        {
            cloned.characterName = characterName;
        }

        // 리스트에 복제된 캐릭터 추가 (필요 시)
        characterList.Add(cloned);

        Debug.Log($"캐릭터 템플릿 '{cloned.characterName}' 생성 완료.");
        return cloned;
    }
    
    // 몬스터를 지정한 위치에 여러 개 소환하는 함수 - 풀링 적용 X
    public void SpawnMonsters(string monsterName, int spawnCount, Vector3 spawnCenter, float spawnRange)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // 랜덤한 위치 계산
            Vector3 spawnPosition = spawnCenter + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0f,  // Y 값은 고정 (필요에 따라 수정 가능)
                Random.Range(-spawnRange, spawnRange)
            );
    
            // 몬스터 생성
            SpawnMonster(monsterName, spawnPosition);
            Debug.Log($"몬스터 '{monsterName}' 스폰 위치: {spawnPosition}");
        }
    }

    // 단일 몬스터 생성 함수 (템플릿 기반) - 풀링 적용 X
    public void SpawnMonster(string templateName, Vector3 spawnPosition)
    {
        MonsterData monster = CreateCharacterFromTemplate(templateName);

        if (monster != null)
        {
            Debug.Log($"몬스터 '{monster.characterName}' 생성 완료. 스폰 위치: {spawnPosition}");
            GameObject monsterInstance = Instantiate(monster.characterPrefab, spawnPosition, Quaternion.identity);
            // 생성된 인스턴스를 MonsterData에 연결
            monster.instance = monsterInstance;
            // 필요한 컴포넌트 추가 및 초기화
            var testComponent = monsterInstance.AddComponent<Test1>();
            testComponent.monster = monster;
        }
    }
    
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    // 몬스터 생성 함수 (템플릿 기반) - 풀링
    public GameObject CreatMonster(string templateName, Transform parent)
    {
        MonsterData monster = CreateCharacterFromTemplate(templateName);
        if (monster != null)
        {
            GameObject monsterInstance = Instantiate(monster.characterPrefab, parent);
            monster.instance = monsterInstance;
            var testComponent = monsterInstance.AddComponent<Test1>();
            testComponent.monster = monster;
            return monsterInstance;
        }
        return null;
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 


    // 몬스터 처치 처리
    public void OnMonsterDefeated(MonsterData monster, Vector3 position)
    {
        // AI 상태를 Dead로 변경
        if (monster.instance != null)
        {
            // BaseMonsterAI 검색 및 처리
            BaseMonsterAI baseAI = monster.instance.GetComponent<BaseMonsterAI>();
            
            if (monster.instance.transform.parent != null) // 부모가 있는지 확인
            {
                if (baseAI != null)
                {
                    baseAI.SetDeadState(true);
                }
            }
            else
            {
                baseAI.SetDeadState(false); // 부모가 없으면 파괴
            }
        }
        
        // 경험치와 골드 보상 처리
        if (PlayerCharacterData != null)
        {
            PlayerCharacterData.AddExperience(monster.experienceReward);
            PlayerCharacterData.AddGold(monster.goldReward);
            UIManager.SystemGameMessage($"{monster.characterName} 처치! 경험치 +{monster.experienceReward}, 골드 +{monster.goldReward}", MessageTag.아이템_획득);
        }
        
        // 아이템 드롭
        ItemManager.Instance.SpawnItemBox(position + new Vector3(0, 1f, 0), monster, false);
        
        characterList.Remove(monster);
    }
    
    // 가장 가까운 몬스터를 찾는 함수
    public MonsterData GetNearestMonster(Transform dragonTransform, float detectRange = 10f)
    {
        // 가까운 몬스터를 찾기 위한 초기값 설정
        MonsterData closestMonster = null;
        float closestDistance = float.MaxValue;

        // 모든 몬스터 리스트에서 가장 가까운 몬스터를 찾기
        foreach (var character in characterList)
        {
            // 몬스터만 필터링
            if (character is MonsterData monster)
            {
                // 드래곤과 몬스터 간의 거리 계산
                float distanceToMonster = Vector3.Distance(dragonTransform.position, monster.characterPrefab.transform.position);

                // 탐지 범위 내에서 가장 가까운 몬스터를 찾음
                if (distanceToMonster < closestDistance && distanceToMonster <= detectRange)
                {
                    closestDistance = distanceToMonster;
                    closestMonster = monster;
                }
            }
        }

        return closestMonster; // 가장 가까운 몬스터 반환
    }
}