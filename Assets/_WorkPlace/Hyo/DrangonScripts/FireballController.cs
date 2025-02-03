using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Transform targetPosition;
    private DragonData dragonData; // 공격자 데이터
    private CharacterData targetData; // 타겟 데이터
    public float speed = 10f;
    
    // 초기화 함수
    public void Initialize(Transform targetPosition, DragonData dragonData, CharacterData targetData)
    {
        this.targetPosition = targetPosition;
        this.dragonData = dragonData;
        this.targetData = targetData;
    }

    void Update()
    {
        // 타겟 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

        // 타겟에 도달 시
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            CombatManager.Instance.ProcessDragonAttack(dragonData, targetData, targetPosition, true);
            
            // VFX 파괴 (필요시, 파티클 효과 등 추가 가능)
            Destroy(gameObject);
        }
    }
}

