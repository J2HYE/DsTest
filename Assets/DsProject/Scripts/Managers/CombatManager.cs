using UnityEngine;

public class CombatManager : BaseManager<CombatManager>
{
    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
        
    }
    
    // 공격 처리 메서드
    public void ProcessAttack(CharacterData playerData, CharacterData monsterData, Transform defenderTransform, bool isPlayerAttacking, bool isMagicAttack, float skillMultiplier = 1f)
    {
        GameStateMachine.Instance.ChangeState(GameSystemState.Combat);
        
        // 공격자와 방어자 설정
        CharacterData actualAttacker = isPlayerAttacking ? playerData : monsterData;
        CharacterData actualDefender = isPlayerAttacking ? monsterData : playerData;
        Transform attackerTransform = isPlayerAttacking ? GameManager.playerTransform : defenderTransform;
        Transform defenderPosition = isPlayerAttacking ? defenderTransform : GameManager.playerTransform;
        
        // 현재 타겟의 실제높이 계산을 위한부분
        Collider collider = defenderTransform.GetComponent<Collider>();
        float characterHeight = collider != null ? collider.bounds.size.y : 0f;
        float targetHeight = defenderTransform.position.y + characterHeight;// ( (characterHeight == 0f ? 2f : characterHeight));

        // 현재 타겟의 실제높이 계산을 위한부분
        Vector3 targetPosition = new Vector3(defenderTransform.position.x, targetHeight, defenderTransform.position.z);

        if (actualDefender == null || actualDefender.currentHp <= 0)
        {
            Debug.Log(actualDefender.currentHp);
            Debug.LogWarning($"{actualDefender.characterName}는 이미 사망했습니다.");
            return;
        }
        
        // 회피율 적용
        if (Random.value < actualDefender.dodgeChance)
        {
            Debug.Log($"{actualDefender.characterName}는 공격을 회피했습니다.");
            return; // 공격이 회피되어 데미지 없음
        }
        
        // 방패 블락율 처리
        if (actualDefender.hasShield)
        {
            // 방패 블록 확률이 적용되는 경우
            if (Random.value < actualDefender.blockChance)
            {
                Debug.Log($"{actualDefender.characterName}는 방패로 공격을 차단했습니다!");
                return; // 방패 차단
            }
        }
        
        // 크리티컬 체크: 물리 공격 또는 마법 공격에 대해 크리티컬 확률 계산
        bool isCritical = Random.value < actualAttacker.criticalChance;
        
        // 기본 데미지 계산
        float damage = 0f;
        
        if (isMagicAttack)
        {
            // 마법 공격
            damage = actualAttacker.magicDamage * (1 - actualDefender.magicDamageReduction);
        }
        else
        {
            // 물리 공격
            damage = actualAttacker.physicalDamage * (1 - actualDefender.physicalDamageReduction);
        }
        
        // 스킬 배율이 있을 때만 적용 (배율이 없으면 기본값 1을 사용)
        if (skillMultiplier > 1f)
        {
            damage *= skillMultiplier;
        }
        
        // 크리티컬 데미지 배율 적용
        if (isCritical)
        {
            damage *= actualAttacker.criticalDamage;
            Debug.Log($"{actualAttacker.characterName}가 크리티컬 히트를 발생시켰습니다!");
        }
        
        // 최소 데미지는 0으로 처리
        int finalDamage = Mathf.Max(0, (int)damage);
        
        // 데미지 적용
        if (damage > 0)
        {
            // 250131 2:00PM Hyeon ===============================================
            if (GameManager.playerTransform.GetComponent<PlayerController>().onParry)
            {
                Debug.LogWarning("패링 성공");  
                attackerTransform.GetComponent<BaseMonsterAI>().ChangeState(BaseMonsterAI.AIState.Stun);
                return;
            }
            // 250131 2:00PM Hyeon ===============================================
            actualDefender.TakeDamage(finalDamage, attackerTransform);
        }
        
        // UI에 데미지 텍스트 표시
        if (damage > 0)
        {
            UIManager.DisplayPopupText(finalDamage.ToString(), targetPosition, isPlayerAttacking ? MessageTag.적_피해 : MessageTag.플레이어_피해);
        }
        
        Debug.Log($"{actualAttacker.characterName}가 {actualDefender.characterName}에게 {finalDamage}의 데미지를 입혔습니다.");
        Debug.Log($"{actualDefender.characterName}의 체력이 {actualDefender.currentHp} 만큼 남았습니다.");

        // 대상이 사망했는지 확인
        if (actualDefender.currentHp <= 0)
        {
            if (!isPlayerAttacking)
            {
                // 플레이어 사망 처리
                GameStateMachine.Instance.ChangeState(GameSystemState.GameOver);
                Debug.LogError("플레이어 사망");
                return;
            }
            HandleDefeated(actualDefender, defenderPosition);
            Debug.Log(actualAttacker.ToStringForTMPro());
            QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Kill, actualDefender.characterName, 1);
        }
    }
    
    public void ProcessDragonAttack(DragonData dragonData, CharacterData targetData, Transform targetTransform, bool isMagicAttack, float skillMultiplier = 1f)
    {
        
        // 현재 타겟의 실제높이 계산을 위한부분
        Collider collider = targetTransform.GetComponent<Collider>();
        float characterHeight = collider != null ? collider.bounds.size.y : 0f;
        float targetHeight = targetTransform.position.y + characterHeight;// ( (characterHeight == 0f ? 2f : characterHeight));

        // 현재 타겟의 실제높이 계산을 위한부분
        Vector3 targetPosition = new Vector3(targetTransform.position.x, targetHeight, targetTransform.position.z);
        
        // 타겟의 데이터가 없거나 체력이 0이면 공격하지 않음
        if (targetData == null || targetData.currentHp <= 0)
        {
            Debug.LogWarning($"{targetData.characterName}는 이미 사망했습니다.");
            return;
        }

        // 회피율 적용 (타겟이 회피하면 공격하지 않음)
        if (Random.value < targetData.dodgeChance)
        {
            Debug.Log($"{targetData.characterName}는 공격을 회피했습니다.");
            return;
        }

        // 방패 블락 확률 처리
        if (targetData.hasShield)
        {
            if (Random.value < targetData.blockChance)
            {
                Debug.Log($"{targetData.characterName}는 방패로 공격을 차단했습니다!");
                return; // 방패 차단
            }
        }

        // 데미지 계산
        float damage = 0f;
        if (isMagicAttack)
        {
            // 마법 공격 처리
            damage = dragonData.magicDamage * (1 - targetData.magicDamageReduction);
        }
        else
        {
            // 물리 공격 처리
            damage = dragonData.physicalDamage * (1 - targetData.physicalDamageReduction);
        }
        
        // 스킬 배율이 있을 때만 적용 (배율이 없으면 기본값 1을 사용)
        if (skillMultiplier > 1f)
        {
            damage *= skillMultiplier;
        }

        // 크리티컬 체크: 드래곤의 크리티컬 확률
        bool isCritical = Random.value < dragonData.criticalChance;
        if (isCritical)
        {
            damage *= dragonData.criticalDamage;
            Debug.Log($"{dragonData.characterName}가 크리티컬 히트를 발생시켰습니다!");
        }

        // 최소 데미지는 0으로 설정
        int finalDamage = Mathf.Max(0, (int)damage);

        // 데미지 적용
        if (damage > 0)
        {
            targetData.TakeDamage(finalDamage, targetTransform);
        }

        // UI에 데미지 표시
        UIManager.DisplayPopupText(finalDamage.ToString(), targetPosition, MessageTag.적_피해);

        Debug.Log($"{dragonData.characterName}가 {targetData.characterName}에게 {finalDamage}의 데미지를 입혔습니다.");
        Debug.Log($"{targetData.characterName}의 체력이 {targetData.currentHp} 만큼 남았습니다.");

        // 대상이 사망했는지 확인
        if (targetData.currentHp <= 0)
        {
            HandleDefeated(targetData, targetTransform);
            QuestManager.Instance.UpdateQuestProgress(QuestConditionType.Kill, dragonData.characterName, 1);
        }
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
