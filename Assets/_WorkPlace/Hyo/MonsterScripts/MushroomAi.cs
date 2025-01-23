using System.Collections;
using UnityEngine;

public class MushroomAi : BaseMonsterAI
{
    protected override void PerformAttack()
    {
        // 물리 처리 방식
        SetAttackingState();
        // 공격 상태로 전환하고 애니메이션 실행
        base.PerformAttack();
    }

    private void SetAttackingState()
    {
        // 공격 상태에서는 물리 엔진 비활성화
        SetCollisionAndPhysics(false);
        StartCoroutine(ReEnablePhysics()); // 공격 후 물리 엔진 재활성화
    }

    private void SetCollisionAndPhysics(bool enablePhysics)
    {
        // 물리 충돌 및 물리 설정 변경
        if (col != null)
        {
            col.isTrigger = !enablePhysics; // 충돌 여부 설정
        }

        if (rb != null)
        {
            rb.isKinematic = !enablePhysics; // Kinematic 설정
            rb.collisionDetectionMode = enablePhysics ? CollisionDetectionMode.ContinuousDynamic : CollisionDetectionMode.Discrete;
        }
    }

    private IEnumerator ReEnablePhysics()
    {
        // 일정 시간 후 물리 계산 재활성화
        yield return new WaitForSeconds(attackCooldown);
        SetCollisionAndPhysics(true);
    }
}
