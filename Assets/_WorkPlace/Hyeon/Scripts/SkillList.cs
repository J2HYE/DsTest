using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillList", menuName = "Ds Project/Skill")]
public class SkillList : ScriptableObject
{
    public List<Skills> skillList;
}