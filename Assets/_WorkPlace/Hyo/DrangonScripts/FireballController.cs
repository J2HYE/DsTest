using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Vector3 targetPosition;
    private DragonData dragonData; // 공격자 데이터
    private CharacterData targetData; // 타겟 데이터
    private System.Action onHitCallback;
    public float speed = 10f;

    // 초기화 함수
    public void Initialize(Vector3 targetPosition, DragonData dragonData, CharacterData targetData, System.Action onHitCallback)
    {
        this.targetPosition = targetPosition;
        this.dragonData = dragonData;
        this.targetData = targetData;
        this.onHitCallback = onHitCallback;
    }

    void Update()
    {
        // 타겟 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 타겟에 도달 시
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // onHitCallback을 호출하여 공격 처리
            onHitCallback?.Invoke();

            // VFX 파괴
            Destroy(gameObject);
        }
    }
}