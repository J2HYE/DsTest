using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Collider swordCollider;

    public HashSet<GameObject> DamagedTargets { get; set; } = new HashSet<GameObject>();

    private void Start()
    {
        swordCollider = GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword Trigger");
        if (!swordCollider.enabled) return;
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
                    CombatManager.Instance.ProcessAttack(CharacterManager.PlayerCharacterData, monster, other.transform, true);
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
