using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Flame : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private List<GameObject> attackedEnemy = new List<GameObject>();
    private Dictionary<GameObject, int> enemyDamageCount = new Dictionary<GameObject, int>();
    public int maxHits = 3;

    public int damage = 10;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (enemyDamageCount.ContainsKey(other))
        {
            if(enemyDamageCount[other] >= maxHits)
            {
                return;
            }

            enemyDamageCount[other]++;
        }
        else
        {
            enemyDamageCount.Add(other, 1);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            MonsterData monster = other.GetComponent<Test1>().monster;
            int monsterHP;
            if (monster != null)
            {
                monsterHP = monster.currentHp;
                // monster.TakeDamage(damage);
                CombatManager.Instance.ProcessAttack(CharacterManager.PlayerCharacterData, monster, other.transform, true);
                Debug.Log($"Damaged: {monster.characterName}, monsterHP: {monsterHP}, monsterCurrentHP: {monster.currentHp}");
            }
        }
        Debug.Log($"Enter : {other.name}");
    }
}
