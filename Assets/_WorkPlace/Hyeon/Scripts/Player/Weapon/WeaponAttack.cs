using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private Collider weaponCollider;
    public string weaponId;

    public string toolTag;

    public HashSet<GameObject> DamagedTargets { get; set; } = new HashSet<GameObject>();

    private void Start()
    {
        weaponCollider = GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        InteractableOb interactable = other.GetComponent<InteractableOb>();
        if (interactable != null)
        {
            interactable.Interact(toolTag); // 상호작용 호출
        }

        Debug.Log("Weapon Trigger");
        if (!weaponCollider.enabled) return;
        if (!DamagedTargets.Contains(other.gameObject))
        {
            DamagedTargets.Add(other.gameObject);
            int monsterHP = 0;
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                MonsterData monster = other.GetComponent<Test1>().monster;
                if (monster != null)
                {
                    monsterHP = monster.currentHp;
                    // monster.TakeDamage(swordDamage);
                    // 2025-01-27 HYO ProcessAttack 로직 변경으로 매개변수 마지막에 물리 공격인지 마법 공격인지 확인 추가 true = 마법데미지, false = 물리데미지 ---------------------------
                    CombatManager.Instance.ProcessAttack(CharacterManager.PlayerCharacterData, monster, other.transform, true, false);
                    // -----------------------------------------------------------------------------------------------------------------------------------------------------
                    Debug.Log($"Damaged: {monster.characterName}, monsterHP: {monsterHP}, monsterCurrentHP: {monster.currentHp}");
                }
            }
            else
            {
                Debug.Log("암튼 뭔갈 침");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        DamagedTargets.Remove(other.gameObject);
    }

}
