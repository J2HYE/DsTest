using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestList", menuName = "Ds Project/QuestList")]
public class QuestList : ScriptableObject
{
    public List<Quest> mainQuestList = new List<Quest>();
    public List<Quest> questList = new List<Quest>();
}