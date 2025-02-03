using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController controller;
    public WeaponManager weapon;
    private PlayerData playerData;
    public Collider weaponCollider;
    [SerializeField] private float attackPerceptionRange = 5f;
    [SerializeField] private float skillPerceptionRange = 5f;

    public Animator playerAnimator;
    private Transform closestMonster;

    public bool CanReceiveInput { get; set; } = true;
    public bool hasWeapon;

    // public bool inputReceived = false;
    // private bool inputReceived = false;

    public Quaternion targetRotation;



    ////////////////////////////////////////////////////////////
    /// /// JWS 2025.01.27 13:00 수정
    // 사용처가 없어서 주석처리
    // public HashSet<GameObject> DamagedTargets { get; set; } = new HashSet<GameObject>();
    //   
    // private static readonly int[] AttackStateHash = {
    //    Animator.StringToHash("Base Layer.ComboAttack.Attack_1"),
    //    Animator.StringToHash("Base Layer.ComboAttack.Attack_2"),
    //    Animator.StringToHash("Base Layer.ComboAttack.Attack_3")
    //};
    ////////////////////////////////////////////////////////////

    private static readonly int[] SkillStateHash = {
        Animator.StringToHash("Base Layer.Skill_1"),
        Animator.StringToHash("Base Layer.Skill_2")
    };

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        //weaponCollider = currentWeapon.GetComponent<Collider>();
        playerData = controller.playerData;

        if(weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    private void Update()
    {
        ////////////////////////////////////////////////////////////
        // if (controller.uiCheck) return;
        // controller.uiCheck UI 켜져있을때 막기위한 추가 확인조건
        /// JWS 2025.01.27 13:00 수정
        if (controller.uiCheck) return;
        ////////////////////////////////////////////////////////////

        //Debug.Log($"Current CanMove : {controller.CanMove}");

        HandleAttackInput();
        HandleSkillInput();

        // 2025.01.29 JWS 주석처리 사용안해서
        // AttackFinishedCheck();
        SkillFinishedCheck();
    }

    private void HandleAttackInput()
    {

        if (InputManager.InputActions.actions["Attack"].triggered && CanReceiveInput && hasWeapon)
        {
            Debug.Log("Attack");

            // 2025.01.29 JWS 주석처리 사용안해서
            //inputReceived = true;

            PerformComboAttack();
        }
    }

    ////////////////////////////////////////////////////////////
    // 스테이트머신에서 처리하는걸로 옮겨서 불필요
    /// JWS 2025.01.29 12:00 수정
    //private void PerformComboAttack()
    //{
    //    controller.isAttack = true;
    //    closestMonster = GetClosestMonster(attackPerceptionRange);
    //    if (closestMonster != null)
    //    {
    //        //Debug.Log("Closest Monster: " + closestMonster.name);
    //        LookEnemy();
    //    }

    //    if (inputReceived)
    //    {
    //        inputReceived = false;
    //        CanReceiveInput = false;

    //        playerAnimator.SetTrigger("NextCombo");
    //    }
    //}

    //private void AttackFinishedCheck()
    //{
    //    AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
    //    if(AttackStateHash.Contains(stateInfo.fullPathHash))
    //    {
    //        float normalizedTime = stateInfo.normalizedTime;

    //        if(normalizedTime >= 0.95f)
    //        {
    //            controller.CanMove = true;
    //            controller.isAttack = false;
    //        }
    //    }
    //}
    ////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////
    ///// JWS 2025.01.29 12:00 수정
    //private void PerformComboAttack()
    //{
    //    controller.isAttack = true;
    //    closestMonster = GetClosestMonster(attackPerceptionRange);
    //    if (closestMonster != null)
    //    {
    //        //Debug.Log("Closest Monster: " + closestMonster.name);
    //        LookEnemy();
    //    }
    //    playerAnimator.SetTrigger("NextCombo");
    //}
    private void PerformComboAttack()
    {
        controller.isAttack = true;
        LookEnemy();
        playerAnimator.SetTrigger("NextCombo");
    }

    // 현재 콤보진행상태 받아보는 함수 의미 없음.
    public void CurrentComboStates(StateComboName stateComboName)
    {
        Debug.LogWarning(stateComboName.ToString());
    }

    // 콤보 끝났을때 받는 함수.
    public void AttackFinishedCheck()
    {
        Debug.LogWarning("콤보종료함");
        controller.CanMove = true;
        controller.isAttack = false;
    }
    ////////////////////////////////////////////////////////////

    private void HandleSkillInput()
    {
        GameObject closestMonster = null;
        if (GetClosestMonster(skillPerceptionRange) != null)
        {
            closestMonster = GetClosestMonster(skillPerceptionRange).gameObject;
        }
        if (InputManager.InputActions.actions["PlayerSkill_1"].triggered)
        {
            if (SkillManager.Instance.CheckMana("Fire"))
            {
                SkillManager.Instance.ActivateSkill("Fire");
                if (!SkillManager.Instance.isActivating)
                {
                    controller.isUseSkill = true;
                }
            }
            else
            {
                Debug.Log("스킬 마나 부족");
            }
        }
        if (InputManager.InputActions.actions["PlayerSkill_2"].triggered)
        {
            controller.CanMove = false;
            controller.CanAttack = false;
            SkillManager.Instance.ActivateSkill("Water", closestMonster);
        }
        if (InputManager.InputActions.actions["PlayerSkill_3"].triggered)
        {
            controller.CanMove = false;
            controller.CanAttack = false;
            SkillManager.Instance.ActivateSkill("eee", closestMonster);
        }
    }

    // 스킬 애니메이션이 끝났는지 검사
    private void SkillFinishedCheck()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (SkillStateHash.Contains(stateInfo.fullPathHash))
        {
            float normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= 0.95f)
            {
                controller.isUseSkill = false;
            }
        }
    }

    private Transform GetClosestMonster(float perceptionRange)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, perceptionRange);
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTransform = collider.transform;
                }
            }
        }
        return closestTransform;
    }

    ////////////////////////////////////////////////////////////
    ///// JWS 2025.01.29 12:00 수정
    //private void LookEnemy()
    //{
    //    Vector3 dir = (closestMonster.position - transform.position).normalized;
    //    dir.y = 0f;
    //    targetRotation = Quaternion.LookRotation(dir);
    //    controller.transform.rotation = targetRotation;
    //}

    public void LookEnemy()
    {
        closestMonster = GetClosestMonster(attackPerceptionRange);
        if (closestMonster == null) { return; }
        Vector3 dir = (closestMonster.position - transform.position).normalized;
        dir.y = 0f;
        targetRotation = Quaternion.LookRotation(dir);
        controller.transform.rotation = targetRotation;
    }
    ////////////////////////////////////////////////////////////

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackPerceptionRange);
    }
}
