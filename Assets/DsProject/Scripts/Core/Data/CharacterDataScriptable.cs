using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterList", menuName = "Ds Project/Character List")]
public class CharacterList : ScriptableObject
{
    public List<PlayerData> players = new List<PlayerData>();
    public List<MonsterData> monsters = new List<MonsterData>();
    public List<DragonData> dragons = new List<DragonData>(); // 드래곤 데이터 리스트
}
