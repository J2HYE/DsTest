using System;
using UnityEngine;

[Serializable]
public class NPCData
{
    public string id; // NPC 고유 ID
    public string name; // NPC 이름
    public string description; // NPC 설명
    public NPCType npcType; // NPC 타입
    public Sprite sprite; // NPC 이미지
    public NPCState currentState = NPCState.중립; // NPC 현재 상태
    public GameObject currentNPC;

    public string[] dialogue; // 대화 내용
    public string[] conditionalDialogue; // 조건부 대화

    public Quest[] quests; // 제공 퀘스트용으로 사용하고 리스트로 대체해도 됨.
    public Item[] items; // 판매/교환 아이템 목록으로 사용하고 리스트로 대체해도 됨.
    public ShopData shopData; // 샵정보
    public bool isInteractable; // 상호작용 가능 여부
    public string interactionCondition; // 상호작용 조건 설명(에디터용으로만 사용)

    // 행동/AI 관련
    public bool canMove; // 이동 가능 여부
    public float moveSpeed; // 이동 속도
    public Vector3[] patrolPoints; // 패트롤 경로 (상호작용 하기전 NPC만의 행동패턴)

    // 시간 및 스케줄
    public bool hasSchedule; // 스케줄 여부 ( 패트롤 혹은 시퀀스 타이밍을 위한 스케줄)
    public string activeTime; // 활성화 시간 (예: "09:00-18:00")

    // 시각적/청각적 효과 ( 필요한 경우에만 쓰게끔 설정할필요있음 )
    public AudioClip[] voiceLines; // 대사 음성 있을 경우 이런 프로퍼티를 사용
    public ParticleSystem interactionEffect; // 상호작용 시 이펙트가 있을 경우 이런 프로퍼티를 사용

    // 상점/퀘스트 세부
    public bool isShop; // 상점 여부
    public bool isQuestGiver; // 퀘스트 제공 여부
    public int reputationRequirement; // 평판 요구
}

public enum NPCType
{
    일반,        // 특별한 역할 없이 대화만 가능
    상점,        // 아이템을 판매하거나 구매 가능
    퀘스트,      // 퀘스트 제공
    이벤트,      // 특정 이벤트 트리거
    정보제공,    // 게임 정보 제공 (지형, 몬스터 약점 등)
    상호작용,    // 상호작용 트리거 (문 열기, 장치 활성화 등)
    힐러,        // 체력을 일부 회복시키는 나중에 축복같은 버프부여로 연결가능
    적NPC,       // 적으로 변할 수 있는 NPC
    동료,        // 플레이어의 동료가 될 수 있는 NPC
}

public enum NPCState
{
    중립,       // 기본 상태
    동료,       // 플레이어의 동료
    적          // 적대 상태
}
