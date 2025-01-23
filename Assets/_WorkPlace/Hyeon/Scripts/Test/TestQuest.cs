using UnityEngine;

[CreateAssetMenu(fileName = "TestQuest", menuName = "Scriptable Objects/TestQuest")]
public class TestQuest : ScriptableObject
{
    public string questTitle;
    public string questDescription;
    public float expReward;
    public float moneyReward;
}
