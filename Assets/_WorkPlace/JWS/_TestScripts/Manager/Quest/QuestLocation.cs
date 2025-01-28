using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    private void Start()
    {
        if (!QuestManager.QuestConditionPoint.ContainsKey(this.gameObject.name))
        {
            QuestManager.QuestConditionPoint.Add(this.gameObject.name, this.transform.position);
        }
        else
        {
            Debug.LogError("이미 등록된 포인트 입니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name);
            QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Explore, this.gameObject.name);
        }
    }
}
