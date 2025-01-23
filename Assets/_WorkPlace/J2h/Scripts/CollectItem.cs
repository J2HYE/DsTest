using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{itemName}을(를) 획득했습니다!");
            Destroy(gameObject); // 아이템 제거
        }
    }
}
