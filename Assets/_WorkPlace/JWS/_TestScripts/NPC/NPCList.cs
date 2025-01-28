using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCList", menuName = "Ds Project/NPC List")]
public class NPCList : ScriptableObject
{
    public List<NPCData> mainQuestNpcLists; // 여러 NPC 데이터를 관리하는 리스트
    public List<NPCData> shopNpcLists; // 여러 NPC 데이터를 관리하는 리스트
    public List<NPCData> npcLists; // 여러 NPC 데이터를 관리하는 리스트
}