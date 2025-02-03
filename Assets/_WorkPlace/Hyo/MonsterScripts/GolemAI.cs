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
        
        // 공격 상태로 전환하고 애니메이션 실행
        animator.SetTrigger(Attack);
        
        // 애니메이션의 길이 가져오기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        
        isAttacking = true;
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
}
