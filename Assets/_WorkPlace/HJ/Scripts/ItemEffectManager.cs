//using System.Collections.Generic;
//using UnityEngine;
//using System.Collections;

//public class ItemEffectManager : BaseManager<ItemEffectManager>
//{
//    #region Variables
//    //테스트 확인용 임시변수
//    [SerializeField] private Item equippedWeapon;
//    [SerializeField] private Item equippedAccessory;
//    // private Dictionary<EquipmentSlot, Item> equippedArmors = new Dictionary<EquipmentSlot, Item>();
//    private Dictionary<EquipmentSlot, Item> equippedItems = new Dictionary<EquipmentSlot, Item>();

//    [SerializeField] private float effectParticleDuration = 2f;                 //아이템 이펙트 파티클 재생시간
//    [SerializeField] private Vector3 particlePositionOffset = new Vector3();    //파티클 위치 오프셋

//    private PlayerData Player
//    {
//        get { return CharacterManager.PlayerCharacterData; }
//        set { CharacterManager.PlayerCharacterData = value; }
//    }
//    #endregion


//    //아이템 사용 효과 적용
//    public void ApplyItemEffect(Item item, int quantity = 1)
//    {
//        switch(item.type)
//        {
//            case ItemType.소모품:
//                ApplyConsumableEffect(item, quantity);
//                break;
//            case ItemType.무기:
//            case ItemType.방어구:
//            case ItemType.장신구:
//                ApplyEquipmentEffect(item);
//            break;
//            default:
//                Debug.Log($"아이템타입 {item.type}의 효과 없음");
//            break;
//        }
//    }

//    //아이템 장착 해제
//    public void UnequipmentEffect(Item item)
//    {
//        if (InventoryManager.Instance.GetRemainingInventory() <= 0)
//            return;

//        if(item.type == ItemType.무기)
//        {
//            if(equippedWeapon == item)
//            {
//                UpdatePlayerStats(item.itemStat, -1);
//                equippedWeapon = null;
//                Debug.Log($"{item.name} 무기 해제");
//            }
//        }
//        else if(item.type == ItemType.장신구)
//        {
//            if(equippedAccessory == item)
//            {
//                UpdatePlayerStats(item.itemStat, -1);
//                equippedAccessory = null;
//                Debug.Log($"{item.name} 장신구 해제");
//            }
//        }
//        else if(item.type == ItemType.방어구)
//        {
//            if(equippedItems.TryGetValue(item.equipmentSlot, out Item equippedItem))
//            {
//                if(equippedItem == item)
//                {
//                    UpdatePlayerStats(item.itemStat, -1);
//                    equippedItems.Remove(item.equipmentSlot);
//                    Debug.Log($"{item.name} 방어구를 {item.equipmentSlot}에서 해제");
//                }
//            }
//            else
//            {
//                Debug.Log("장착된 방어구 찾을 수 없음");
//            }
//        }
//    }


//    #region Private Method
//    //소모품 효과
//    private void ApplyConsumableEffect(Item item, int quantity = 1)
//    {
//        if (item.consumableType == ConsumableType.체력포션)
//        {
//            Player.currentHp = Mathf.Min(Player.maxHp, (Player.currentHp + item.effectAmount * quantity));
//            Debug.Log($"{item.name}을 사용하여 체력 {item.effectAmount}만큼 회복");
//        }
//        else if (item.consumableType == ConsumableType.마나포션)
//        {
//            Player.currentMp = Mathf.Min(Player.maxMp, (Player.currentMp + item.effectAmount * quantity));
//            Debug.Log($"{item.name}을 사용하여 마나 {item.effectAmount}만큼 회복");
//        }
//        else if(item.consumableType == ConsumableType.버프)
//        {
//            ApplyBuff(item, item.effect.duration, quantity);
//            Debug.Log($"{item.name}을 사용하여 버프 적용");
//        }

//        PlayParticle(item);
//        // InventoryManager.Instance.RemoveItemLogic(item.id, quantity);
//    }

//    //장착 아이템 효과
//    private void ApplyEquipmentEffect(Item item)
//    {
//        if (item.itemStat != null)
//        {
//            bool isEquipped = false;

//            if (item.type == ItemType.무기)
//            {
//                if (item.weaponType == WeaponType.한손무기)
//                {
//                    if (equippedWeapon != null)
//                    {
//                        Debug.Log($"이미 무기가 장착되어 있음, {equippedWeapon.name} 장착해제");
//                        UpdatePlayerStats(equippedWeapon.itemStat, -1);
//                    }

//                    equippedWeapon = item;
//                    Debug.Log($"{item.name} 무기 장착");
//                    isEquipped = true;
//                }
//                else if (item.weaponType == WeaponType.양손무기)
//                {
//                    if (equippedWeapon != null)
//                    {
//                        Debug.Log($"이미 무기가 장착되어 있음, {equippedWeapon.name} 장착해제");
//                        UpdatePlayerStats(equippedWeapon.itemStat, -1);
//                        equippedWeapon = null;
//                    }

//                    if (equippedItems.TryGetValue(EquipmentSlot.방패, out Item equippedItem))
//                    {
//                        Debug.Log($"방패가 장착되어 있음, {equippedItem.name} 장착해제");
//                        UpdatePlayerStats(equippedItem.itemStat, -1);
//                        equippedItems.Remove(EquipmentSlot.방패);
//                    }

//                    equippedWeapon = item;
//                    Debug.Log($"{item.name} 무기 장착");
//                    isEquipped = true;
//                }
//            }
//            else if(item.type == ItemType.장신구)
//            {
//                if(equippedAccessory != null)
//                {
//                    Debug.Log($"이미 장신구가 장착되어 있음, {equippedAccessory.name} 장착해제");
//                    UpdatePlayerStats(equippedAccessory.itemStat, -1);
//                }
//                equippedAccessory = item;
//                Debug.Log($"{item.name} 장신구 장착");
//                isEquipped = true;
//            }
//            else if (item.type == ItemType.방어구)
//            {
//                if (item.equipmentSlot == EquipmentSlot.방패 && equippedWeapon != null && equippedWeapon.weaponType == WeaponType.양손무기)
//                {
//                    Debug.Log($"무기 슬롯에 양손무기가 장착되어 있음, {equippedWeapon.name} 장착 해제");
//                    UpdatePlayerStats(equippedWeapon.itemStat, -1);
//                    equippedWeapon = null;
//                }

//                if (equippedItems.TryGetValue(item.equipmentSlot, out Item equippedItem))
//                {
//                    Debug.Log($"{item.equipmentSlot} 슬롯에 이미 방어구가 장착되어 있음, {equippedItem.name} 장착 해제");
//                    UpdatePlayerStats(equippedItem.itemStat, -1);
//                }

//                equippedItems[item.equipmentSlot] = item;
//                Debug.Log($"{item.name} 방어구를 {item.equipmentSlot} 슬롯에 장착");
//                isEquipped = true;
//            }
//            else
//            {
//                Debug.Log("장착할 수 없는 아이템");
//                return;
//            }

//            if (isEquipped)
//            {
//                UpdatePlayerStats(item.itemStat, 1);
//                PlayParticle(item);
//            }
//        }        
//    }

//    //버프 아이템 효과
//    private void ApplyBuff(Item item, float duration, int quantity = 1)
//    {
//        //버프아이템 효과 적용
//        UpdatePlayerStats(item.itemStat, item.effectAmount);

//        Debug.Log($"{item.name} 버프 아이템 사용, 효과량: {item.effectAmount}");

//        //버프 지속시간
//        StartCoroutine(RemoveBuffAfterDuration(item, duration * quantity, item.effectAmount));
//    }

//    //플레이어 스탯 업데이트
//    private void UpdatePlayerStats(ItemStat stat, int multiplier)
//    {
//        Player.strength += stat.Strength * multiplier;
//        //Player.dexterity += stat.Dexterity * multiplier;
//        Player.intelligence += stat.Intelligence * multiplier;
//        Player.vitality += stat.Vitality * multiplier;
//        //Player.luck += stat.Luck * multiplier;

//        Player.maxHp += stat.MaxHealth * multiplier;
//        Player.maxMp += stat.MaxMana * multiplier;
//        Player.physicalDamage += stat.PhysicalAttack * multiplier;
//        Player.magicDamage += stat.MagicAttack * multiplier;
//        Player.physicalDefense += stat.PhysicalDefense * multiplier;
//        Player.magicDefense += stat.MagicDefense * multiplier;

//        Player.criticalChance += stat.CriticalChance * multiplier;
//        Player.attackSpeed += stat.AttackSpeed * multiplier;
//        //Player.evasion += stat.Evasion * multiplier;

//        Debug.Log($"플레이어 스탯: {stat.Strength * multiplier}");
//    }

//    //버프 지속시간 처리
//    private IEnumerator RemoveBuffAfterDuration(Item item, float duration, int amount)
//    {
//        yield return new WaitForSeconds(duration);

//        //버프 스탯 해제
//        UpdatePlayerStats(item.itemStat, -amount);
//        Debug.Log($"{item.name} 버프 효과 해제, 효과량 {amount}");
//    }

//    private void PlayParticle(Item item)
//    {
//        if (item.effect.effectParticle != null)
//        {
//            GameObject itemEffectGo = Instantiate(item.effect.effectParticle, GameManager.playerTransform.position + particlePositionOffset, Quaternion.identity);

//            itemEffectGo.transform.SetParent(GameManager.playerTransform);
//            Destroy(itemEffectGo, effectParticleDuration);
//        }
//    }
//    #endregion

//    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
//    {

//    }
//}



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// JWS 수정                                                                                                            ///  
/// 2025.01.27 17:20 인벤토리에서 더블클릭으로 무기 장착 시스템 완성                                                    ///  
/// 이후 클릭이 아닌 소켓을 이용할때도 그대로 이용만 하면 됨.                                                           ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ItemEffectManager : BaseManager<ItemEffectManager>
{
    #region Variables

    private Dictionary<EquipmentSlot, Item> equippedItems = new Dictionary<EquipmentSlot, Item>();

    [SerializeField] private float effectParticleDuration = 2f; // 아이템 이펙트 파티클 재생 시간
    [SerializeField] private Vector3 particlePositionOffset = new Vector3(); // 파티클 위치 오프셋
    private WeaponManager weaponManager;

    private PlayerData Player
    {
        get => CharacterManager.PlayerCharacterData;
        set => CharacterManager.PlayerCharacterData = value;
    }

    #endregion

    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        weaponManager = GameManager.playerTransform.GetComponent<PlayerCombat>().weapon;
        InitializeEquipmentSlots(); // 딕셔너리 초기화
    }

    #endregion

    #region Public Methods

    // 아이템 효과 적용
    public void ApplyItemEffect(Item item, int quantity = 1)
    {
        switch (item.type)
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
                Debug.Log($"아이템 타입 {item.type}의 효과 없음");
                break;
        }
    }

    // 아이템기준 슬롯에서 아이템 장착 해제
    public void UnequipmentEffect(Item item)
    {
        if (equippedItems[item.equipmentSlot] != null && equippedItems[item.equipmentSlot] == item)
        {
            UnequipmentEffect(item.equipmentSlot);
        }
        else
        {
            Debug.LogWarning($"{item.name}이 {item.equipmentSlot}에 장착되어 있지 않습니다.");
        }
    }

    // 슬롯기준 슬롯에서 아이템 장착 해제
    public void UnequipmentEffect(EquipmentSlot equipmentSlot)
    {
        Item item = GetEquippedItem(equipmentSlot);
        if (item != null)
        {
            UpdatePlayerStats(item.itemStat, -1);
            equippedItems[item.equipmentSlot] = null;
            item.isEquired = false;
            if (item.equipmentSlot == EquipmentSlot.손) weaponManager.EquipWeapon(null);
            Debug.Log($"{item.name}을 {item.equipmentSlot}에서 해제");
        }
    }

    // 특정 슬롯의 장착 아이템 가져오기
    public Item GetEquippedItem(EquipmentSlot slot)
    {
        return equippedItems[slot]; // null이면 슬롯이 비어있음을 의미
    }

    #endregion

    #region Private Methods

    private void InitializeEquipmentSlots()
    {
        foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
        {
            if (!equippedItems.ContainsKey(slot))
            {
                equippedItems[slot] = null;
            }
        }
        Debug.Log("장비 슬롯 초기화 완료");
    }

    private void ApplyConsumableEffect(Item item, int quantity = 1)
    {
        if (item.consumableType == ConsumableType.체력포션)
        {
            Player.currentHp = Mathf.Min(Player.maxHp, Player.currentHp + item.effectAmount * quantity);
            Debug.Log($"{item.name}을 사용하여 체력 {item.effectAmount}만큼 회복");
        }
        else if (item.consumableType == ConsumableType.마나포션)
        {
            Player.currentMp = Mathf.Min(Player.maxMp, Player.currentMp + item.effectAmount * quantity);
            Debug.Log($"{item.name}을 사용하여 마나 {item.effectAmount}만큼 회복");
        }
        else if (item.consumableType == ConsumableType.버프)
        {
            ApplyBuff(item, item.effect.duration, quantity);
            Debug.Log($"{item.name}을 사용하여 버프 적용");
        }

        PlayParticle(item);
    }

    private void ApplyEquipmentEffect(Item item)
    {
        if (item.itemStat == null) return;

        // 기존 아이템 해제
        if (equippedItems[item.equipmentSlot] != null)
        {
            Debug.Log($"{item.equipmentSlot}에 장착된 {equippedItems[item.equipmentSlot].name} 해제");
            UnequipmentEffect(equippedItems[item.equipmentSlot]);
        }

        // 새로운 아이템 장착
        equippedItems[item.equipmentSlot] = item;
        item.isEquired = true;
        UpdatePlayerStats(item.itemStat, 1);
        PlayParticle(item);
        if (item.equipmentSlot == EquipmentSlot.손) weaponManager.EquipWeapon(item);
        Debug.Log($"{item.name}을 {item.equipmentSlot}에 장착");
    }

    private void ApplyBuff(Item item, float duration, int quantity = 1)
    {
        UpdatePlayerStats(item.itemStat, item.effectAmount);
        Debug.Log($"{item.name} 버프 아이템 사용, 효과량: {item.effectAmount}");

        StartCoroutine(RemoveBuffAfterDuration(item, duration * quantity, item.effectAmount));
    }

    private void UpdatePlayerStats(ItemStat stat, int multiplier)
    {
        Player.strength += stat.Strength * multiplier;
        Player.intelligence += stat.Intelligence * multiplier;
        Player.vitality += stat.Vitality * multiplier;

        Player.maxHp += stat.MaxHealth * multiplier;
        Player.maxMp += stat.MaxMana * multiplier;
        Player.physicalDamage += stat.PhysicalAttack * multiplier;
        Player.magicDamage += stat.MagicAttack * multiplier;
        Player.physicalDefense += stat.PhysicalDefense * multiplier;
        Player.magicDefense += stat.MagicDefense * multiplier;

        Player.criticalChance += stat.CriticalChance * multiplier;
        Player.attackSpeed += stat.AttackSpeed * multiplier;

        Debug.Log($"플레이어 스탯 업데이트: {stat.Strength * multiplier}");
    }

    private IEnumerator RemoveBuffAfterDuration(Item item, float duration, int amount)
    {
        yield return new WaitForSeconds(duration);

        UpdatePlayerStats(item.itemStat, -amount);
        Debug.Log($"{item.name} 버프 효과 해제, 효과량 {amount}");
    }

    private void PlayParticle(Item item)
    {
        if (item.effect.effectParticle != null)
        {
            var itemEffectGo = Instantiate(item.effect.effectParticle,
                GameManager.playerTransform.position + particlePositionOffset,
                Quaternion.identity);

            itemEffectGo.transform.SetParent(GameManager.playerTransform);
            Destroy(itemEffectGo, effectParticleDuration);
        }
    }

    #endregion

    protected override void HandleGameStateChange(GameSystemState newState, object additionalData)
    {
    }
}

