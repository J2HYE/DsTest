using UnityEngine;
using TMPro;

public class QuestDistanceCheck : MonoBehaviour
{
    private Quest quest;
    private TextMeshProUGUI distanceText;

    public void SetQuest(Quest newQuest)
    {
        quest = newQuest;
    }

    public void SetUIText(TextMeshProUGUI text)
    {
        distanceText = text;
    }

    private void Update()
    {
        if (quest == null || distanceText == null) return;

        string distanceColor = string.Empty;
        string questDescription = quest.description;
        float distance = 0f;
        foreach (var condition in quest.requiredConditions)
        {
            string keyWord = condition.Key;
            QuestCondition questCondition = condition.Value;

            if (questCondition.isCompleted)
            {
                continue;
            }

            if (keyWord.Contains("location"))
            {
                CompassIndicater.AddTarget(QuestManager.GetQuestConditionPoint(keyWord));
                distance = Vector3.Distance(GameManager.playerTransform.position, QuestManager.GetQuestConditionPoint(keyWord).position);
                string color = QuestManager.GetDistanceColor(distance);
                distanceColor = $"     <color={color}>{distance:F1}m</color>";
            }
        }
        distanceText.text = questDescription + distanceColor;
        Canvas.ForceUpdateCanvases();
    }
}
