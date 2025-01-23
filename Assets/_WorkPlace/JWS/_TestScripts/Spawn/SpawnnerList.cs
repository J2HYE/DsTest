using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnnerList", menuName = "Ds Project/Spawnner List", order = 1)]
public class SpawnnerList : ScriptableObject
{
    public List<SpawnData> spawnMonsterDataLists = new List<SpawnData>();
    public List<SpawnData> spawnNPCDataLists = new List<SpawnData>();
    public List<SpawnData> spawnBossDataLists = new List<SpawnData>();
}