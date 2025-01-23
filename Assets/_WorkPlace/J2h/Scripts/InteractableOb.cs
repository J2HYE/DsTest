using NUnit.Framework.Interfaces;
using UnityEngine;

public abstract class InteractableOb : MonoBehaviour
{
    protected int hitCount = 0; // 현재 타격 수
    public int maxHits = 3;     // 부서지기까지 필요한 타격 수

    [Header("Drop Settings")]
    public ItemDataJH[] dropItems; // 드롭할 아이템 목록

    public virtual void Interact(string toolTag)
    {
        hitCount++;
        Debug.Log($"현재 타격 수: {hitCount}/{maxHits}");

        if (hitCount >= maxHits)
        {
            DropItems();  // 아이템 드롭
            DestroyObject(); // 오브젝트 파괴
        }
    }

    protected virtual void DestroyObject()
    {
        Debug.Log($"{gameObject.name}이(가) 부서졌습니다!");
        Destroy(gameObject);
    }

    protected virtual void DropItems()
    {
        foreach (ItemDataJH itemData in dropItems)
        {
            // 확률 계산
            if (Random.value <= itemData.dropChance) // Random.value는 0.0~1.0 사이의 값
            {
                for (int i = 0; i < itemData.dropCount; i++)
                {
                    // 랜덤 오프셋 계산
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-1f, 1f), // X축 랜덤 오프셋
                        0.5f,                 // Y축 약간 위로
                        Random.Range(-1f, 1f) // Z축 랜덤 오프셋
                    );

                    // 현재 위치 + 랜덤 오프셋
                    Vector3 dropPosition = transform.position + randomOffset;

                    // 아이템 생성
                    Instantiate(itemData.itemPrefab, dropPosition, Quaternion.identity);
                }
            }
        }
    }
}
