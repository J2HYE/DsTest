/*using System;
using System.Collections.Generic;

namespace JWS
{
    public enum GameSystemState
    {
        None,

        // 메뉴 관련 구성
        MainMenu,
        SubMenu,
        Loading,

        // 게임 진행 관련 구성
        Exploration,
        Combat,

        // 게임 내 UI 관련 구성
        Inventory,
        GamePause,
        GameOver,
        EtcUIInteraction,

        // 이벤트 혹은 컨텐츠 진행 관련 구성
        Dialogue,
        Cutscene,

        // 아이템 관련 구성
        ItemCollected,
        ItemRemoved,
        ItemUsed,
        ItemEquipped,
        ItemUnequipped,
        ItemDropped,

        // 퀘스트 관련 구성
        QuestAccepted,
        QuestUpdated,
    }

    public enum SystemType
    {
        None,
        CharacterManager,
        PlayerManager,
        EnemyManager,
        AIManager,
        CombatManager,
        ItemManager,
        InventoryManager,
        QuestManager,
        UIManager,
        DialogueManager
    }

    public enum ParameterType
    {
        None,             // 파라미터 없음
        SerializedData,   // JSON 데이터
        DirectReference   // Unity 오브젝트 참조
    }

    [Serializable]
    public class SystemLogic
    {
        public SystemType SystemName;        // 필수: 로직이 속한 시스템 이름
        public string Logic;                // 필수: 실행할 함수 이름
        public string ParameterType; // 선택: 함수에 전달할 매개변수의 타입 (None, SerializedData, DirectReference)
        public string Parameters;           // 선택: JSON 데이터 (ParameterType이 SerializedData일 때 사용)
        public string LogicDescription;     // 선택: 로직 설명
    }

    [Serializable]
    public class StateData
    {
        public GameSystemState StateName;          // 상태 이름
        public string TransitionCondition;         // 상태 전환 조건 (문자열 기반 커스텀 로직 가능)
        public List<SystemLogic> SystemLogics;     // 상태와 연결된 시스템 로직 목록
    }

}*/