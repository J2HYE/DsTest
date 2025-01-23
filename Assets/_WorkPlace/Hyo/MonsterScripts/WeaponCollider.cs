using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private BaseMonsterAI monsterAI;
    private bool hasProcessedAttack = false; // 중복 방지 플래그

    private void Start()
    {
        // 부모 오브젝트에서 BaseMonsterAI를 가져옴
        monsterAI = GetComponentInParent<BaseMonsterAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasProcessedAttack) return; // 이미 처리되었으면 중단
        
        if (other.CompareTag("Player"))
        {
            hasProcessedAttack = true; // 플래그 설정
            
            // BaseMonsterAI의 GetMonsterData 메서드로 monsterData 가져오기
            MonsterData monsterData = monsterAI.GetMonsterData();

            // 공격 처리
            CombatManager.Instance.ProcessAttack(
                CharacterManager.PlayerCharacterData,
                monsterData,
                transform.parent,
                false
            );
            
            // 충돌 처리 후 플래그 해제 (필요에 따라 설정)
            Invoke(nameof(ResetAttackProcess), 0.1f); // 일정 시간 후 플래그 초기화
        }
    }
    
    private void ResetAttackProcess()
    {
        hasProcessedAttack = false;
    }

    public void EnableWeaponCollider(bool enable)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = enable;
        }
    }
}