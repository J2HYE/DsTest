using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] private SpawnnerList spawnnerLists;    // 관리할 스포너 리스트
    [SerializeField] private TextMeshProUGUI uiText;                   // UI 텍스트
    [SerializeField] private List<NPCSpawner> npcSpawnerList = new List<NPCSpawner>();
    [SerializeField] private List<NPCSpawner> bossSpawnerList = new List<NPCSpawner>();
    [SerializeField] private List<NPCSpawner> monsterSpawnerList = new List<NPCSpawner>();

    private float uiUpdateInterval = 0.5f;
    private float uiTimer = 0f;

    public static SpawnManager GetInstance()
    {
        if (Instance == null)
        {
            var instances = Resources.FindObjectsOfTypeAll<SpawnManager>();
            if (instances.Length == 0)
            {
                Debug.LogError("SpawnManager 인스턴스가 존재하지 않습니다.");
                return null;
            }
            else if (instances.Length > 1)
            {
                Debug.LogWarning($"SpawnManager가 여러 개 발견되었습니다. 첫 번째 인스턴스를 사용합니다.");
            }
            Instance = instances[0];
        }
        return Instance;
    }

    private void Awake()
    {
        npcSpawnerList.Clear();
        bossSpawnerList.Clear();
        monsterSpawnerList.Clear();
        
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        foreach (var spawnData in spawnnerLists.spawnNPCDataLists)
        {
            if (!npcSpawnerList.Exists(spawner => spawner.SpawnData == spawnData))
            {
                CreateSpawnerFromData(spawnData);
            }
        }

        foreach (var spawnData in spawnnerLists.spawnMonsterDataLists)
        {
            if (!monsterSpawnerList.Exists(spawner => spawner.SpawnData == spawnData))
            {
                CreateSpawnerFromData(spawnData);
            }
        }

        foreach (var spawnData in spawnnerLists.spawnBossDataLists)
        {
            if (!npcSpawnerList.Exists(spawner => spawner.SpawnData == spawnData))
            {
                CreateSpawnerFromData(spawnData);
            }
        }
    }

    private void CreateSpawnerFromData(SpawnData spawnData)
    {
        GameObject spawnerObject = new GameObject(spawnData.spwanName);
        spawnerObject.transform.position = spawnData.spawnPosition;
        NPCSpawner newSpawner = spawnerObject.AddComponent<NPCSpawner>();
        newSpawner.SpawnData = spawnData;
        switch (spawnData.spwanType)
        {
            case SpawnnerType.None:
                break;
            case SpawnnerType.Npc:
                npcSpawnerList.Add(newSpawner);
                break;
            case SpawnnerType.Monster:
                monsterSpawnerList.Add(newSpawner);
                break;
            case SpawnnerType.Boss:
                bossSpawnerList.Add(newSpawner);
                break;
        }
        Debug.Log($"스폰 데이터 '{spawnData.spwanName}'에서 새로운 스포너 생성.");
    }

    private void Update()
    {
        uiTimer += Time.deltaTime;
        if (uiTimer >= uiUpdateInterval)
        {
            uiTimer = 0f;
            UpdateUIText();
        }
    }

    private void UpdateUIText()
    {
        if (uiText != null)
        {
            int totalCount = npcSpawnerList.Count + monsterSpawnerList.Count + bossSpawnerList.Count;
            string uiStatus = $"스폰 정보 ( 스폰갯수 : <color=green>{totalCount}</color>개 )\n";
            uiStatus += npcSpawnerList.Count > 0 ? $"========== NPC 스폰 ==========\n" : "";
            foreach (var spawner in npcSpawnerList)
            {
                float distance = Vector3.Distance(GameManager.playerTransform.position, spawner.SpawnData.spawnPosition);

                string distanceColor;
                if (distance < 60)
                    distanceColor = "green";
                else if (distance < 100)
                    distanceColor = "yellow";
                else
                    distanceColor = "red";

                uiStatus += $"<color=#00FFFF>{spawner.SpawnData.spwanName}</color>  ({spawner.ActiveObjectCount})" +
                            $"-  <color=yellow>{spawner.SpawnType}</color>  " +
                            $"- <color={distanceColor}>{distance:F1}m</color>\n";
            }
            uiStatus += monsterSpawnerList.Count > 0 ? $"\n========== 몬스터 스폰 ==========\n" : "";
            foreach (var spawner in monsterSpawnerList)
            {
                float distance = Vector3.Distance(GameManager.playerTransform.position, spawner.SpawnData.spawnPosition);

                // 거리별 컬러 지정
                string distanceColor;
                if (distance < 60)
                    distanceColor = "green";
                else if (distance < 100)
                    distanceColor = "yellow";
                else
                    distanceColor = "red";

                uiStatus += $"<color=#00FFFF>{spawner.SpawnData.spwanName}</color>  ({spawner.ActiveObjectCount})" +
                            $"-  <color=yellow>{spawner.SpawnType}</color>  " +
                            $"- <color={distanceColor}>{distance:F1}m</color>\n";
            }
            uiStatus += bossSpawnerList.Count > 0 ? $"\n========== 보스 스폰 ==========\n" : "";
            foreach (var spawner in bossSpawnerList)
            {
                float distance = Vector3.Distance(GameManager.playerTransform.position, spawner.SpawnData.spawnPosition);

                // 거리별 컬러 지정
                string distanceColor;
                if (distance < 60)
                    distanceColor = "green";
                else if (distance < 100)
                    distanceColor = "yellow";
                else
                    distanceColor = "red";

                uiStatus += $"<color=#00FFFF>{spawner.SpawnData.spwanName}</color>  ({spawner.ActiveObjectCount})" +
                            $"-  <color=yellow>{spawner.SpawnType}</color>  " +
                            $"- <color={distanceColor}>{distance:F1}m</color>\n";
            }
            uiText.text = uiStatus;
        }
    }


    public void AddNPCSpawner(NPCSpawner nPCSpawner)
    {
        switch (nPCSpawner.SpawnType)
        {
            case SpawnnerType.Monster:
                if (!monsterSpawnerList.Contains(nPCSpawner))
                {
                    monsterSpawnerList.Add(nPCSpawner);
                }
                if (spawnnerLists != null && !spawnnerLists.spawnMonsterDataLists.Contains(nPCSpawner.SpawnData))
                {
                    spawnnerLists.spawnMonsterDataLists.Add(nPCSpawner.SpawnData);
                }
                Debug.Log($"NPCSpawner '{nPCSpawner.SpawnData.spwanName}'가 SpawnManager에 등록되었습니다.");
                break;
            case SpawnnerType.Npc:
                if (!npcSpawnerList.Contains(nPCSpawner))
                {
                    npcSpawnerList.Add(nPCSpawner);
                }
                if (spawnnerLists != null && !spawnnerLists.spawnNPCDataLists.Contains(nPCSpawner.SpawnData))
                {
                    spawnnerLists.spawnNPCDataLists.Add(nPCSpawner.SpawnData);
                }
                Debug.Log($"NPCSpawner '{nPCSpawner.SpawnData.spwanName}'가 SpawnManager에 등록되었습니다.");
                break;
            case SpawnnerType.Boss:
                if (!bossSpawnerList.Contains(nPCSpawner))
                {
                    bossSpawnerList.Add(nPCSpawner);
                }
                if (spawnnerLists != null && !spawnnerLists.spawnBossDataLists.Contains(nPCSpawner.SpawnData))
                {
                    spawnnerLists.spawnBossDataLists.Add(nPCSpawner.SpawnData);
                }
                Debug.Log($"NPCSpawner '{nPCSpawner.SpawnData.spwanName}'가 SpawnManager에 등록되었습니다.");
                break;
            case SpawnnerType.None:
                break;
        }
    }
}
