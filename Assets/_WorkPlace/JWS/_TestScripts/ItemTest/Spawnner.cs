using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] spawnableItems;
    [SerializeField] private float spawnInterval = 10f;
    private int maxSpawnsPerCycle = 1; // 주기당 최대 스폰 개수

    private Dictionary<Transform, GameObject> spawnItemsList = new Dictionary<Transform, GameObject>();

    void Start()
    {
        foreach (var point in spawnPoints)
        {
            spawnItemsList[point] = null;
        }
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    void Spawn()
    {
        int spawnCount = 0;

        foreach (var point in new List<KeyValuePair<Transform, GameObject>>(spawnItemsList))
        {
            if (point.Value == null && spawnCount < maxSpawnsPerCycle)
            {
                GameObject randomItem = spawnableItems[Random.Range(0, spawnableItems.Length)];
                GameObject spawnedItem = Instantiate(randomItem, point.Key.position + point.Key.up, Quaternion.identity);
                spawnedItem.transform.localScale = Vector3.one / 2;
                spawnItemsList[point.Key] = spawnedItem;
                spawnCount++;
            }
        }

        if (spawnCount == 0)
        {
            Debug.Log("[Spawner] 모든 스폰 지점에 아이템이 있어 이번 스폰은 건너뜁니다.");
        }
    }

}
