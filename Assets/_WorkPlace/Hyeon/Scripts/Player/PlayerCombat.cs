using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController controller;
    private PlayerData playerData;
    public GameObject sword;
    private Collider swordCollider;
    [SerializeField] private float attackPerceptionRange = 5f;
    [SerializeField] private float skillPerceptionRange = 5f;

    public Animator playerAnimator;
    private Transform closestMonster;

    public bool CanReceiveInput { get; set; } = true;

    public bool inputReceived = false;

    public Quaternion targetRotation;
    public Collider SwordCollider => swordCollider;

    public HashSet<GameObject> DamagedTargets { get; set; } = new HashSet<GameObject>();
    private static readonly int[] AttackStateHash = {
        Animator.StringToHash("Base Layer.ComboAttack.Attack_1"),
        Animator.StringToHash("Base Layer.ComboAttack.Attack_2"),
        Animator.StringToHash("Base Layer.ComboAttack.Attack_3")
    };

    private static readonly int[] SkillStateHash = {
        Animator.StringToHash("Base Layer.Skill_1"),
        Animator.StringToHash("Base Layer.Skill_2")
    };

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        swordCollider = sword.GetComponent<Collider>();
        playerData = controller.playerData;

        swordCollider.enabled = false;
    }

    private void Update()
    {
        //Debug.Log($"Current CanMove : {controller.CanMove}");

        HandleAttackInput();
        HandleSkillInput();
        AttackFinishedCheck();
        SkillFinishedCheck();
    }

    private void HandleAttackInput()
    {
        if (InputManager.InputActions.actions["Attack"].triggered && CanReceiveInput)
        {
            Debug.Log("Attack");
            inputReceived = true;

            PerformComboAttack();
        }
    }

    private void PerformComboAttack()
    {
        controller.isAttack = true;
        closestMonster = GetClosestMonster(attackPerceptionRange);
        if (closestMonster != null)
        {
            //Debug.Log("Closest Monster: " + closestMonster.name);
            LookEnemy();
        }
        
        if (inputReceived)
        {
            inputReceived = false;
            CanReceiveInput = false;

            playerAnimator.SetTrigger("NextCombo");
        }
    }

    private void HandleSkillInput()
    {
        GameObject closestMonster = null;
        if(GetClosestMonster(skillPerceptionRange) != null)
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
                    controller.CanMove = false;
                    controller.CanAttack = false;
                    controller.CanUseSkill = false;
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

    private void AttackFinishedCheck()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if(AttackStateHash.Contains(stateInfo.fullPathHash))
        {

            float normalizedTime = stateInfo.normalizedTime;

            if(normalizedTime >= 0.95f)
            {
                controller.CanMove = true;
                controller.isAttack = false;
            }
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
                controller.CanMove = true;
                controller.CanAttack = true;   // 막았던 평타 키 활성화
                controller.CanUseSkill = true;
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

    private void LookEnemy()
    {
        Vector3 dir = (closestMonster.position - transform.position).normalized;
        dir.y = 0f;
        targetRotation = Quaternion.LookRotation(dir);
        controller.transform.rotation = targetRotation;
    }

    public void ToggleSwordVisible()
    {
        sword.SetActive(!sword.activeSelf);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackPerceptionRange);
    }
}
