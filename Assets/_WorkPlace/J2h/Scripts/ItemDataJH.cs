using UnityEngine;

[System.Serializable]
public class ItemDataJH 
{
    public GameObject itemPrefab;   //드롭 아이템 프리팹
    public int dropCount = 1;       //드롭 아이템 개수
    public float dropChance = 1.0f; // 드롭 확률 (0.0 ~ 1.0)
}
