using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ItemEffectManager : BaseManager<ItemEffectManager>
{
    #region Variables
    //테스트 확인용 임시변수
    private Item equippedWeapon;
    private Item equippedAccessory;
    private Dictionary<EquipmentSlot, Item> equippedArmors = new Dictionary<EquipmentSlot, Item>();

    [SerializeField] private float effectParticleDuration = 2f;                 //아이템 이펙트 파티클 재생시간
    [SerializeField] private Vector3 particlePositionOffset = new Vector3();    //파티클 위치 오프셋

    private PlayerData Player
    {
        get { return CharacterManager.PlayerCharacterData; }
        set { CharacterManager.PlayerCharacterData = value; }
    }
    #endregion


    //아이템 사용 효과 적용
    public void ApplyItemEffect(Item item, int quantity = 1)
    {
        switch(item.type)
        {
            case ItemType.소모품:
                ApplyConsumableEffect(item, quantity);
                break;
            case ItemType.무기:
            case ItemType.방어구:
            case ItemType.장신구:
                ApplyEquipmentEffect(item);
            break;
            default:
                Debug.Log($"아이템타입 {item.type}의 효과 없음");
            break;
        }
    }

    //아이템 장착 해제
    public void UnequipmentEffect(Item item)
    {
        if(item.type == ItemType.무기)
        {
            if(equippedWeapon == item)
            {
                UpdatePlayerStats(item.itemStat, -1);
                equippedWeapon = null;
                InventoryManager.Instance.AddItemLogic(item);
                Debug.Log($"{item.name} 무기 해제");
            }
        }
        else if(item.type == ItemType.장신구)
        {
            if(equippedAccessory == item)
            {
                UpdatePlayerStats(item.itemStat, -1);
                equippedAccessory = null;
                InventoryManager.Instance.AddItemLogic(item);
                Debug.Log($"{item.name} 장신구 해제");
            }
        }
        else if(item.type == ItemType.방어구)
        {
            if(equippedArmors.TryGetValue(item.equipmentSlot, out Item equippedItem))
            {
                if(equippedItem == item)
                {
                    UpdatePlayerStats(item.itemStat, -1);
                    equippedArmors.Remove(item.equipmentSlot);
                    InventoryManager.Instance.AddItemLogic(item);
                    Debug.Log($"{item.name} 방어구를 {item.equipmentSlot}에서 해제");
                }
            }
            else
            {
                Debug.Log("장착된 방어구 찾을 수 없음");
            }
        }

    }


    #region Private Method
    //소모품 효과
    private void ApplyConsumableEffect(Item item, int quantity = 1)
    {
        if (item.consumableType == ConsumableType.체력포션)
        {
            Player.currentHp = Mathf.Min(Player.maxHp, (Player.currentHp + item.effectAmount * quantity));
            Debug.Log($"{item.name}을 사용하여 체력 {item.effectAmount}만큼 회복");
        }
        else if (item.consumableType == ConsumableType.마나포션)
        {
            Player.currentMp = Mathf.Min(Player.maxMp, (Player.currentMp + item.effectAmount * quantity));
            Debug.Log($"{item.name}을 사용하여 마나 {item.effectAmount}만큼 회복");
        }
        else if(item.consumableType == ConsumableType.버프)
        {
            ApplyBuff(item, item.effect.duration, quantity);
            Debug.Log($"{item.name}을 사용하여 버프 적용");
        }

        PlayParticle(item);
        InventoryManager.Instance.RemoveItemLogic(item.id, quantity);
    }

    //장착 아이템 효과
    private void ApplyEquipmentEffect(Item item)
    {
        if (item.itemStat != null)
        {            
            if(item.type == ItemType.무기)
            {
                if(equippedWeapon != null)
                {
                    Debug.Log("이미 무기가 장착되어 있음");
                    return;
                }

                equippedWeapon = item;
                Debug.Log($"{item.name} 무기 장착");
            }
            else if(item.type == ItemType.장신구)
            {
                if(equippedAccessory != null)
                {
                    Debug.Log("이미 장신구가 장착되어 있음");
                    return;
                }

                equippedAccessory = item;
                Debug.Log($"{item.name} 장신구 장착");
            }
            else if(item.type == ItemType.방어구)
            {
                if(equippedArmors.TryGetValue(item.equipmentSlot, out Item equippedItem))
                {
                    Debug.Log($"{item.equipmentSlot} 슬롯에 이미 {equippedItem} 방어구가 장착되어 있음");
                    return;
                }

                equippedArmors[item.equipmentSlot] = item;
                Debug.Log($"{item.name} 방어구를 {item.equipmentSlot} 슬롯에 장착");
            }
            else
            {
                Debug.Log("장착할 수 없는 아이템");
                return;
            }

            UpdatePlayerStats(item.itemStat, 1);
            PlayParticle(item);
            InventoryManager.Instance.RemoveItemLogic(item.id);
        }
    }

    //버프 아이템 효과
    private void ApplyBuff(Item item, float duration, int quantity = 1)
    {
        //버프아이템 효과 적용
        UpdatePlayerStats(item.itemStat, item.effectAmount);

        Debug.Log($"{item.name} 버프 아이템 사용, 효과량: {item.effectAmount}");

        //버프 지속시간
        StartCoroutine(RemoveBuffAfterDuration(item, duration * quantity));
    }

    //플레이어 스탯 업데이트
    private void UpdatePlayerStats(ItemStat stat, int multiplier)
    {
        Player.strength += stat.Strength * multiplier;
        //Player.dexterity += stat.Dexterity * multiplier;
        Player.intelligence += stat.Intelligence * multiplier;
        Player.vitality += stat.Vitality * multiplier;
        //Player.luck += stat.Luck * multiplier;

        Player.maxHp += stat.MaxHealth * multiplier;
        Player.maxMp += stat.MaxMana * multiplier;
        Player.physicalDamage += stat.PhysicalAttack * multiplier;
        Player.magicDamage += stat.MagicAttack * multiplier;
        Player.physicalDefense += stat.PhysicalDefense * multiplier;
        Player.magicDefense += stat.MagicDefense * multiplier;

        Player.criticalChance += stat.CriticalChance * multiplier;
        Player.attackSpeed += stat.AttackSpeed * multiplier;
        //Player.evasion += stat.Evasion * multiplier;

        Debug.Log($"플레이어 스탯: {stat.Strength * multiplier}");
    }

    //버프 지속시간 처리
    private IEnumerator RemoveBuffAfterDuration(Item item, float duration)
    {
        yield return new WaitForSeconds(duration);

        //버프 스탯 해제
        UpdatePlayerStats(item.itemStat, -1);
        Debug.Log($"{item.name} 버프 효과 종료");
    }

    private void PlayParticle(Item item)
    {
        if (item.effect.effectParticle != null)
        {
            GameObject itemEffectGo = Instantiate(item.effect.effectParticle, GameManager.playerTransform.position + particlePositionOffset, Quaternion.identity);

            itemEffectGo.transform.SetParent(GameManager.playerTransform);
            Destroy(itemEffectGo, effectParticleDuration);
        }
    }
    #endregion

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
   
    }
}
