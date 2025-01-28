using System;

/// <summary>
/// 프로젝트에 필요한 Enum을 정리한 리스트
/// </summary>
// 게임 전체 상태 - 메인 State
public enum GameSystemState
{
    MainMenu,         // 메인 메뉴
    Exploration,      // 탐험 모드
    Combat,           // 전투 모드
    BossBattle,       // 보스 전투 모드
    DialogueState,    // 대화 상태
    Inventory,        // 인벤토리 화면
    QuestReview,      // 퀘스트 정보 및 진행 상황 확인
    StatusUI,
    Shopping,         // 쇼핑
    PetInteraction,   // 펫 상호작용
    Training,         // 훈련 모드
    Cutscene,         // 컷씬 연출
    Pause,            // 일시정지
    Play,             // 게임 진행
    MainQuestPlay,    // 메인퀘스트 진행.
    InfoMessage,      // 간단한 안내나 출력매새지.
    GameOver          // 게임 종료
}