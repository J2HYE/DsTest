using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

public class PlayerController : MonoBehaviour
{
    #region ----------Variables----------
    public PlayerData playerData;
    public PlayerCombat playerCombat;
    [SerializeField] private float staminaRecoveryRate;

    public PlayerState CurrentState { get; private set; } = PlayerState.PlayerIdle;
    private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;
    public Animator PlayerAnimator => playerAnimator;

    [Header("이동")]
    private Vector2 moveInput;
    [SerializeField] private Transform cameraTransform;
    private float walkSpeed;
    private float sprintSpeed;
    private Vector3 direction;
    private Vector3 moveDirection;
    public bool CanMove;
    [SerializeField] private bool isGrounded;
    private bool isSprinting;

    [Header("점프 / 중력")]
    [SerializeField] private float jumpHeight = 0.5f;
    private float gravity = -9.81f;
    private Vector3 verticalVelocity;
    private float lastGroundHeight;
    [SerializeField] private float fallDamageThreshold = 5f;
    [SerializeField] private float fallDamageMultiplier = 5f;
    [SerializeField] private bool isFreefall;

    [Header("벽타기")]
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private float climbRange = 1f;
    private Vector3 currentCliffNormal;
    public bool isClimb;
    public bool CanClimb;
    [SerializeField] private float climbEndCheckOffset = 0.2f; // 절벽 끝 검사 오프셋

    [Header("글라이딩")]
    [SerializeField] private bool isGliding;
    [SerializeField] private bool CanGliding;
    [SerializeField] private float glidableHeight = 6f;

    [Header("닷지")]
    [SerializeField] private float dodgeDist = 3f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private bool isDodging = false;
    //[SerializeField] private bool isInvincible = false;

    [Header("공격")]
    public bool CanAttack;
    public bool CanUseSkill;
    public bool CanParry;
    public bool isAttack;
    public bool isUseSkill;
    public bool isParry;

    [Header("디버그용")]
    [SerializeField] private bool CanSprint;
    [SerializeField] private float staminaUseAmount;
    [SerializeField] private float currentStamina;
    [SerializeField] private bool isEnoughMana;
    [SerializeField] private bool isRecovery;

    private BasicTimer RecoveryTimer;
    [SerializeField] private float RecoveryTime = 1f;


    #endregion

    private void OnEnable()
    {
        GameManager.playerTransform = this.transform;
    }

    private void OnDestroy()
    {
        if (playerData != null) playerData.OnTakeDamage -= HitCheck;
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        playerCombat = GetComponent<PlayerCombat>();
        CanMove = true;
        CanAttack = true;
        CanUseSkill = true;
        CanParry = true;
        playerData = CharacterManager.PlayerCharacterData;
        RecoveryTimer = new BasicTimer(RecoveryTime);

        if (playerData != null) playerData.OnTakeDamage += HitCheck;

        ValueInitialize();
    }

    private void Update()
    {
        DeathCheck();
        isGrounded = characterController.isGrounded;
        playerAnimator.SetBool("Grounded", isGrounded);

        RecoverStats();
        HandleGravity();
        avoidKeyInput();


        DetectCliff();
        UpdateClimbState();
        if (!isDodging)
        {
            ControlMovement();
            ControlJump();
            OnDodge();
            OnParry();
        }

        CanGlidingCheck();
        OnGliding();

        switch (CurrentState)
        {
            case PlayerState.PlayerIdle:
                break;
            case PlayerState.PlayerMove:
                break;
            case PlayerState.PlayerAttack:
                break;
            case PlayerState.PlayerHit:
                break;
            case PlayerState.PlayerDeath:
                break;
        }
    }

    private void SetState(PlayerState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
    }

    // PlayerStats 초기화
    private void ValueInitialize()
    {
        if(playerData != null)
        {
            walkSpeed = playerData.speed;
            sprintSpeed = playerData.speed * 2f;
        }
    }

    private void RunableCheck()
    {
        if (isSprinting)
        {
            currentStamina = playerData.staminaCurrent;
            if (isRecovery && currentStamina > 10f)
            {
                CanSprint = true;
                return;
            }
            else if (!isRecovery)
            {
                CanSprint = currentStamina >= staminaUseAmount ? true : false;
                return;
            }
        }
        else
        {
            CanSprint = false;
        }
    }

    private void UsingStamina()
    {
        if (CanSprint && moveInput != Vector2.zero)
        {
            playerData.UseStamina(staminaUseAmount);
        }
    }

    private void RecoverStats()
    {
        if (!CanSprint || moveInput == Vector2.zero)
        {
            playerData.RegenerateStamina();
            isRecovery = true;
        }
        else
        {
            isRecovery = false;
        }
        if (!RecoveryTimer.IsRunning)
        {
            playerData.RegenerateMp();
            TimerManager.Instance.StartTimer(RecoveryTimer);
        }
    }

    private void SwitchingBoolState()
    {
        switch (isAttack)
        {
            case true:
                CanMove = false;
                CanParry = false;
                break;
            case false:
                CanMove = true;
                CanAttack = true;
                CanParry = true;
                break;
        }

        switch (isUseSkill)
        {
            case true:
                CanMove = false;
                CanUseSkill = false;
                CanParry = false;
                break;
            case false:
                CanMove = true;
                CanUseSkill = true;
                CanParry = true;
                break;
        }
    }

    // 상시 중력 적용
    private void HandleGravity()
    {
        if (isClimb) return;

        if (!isGrounded)
        {
            CanAttack = false; CanUseSkill = false; CanParry = false;
            if (verticalVelocity.y <= fallDamageThreshold && !isFreefall && !isClimb)
            {
                isFreefall = true;
                lastGroundHeight = transform.position.y;
                playerAnimator.SetBool("Freefall", true);
            }

            if (isGliding)
            {
                isFreefall = false;
                verticalVelocity.y += (gravity / 10) * Time.deltaTime;
            }
            else
            {
                isFreefall = true;
                verticalVelocity.y += gravity * Time.deltaTime;
            }
        }
        else
        {
            if (isFreefall)
            {
                float fallDistance = lastGroundHeight - transform.position.y;

                if(fallDistance > fallDamageThreshold)
                {
                    ApplyFallDamage(fallDistance);
                }
            }
            isFreefall = false;
            isGliding = false;
            if (!isAttack && !isUseSkill && !isParry)
            {
                CanAttack = true;
                CanUseSkill = true;
                CanParry = true;
            }
            playerAnimator.SetBool("Freefall", false);
            playerAnimator.SetBool("Jump", false);
            if (verticalVelocity.y < 0)
            {
                verticalVelocity.y = -0.5f;
            }
        }
    }

    // 낙뎀
    private void ApplyFallDamage(float fallDistance)
    {
        float damage = (fallDistance - fallDamageThreshold) * fallDamageMultiplier;
        damage = Mathf.Max(0, damage);
        playerData.TakeDamage((int)damage);
        Debug.Log($"낙하 데미지: {(int)damage}");
    }

    private void StateCheck()
    {
        if(isGrounded && moveInput == Vector2.zero)
        {
            SetState(PlayerState.PlayerIdle);
        }
    }

    private bool animFinishCheck(string animName)
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
        AnimationClip currentClip = clipInfo[0].clip;
        float normalize = stateInfo.normalizedTime;
        if(currentClip.name == animName)
        {
            if (normalize >= 0.95f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    // 카메라 회전 기준으로 정면 변경
    private Vector3 GetDirection(Vector2 _moveInput)
    {
        Vector2 moveInput = _moveInput;
        Vector3 forward;
        Vector3 right;
        if (!isClimb)
        {
            forward = cameraTransform.forward;
            right = cameraTransform.right;

            
        }
        else
        {
            forward = transform.up;
            right = transform.right;
        }

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return (forward * moveInput.y + right * moveInput.x).normalized;

    }

    // 캐릭터 이동
    private void ControlMovement()
    {
        if (!CanMove) return;

        if (isClimb)
        {
            ControlCliffMovement();
            return;
        }

        moveInput = InputManager.InputActions.actions["Move"].ReadValue<Vector2>();
        isSprinting = InputManager.InputActions.actions["Sprint"].IsPressed();

        direction = GetDirection(moveInput);
        moveDirection = direction;
        moveDirection.y = verticalVelocity.y;

        RunableCheck();
        float currentSpeed = CanSprint ? sprintSpeed : walkSpeed;
        if (isGliding)
        {
            currentSpeed = walkSpeed;
        }

        if (moveInput == Vector2.zero)
        {
            playerAnimator.SetFloat("MotionSpeed", 0);
            playerAnimator.SetFloat("Speed", 0);

        }
        else
        {
            playerAnimator.SetFloat("MotionSpeed", 1);
            playerAnimator.SetFloat("Speed", currentSpeed);
        }

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        UsingStamina();

        if (direction != Vector3.zero)
        {
            if(transform.rotation.eulerAngles.y != cameraTransform.rotation.eulerAngles.y)
            {

            }
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(forward, direction, Vector3.up);

            if(Mathf.Abs(angle) > 0.1f)
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            }
        }
    }

    // 캐릭터 점프 제어
    private void ControlJump()
    {
        if (!CanMove) return;

        if (InputManager.InputActions.actions["Jump"].triggered && isGrounded)
        {
            OnJump();
        }
    }

    // 점프
    private void OnJump()
    {
        playerAnimator.SetBool("Jump", true);

        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // 캐릭터 구르기
    private void OnDodge()
    {
        if (InputManager.InputActions.actions["Dodge"].triggered && moveInput != Vector2.zero)
        {
            CanMove = false;
            StartCoroutine(Dodging());
        }
    }

    // 굴리기(무적)
    private IEnumerator Dodging()
    {
        isDodging = true;
        //isInvincible = true;

        Vector3 dodgeDirection = GetDirection(moveInput);
        float elapsedTime = 0f;

        while(elapsedTime < dodgeDuration)
        {
            characterController.Move(dodgeDirection *(dodgeDist / dodgeDuration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CanMove = true;
        isDodging = false;
        //isInvincible = false;
    }

    // 키 활성화 변경
    private void avoidKeyInput()
    {
        switch (CanMove)
        {
            case true:
                InputManager.InputActions.actions["Move"].Enable();
                break;
            case false:
                InputManager.InputActions.actions["Move"].Disable();
                break;
        }

        switch (CanAttack)
        {
            case true:
                InputManager.InputActions.actions["Attack"].Enable();
                break;
            case false:
                InputManager.InputActions.actions["Attack"].Disable();
                break;
        }

        switch (CanUseSkill)
        {
            case true:
                InputManager.InputActions.actions["PlayerSkill_1"].Enable();
                InputManager.InputActions.actions["PlayerSkill_2"].Enable();
                InputManager.InputActions.actions["PlayerSkill_3"].Enable();
                break;
            case false:
                InputManager.InputActions.actions["PlayerSkill_1"].Disable();
                InputManager.InputActions.actions["PlayerSkill_2"].Disable();
                InputManager.InputActions.actions["PlayerSkill_3"].Disable();
                break;
        }

        switch (CanParry)
        {
            case true:
                InputManager.InputActions.actions["Parry"].Enable();
                break;
            case false:
                InputManager.InputActions.actions["Parry"].Disable();
                break;
        }


    }

    // 패링
    private void OnParry()
    {
        if (InputManager.InputActions.actions["Parry"].triggered)
        {
            playerAnimator.SetTrigger("Parry");
        }
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            AnimationClip currentClip = clipInfo[0].clip;
            if (currentClip.name == "Parry")
            {
                CanAttack = false;
                CanMove = false;
                CanUseSkill = false;
                float normalizedTime = stateInfo.normalizedTime;
                if (normalizedTime < 0.2f)
                {
                    // TODO
                }
                if(normalizedTime > 0.95f)
                {
                    CanAttack = true;
                    CanMove = true;
                    CanUseSkill = true;
                }
            }

        }
        
    }

    #region ---------------Climb---------------
    private void DetectCliff()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 3f;
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, detectionRange))
        {
            if (hit.transform.gameObject.layer == 10) return;

            Debug.DrawLine(rayOrigin, hit.point, Color.red);

            float angle = Vector3.Angle(hit.normal, Vector3.up);

            float heightDifference = hit.point.y - transform.position.y;

            if (Mathf.Abs(heightDifference) < 0.01f)
            {
                heightDifference = 0f;
            }
            if (angle > 75f && angle < 105f)
            {
                CanClimb = true;
                //Debug.Log("매달릴 수 있는 벽");
                if (hit.distance <= climbRange)
                {
                    currentCliffNormal = hit.normal;
                    if (!isClimb && moveInput.y > 0)
                    {
                        StartClimbing(hit.point);
                        //Debug.Log("StartClimbing");
                    }
                }
            }
            else
            {
                CanClimb = false;
                //Debug.Log("매달릴 수 없는 벽");
            }
        }
        else
        {
            CanClimb = false;
            //playerAnimator.SetBool("Climb", false);
            //Debug.Log("Ray에 감지 안 됨");
            CheckClimbEnd();
        }
    }

    private void StartClimbing(Vector3 climbStartPosition)
    {
        if (isClimb) return;
        Debug.Log("StartClimbing");
        isClimb = true;
        isFreefall = false;
        isGliding = false;
        playerAnimator.SetBool("Freefall", false);
        playerAnimator.SetBool("Climb", true);
        playerAnimator.SetBool("Jump", false);
        transform.position = climbStartPosition;
        playerCombat.ToggleSwordVisible();
        // Anim
    }

    private void CheckClimbEnd()
    {
        if (!isClimb) return;

        Vector3 rayOrigin = transform.position + transform.up * climbEndCheckOffset;
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit hit, detectionRange))
        {
            //Debug.Log("있는 벽 오르는 중");
            //Debug.DrawLine(rayOrigin + Vector3.up * climbEndCheckOffset, hit.point, Color.red);
            //float topEdgeHeight = hit.point.y - transform.position.y;
            //Debug.Log($"topEdgeHeight = {topEdgeHeight}");
            //if (topEdgeHeight < climbEndThreshold)
            //{
            //    EndClimbing(true); // 절벽 끝까지 올라간 경우
            //    Debug.Log("성공적으로 절벽 끝 도달");
            //}
            if (isGrounded)
            {
                EndClimbing(false);
            }
        }
        else
        {
            // 옆벽 검사
            EndClimbing(true);
            Debug.Log("성공적으로 절벽 끝 도달");
            //Debug.DrawLine(rayOrigin + Vector3.up * climbEndCheckOffset, transform.forward * detectionRange, Color.blue);
        }
    }

    private void EndClimbing(bool successful)
    {
        Debug.Log("EndClimbing");
        if (successful)
        {
            isClimb = false;
            CanMove = false;
            playerAnimator.SetBool("ClimbUp", true);
            playerCombat.ToggleSwordVisible();
            StartCoroutine(FinishingClimbing());
        }
        else
        {
            isClimb = false;
            playerAnimator.SetBool("Climb", false);
            Debug.Log("절벽타기 취소됨");
            playerCombat.ToggleSwordVisible();
        }

        // Anim
    }

    private IEnumerator FinishingClimbing()
    {
        float duration = playerAnimator.GetCurrentAnimatorStateInfo(0).length + 0.5f;
        Vector3 climbUpPosition = transform.position + transform.up * 0.8f + transform.forward * 2f;
        Vector3 dir = (climbUpPosition - transform.position).normalized;
        float offsetDist = Vector3.Distance(transform.position, climbUpPosition);
        //transform.position = climbUpPosition;
        //yield return new WaitForSeconds(duration);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            characterController.Move(dir * (offsetDist / duration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerAnimator.SetBool("Climb", false);
        playerAnimator.SetBool("ClimbUp", false);
        CanMove = true;
    }

    private void HandleClimbJump()
    {
        if (moveInput.y < 0 && InputManager.InputActions.actions["Jump"].triggered)
        {
            EndClimbing(false);
            OnJump();
        }
    }
    private void ControlCliffMovement()
    {
        if (!isClimb) return;

        moveInput = InputManager.InputActions.actions["Move"].ReadValue<Vector2>();

        Vector3 climbUp = transform.up * moveInput.y; // 위/아래 이동
        Vector3 climbSide = transform.right * moveInput.x; // 좌/우 이동

        Vector3 climbDirection = climbUp + climbSide;

        moveDirection = direction;
        moveDirection.y = verticalVelocity.y;

        if (moveInput == Vector2.zero)
        {
            playerAnimator.SetFloat("VelocityY", 0f);

        }
        else
        {
            playerAnimator.SetFloat("VelocityY", moveInput.y > 0 ? 1f: -1f);
        }

        //UpdateCliffNormal();
        if(climbDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-currentCliffNormal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            climbDirection = Vector3.ProjectOnPlane(climbDirection, currentCliffNormal).normalized;
            characterController.Move(climbDirection * walkSpeed * Time.deltaTime);
        }

        StickToCliff();
    }

    private void UpdateCliffNormal()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
        {
            currentCliffNormal = hit.normal;
        }
    }

    private void StickToCliff()
    {
        if (Physics.Raycast(transform.position, -currentCliffNormal, out RaycastHit hit, 1f))
        {
            // 표면에 가까운 위치로 보정
            transform.position = hit.point + currentCliffNormal * 0.5f; // 약간 띄운다.
            Quaternion surfaceRotation = Quaternion.LookRotation(-currentCliffNormal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, surfaceRotation, Time.deltaTime * 5f);
        }
    }

    private void UpdateClimbState()
    {
        if (isClimb)
        {
            ControlCliffMovement();
            CheckClimbEnd();
            //HandleClimbCancel();
            HandleClimbJump();
        }
    }
    #endregion

    //
    private void HitCheck(Transform Attacker)
    {
        playerAnimator.SetTrigger("Hit");
    }

    private void DeathCheck()
    {
        if(playerData.currentHp <= 0)
        {
            Debug.Log("Player Death");
            CanMove = false;
            CanAttack = false;
            CanParry = false;
            CanUseSkill = false;
            // anim
        }
    }

    // 글라이딩
    private void CanGlidingCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, glidableHeight))
        {
            CanGliding = false;
            Debug.DrawLine(transform.position, hit.point, Color.magenta);
        }
        else
        {
            if (!isGrounded && isFreefall)
            {
                CanGliding = true;
            }
        }
    }

    private void OnGliding()
    {
        if (InputManager.InputActions.actions["Jump"].triggered && CanGliding)
        {
            isGliding = true;
        }
    }


    // 장비 장착에 따른 스탯 변화

    //private void IdleMotion()
    //{

    //}
}
