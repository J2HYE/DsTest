using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name);
            QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Explore, this.gameObject.name);
        }
    }
}
