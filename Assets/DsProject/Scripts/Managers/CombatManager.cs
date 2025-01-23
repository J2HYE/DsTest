using UnityEngine;

public class CombatManager : BaseManager<CombatManager>
{
    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        
    }
    
    // 공격 처리 메서드
    public void ProcessAttack(CharacterData player, CharacterData enemy, Transform enemyTransform, bool isPlayerAttacking)
    {
        GameStateMachine.Instance.ChangeState(GameSystemState.Combat);
        
        CharacterData attacker = isPlayerAttacking ? player : enemy;
        CharacterData defender = isPlayerAttacking ? enemy : player;
        Transform attackerTransform = isPlayerAttacking ? GameManager.playerTransform : enemyTransform;
        Transform defenderTransform = isPlayerAttacking ? enemyTransform : GameManager.playerTransform;

        // 현재 타겟의 실제높이 계산을 위한부분
        Collider collider = defenderTransform.GetComponent<Collider>();
        float characterHeight = collider != null ? collider.bounds.size.y : 0f;
        float targetHeight = defenderTransform.position.y + characterHeight;// ( (characterHeight == 0f ? 2f : characterHeight));

        // 현재 타겟의 실제높이 계산을 위한부분
        Vector3 targetPosition = new Vector3(defenderTransform.position.x, targetHeight, defenderTransform.position.z);

        if (defender == null || defender.currentHp <= 0)
        {
            Debug.Log(defender.currentHp);
            Debug.LogWarning($"{defender.characterName}는 이미 사망했습니다.");
            return;
        }

        int damage = CalculateDamage(attacker, defender);

        defender.TakeDamage(damage, attackerTransform);
        

        UIManager.DisplayPopupText(damage.ToString(), targetPosition, isPlayerAttacking ? MessageTag.적_피해 : MessageTag.플레이어_피해);

        Debug.Log($"{attacker.characterName}가 {defender.characterName}에게 {damage}의 데미지를 입혔습니다.");
        Debug.Log($"{defender.characterName}의 체력이 {defender.currentHp} 만큼 남았습니다.");

        // 대상이 사망했는지 확인
        if (defender.currentHp <= 0)
        {
            if (!isPlayerAttacking)
            {
                // 플레이어 사망 처리
                GameStateMachine.Instance.ChangeState(GameSystemState.GameOver);
                Debug.LogError("플레이어 사망");
                return;
            }
            HandleDefeated(defender, enemyTransform);
            Debug.Log(attacker.ToStringForTMPro());
            QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Kill, defender.characterName, 1);
        }
        
        
    }
    
    // 데미지 계산 메서드
    private int CalculateDamage(CharacterData attacker, CharacterData defender)
    {
        // 공격자의 물리 공격력, 방어자의 물리 방어력 계산 로직
        int damage = attacker.physicalDamage - defender.physicalDefense;

        // 크리티컬 여부 체크
        bool isCritical = Random.value < attacker.criticalChance;
        damage = attacker.CalculateDamage(isCritical);
        
        // 데미지는 최소 0 이상
        return Mathf.Max(0, damage);
    }

    // 몬스터 사망 처리
    private void HandleDefeated(CharacterData defeatedCharacter, Transform defenderTransform)
    {
        // defeatedCharacter를 MonsterData로 캐스팅
        MonsterData monsterData = defeatedCharacter as MonsterData;
        if (monsterData != null)
        {
            CharacterManager.Instance.OnMonsterDefeated(monsterData, defenderTransform.position);
        }
        else
        {
            Debug.LogError("defeatedCharacter를 MonsterData로 캐스팅할 수 없습니다.");
        }
        
        GameStateMachine.Instance.ChangeState(GameSystemState.Exploration);
    }
}
