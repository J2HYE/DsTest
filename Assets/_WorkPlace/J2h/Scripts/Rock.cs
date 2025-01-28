using System.Collections.Generic;
using UnityEngine;

public class Rock : InteractableOb
{
    //public int maxHits = 3; // 최대 타격 수 설정

    [Header("Rock Drop Settings")]
    // 드롭할 아이템들을 설정합니다 (예: Stone1, Stone2, Stone3)
    public ItemDataJH stone1;
    public ItemDataJH stone2;
    public ItemDataJH stone3;

    // 도구 태그에 따라 다르게 반응
    public override void Interact(string toolTag)
    {
        base.Interact(toolTag); // 기본 Interact 호출

        if (toolTag == "Pickaxe") // 곡괭이로 타격
        {
            Debug.Log("곡괭이로 돌을 때림!");
        }

        // 타격 후, 아이템 드롭 및 오브젝트 파괴는 부모 클래스에서 처리
    }

    protected override void DropItems()
    {
        // 아이템 드롭을 위한 랜덤 로직
        int randomStoneCount = Random.Range(2, 5); // 랜덤으로 드롭할 스톤 개수 (2~4개)

        for (int i = 0; i < randomStoneCount; i++)
        {
            // 랜덤으로 드롭할 스톤 선택
            ItemDataJH selectedStone = SelectRandomStone();

            // 드롭 확률에 맞춰 아이템을 드롭
            if (Random.value <= selectedStone.dropChance)
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
                Instantiate(selectedStone.itemPrefab, dropPosition, Quaternion.identity);
            }
        }
    }

    private ItemDataJH SelectRandomStone()
    {
        // 스톤 3개 중 랜덤으로 하나를 선택
        int randomIndex = Random.Range(0, 3); // 0, 1, 2 중 하나
        switch (randomIndex)
        {
            case 0: return stone1;
            case 1: return stone2;
            case 2: return stone3;
            default: return stone1; // 기본값
        }
    }
}
