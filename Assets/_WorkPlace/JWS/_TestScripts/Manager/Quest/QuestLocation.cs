using UnityEngine;
using UnityEngine.InputSystem;

public class QuestLocation : MonoBehaviour
{
    private bool isQuestUpdated = false;

    private void Start()
    {
        if (!QuestManager.QuestConditionPoint.ContainsKey(this.gameObject.name))
        {
            QuestManager.QuestConditionPoint.Add(this.gameObject.name, this.transform);
        }
        else
        {
            Debug.LogError($"이미 등록된 퀘스트 포인트입니다: {this.gameObject.name}");
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float distance = Vector3.Distance(this.transform.position, other.transform.position);

            if (distance <= 1f && !isQuestUpdated)
            {
                isQuestUpdated = true;
                QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Explore, this.gameObject.name);
                CompassIndicater.RemoveTarget(QuestManager.GetQuestConditionPoint(this.gameObject.name));
            }
        }
    }
}
