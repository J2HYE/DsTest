using System.Xml.Serialization;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    private static readonly int TurnDirection = Animator.StringToHash("turnDirection");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    
    private DragonData dragonData;           // 드래곤 데이터

    private Transform player;             // 따라다닐 플레이어
    private Animator animator;           // 용의 애니메이터
    private Vector3 lastPlayerPosition;  // 플레이어의 마지막 위치
    private bool isMoving;               // 현재 이동 중인지 상태

    public float followDistance = 3f;    // 플레이어와 유지할 거리
    public float teleportDistance = 10f; // 순간이동할 거리
    public float followSpeed = 5f;       // 따라다니는 속도
    public float hoverHeight = 2f;       // 플레이어 위로 떠 있을 높이
    public float rotationSpeed = 5f;     // 회전 속도

    private Vector3 offset;              // 플레이어와의 상대 위치

    private void OnEnable()
    {
        GameManager.DragonTransform = transform;
    }

    void Start()
    {
        dragonData = CharacterManager.DragonData;
        
        player = GameManager.playerTransform; // 플레이어 Transform 가져오기
        animator = GetComponent<Animator>(); // 애니메이터 가져오기
        
        if (dragonData == null)
        {
            Debug.LogError("DragonData가 CharacterManager에서 설정되지 않았습니다.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player Transform이 GameManager에서 설정되지 않았습니다.");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator가 이 오브젝트에 연결되어 있지 않습니다.");
            return;
        }

        // 플레이어와 초기 상대 위치 설정
        offset = new Vector3(0, hoverHeight, -followDistance);
        lastPlayerPosition = player.position; // 초기 위치 저장
    }

    void Update()
    {
        if (player == null || dragonData == null) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 순간이동 조건
        if (distanceToPlayer > teleportDistance)
        {
            // 플레이어 옆으로 순간이동
            transform.position = player.position + offset;
        }
        else
        {
            // 플레이어를 따라다니는 동작
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

            // 용이 플레이어 방향을 바라보도록 회전
            RotateTowardsPlayer();
        }

        // 플레이어의 이동 상태 업데이트
        UpdateMovementState();

        // 블렌드 트리 파라미터 업데이트
        UpdateTurnDirection();
    }

    private void UpdateMovementState()
    {
        // 플레이어가 이동 중인지 확인
        bool playerIsMoving = Vector3.Distance(player.position, lastPlayerPosition) > 0.01f;

        // 상태가 변경되었을 경우 애니메이터 업데이트
        if (playerIsMoving != isMoving)
        {
            isMoving = playerIsMoving;
            animator.SetBool(IsMoving, isMoving);
        }

        // 플레이어의 현재 위치 저장
        lastPlayerPosition = player.position;
    }

    private void RotateTowardsPlayer()
    {
        // 플레이어가 바라보는 방향으로 용을 회전시킴
        Quaternion targetRotation = player.rotation;

        // y축 회전만 반영하도록 수정
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        // 부드럽게 회전
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateTurnDirection()
    {
        // 플레이어의 바라보는 방향
        Vector3 playerForward = player.forward;
        Vector3 dragonForward = transform.forward;

        // 좌/우 방향 차이 계산 (SignedAngle로 -1 ~ 1 범위 구하기)
        float turnDirection = Vector3.SignedAngle(dragonForward, playerForward, Vector3.up) / 90f;

        // -1 ~ 1로 값 제한
        turnDirection = Mathf.Clamp(turnDirection, -1f, 1f);

        // 애니메이터 파라미터 업데이트
        animator.SetFloat(TurnDirection, turnDirection);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
            Gizmos.DrawWireSphere(player.position, teleportDistance);
        }
    }
}
