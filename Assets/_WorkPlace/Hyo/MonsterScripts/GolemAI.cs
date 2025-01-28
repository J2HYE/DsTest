using UnityEngine;
using System.Collections;

public class GolemAI : BaseMonsterAI
{
    private WeaponCollider weaponCollider;

    protected override void Start()
    {
        base.Start();
        weaponCollider = GetComponentInChildren<WeaponCollider>();
    }
    protected override void PerformAttack()
    {
        if (isAttacking) return; // 이미 공격 중이라면 중복 공격 방지
        
        // 물리 처리 방식
        StopAllActions();
        SetAttackingState();
        // 공격 상태로 전환하고 애니메이션 실행
        animator.SetTrigger(Attack);
        
        // 애니메이션의 길이 가져오기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        
        StartCoroutine(ResetAttackState(animationLength));
    }
    
    // 애니메이션 이벤트에서 호출
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.EnableWeaponCollider(true);
        }
    }
    
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.EnableWeaponCollider(false);
        }
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
