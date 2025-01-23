using System.Linq;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public Animator skillAnimator;
    private PlayerController controller;

    private static readonly int[] SkillStateHash = {
        Animator.StringToHash("Base Layer.Skill_1"),
        Animator.StringToHash("Base Layer.Skill_2")
    };

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UseSkill();
        SkillFinishedCheck();
    }

    private void UseSkill()
    {
        if (InputManager.InputActions.actions["PlayerSkill_1"].triggered)
        {
            controller.CanMove = false;
            InputManager.InputActions.actions["Attack"].Disable();
            Debug.Log(InputManager.InputActions.actions["Attack"].enabled);
            SkillManager.Instance.ActivateSkill("Fire", this.gameObject);
        }
        if (InputManager.InputActions.actions["PlayerSkill_2"].triggered)
        {
            SkillManager.Instance.ActivateSkill("Water", this.gameObject);
        }
        if (InputManager.InputActions.actions["PlayerSkill_3"].triggered)
        {
            SkillManager.Instance.ActivateSkill("eee", this.gameObject);
        }
    }

    // 스킬 애니메이션이 끝났는지 검사
    private void SkillFinishedCheck()
    {
        AnimatorStateInfo stateInfo = skillAnimator.GetCurrentAnimatorStateInfo(0);
        if (SkillStateHash.Contains(stateInfo.fullPathHash))
        {
            float normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= 0.95f)
            {
                controller.CanMove = true;
                InputManager.InputActions.actions["Attack"].Enable();   // 막았던 평타 키 활성화
                Debug.Log("CanMove True");
            }
        }
    }
}
