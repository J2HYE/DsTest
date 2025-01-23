using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ItemEffect
{
    public EffectType effectType;       //이펙트 타입
    public ItemSize itemSize;           //아이템 사이즈
    public float duration;              //아이템 지속 시간
    public GameObject effectParticle;   //이펙트 사용시 파티클 프리팹

    //초기화
    public void Initialize(Item item)
    {
        Debug.Log($"{item.name} 아이템 이펙트 초기화");

        SetEffectTypeByItemType(item);
        SetItemSize(item);
        AddEffectParticle();
        if(effectType == EffectType.Equip)
        {
            ApplyGradeBonus(item);
        }
        else if(effectType == EffectType.Hp || effectType == EffectType.Mp)
        {
            SetEffectAmount(item);
        }
        else if(effectType == EffectType.Buff)
        {
            SetAmountDuration(item);
        }
    }

    /// <summary>
    /// TODO
    /// 1. 이펙트 타입별로 파티클 프리팹 알아서 적용되도록(완료)
    /// 2. 아이템 사이즈를 item에서 어떻게 받아올지 구상(완)
    /// 3. 아이템 사이즈와 등급에 따라 duration을 얼마나 줄지 구상(완)
    /// 4. 이펙트 초기화를 어느 시점에서 해야될지
    /// </summary>
    public class MultiValue
    {
        public float bonusPercnetage;
        public float duration;

        public MultiValue(float bonusPercnetage, float duration)
        {
            this.bonusPercnetage = bonusPercnetage;
            this.duration = duration;
        }
    }

    //아이템 사이즈에 따른 효과량 설정(체력, 마나 포션)
    private Dictionary<ItemSize, int> sizeAmount = new Dictionary<ItemSize, int>
    {
        { ItemSize.None, 0 },
        { ItemSize.Small, 100 },
        { ItemSize.Medium, 200 },
        { ItemSize.Big, 300 }
    };

    //아이템 사이즈에 따른 효과량 보너스퍼센트, 지속시간 설정(버프 포션)
    private Dictionary<ItemSize, MultiValue> sizeAmountDuration = new Dictionary<ItemSize, MultiValue>
    {
        { ItemSize.None, new MultiValue(0f, 0f)},
        { ItemSize.Small, new MultiValue(0.1f, 30f)},
        { ItemSize.Medium, new MultiValue(0.2f, 60f)},
        { ItemSize.Big, new MultiValue(0.3f, 120f)}
    };

    //아이템 등급에 따른 스탯 보너스 값(장착하는 아이템 적용)
    private Dictionary<ItemGrade, float> gradeBonusPercentage = new Dictionary<ItemGrade, float>
    {
        { ItemGrade.일반, 0f },
        { ItemGrade.고급, 0.1f },
        { ItemGrade.희귀, 0.2f },
        { ItemGrade.에픽, 0.3f },
        { ItemGrade.전설, 0.5f },
        { ItemGrade.신화, 0.75f }
    };

    //아이템타입에 따라 이펙트타입 설정
    public void SetEffectTypeByItemType(Item item)
    {
        ItemType itemType = item.type;
        ConsumableType consumableType = item.consumableType;

        if (itemType == ItemType.무기 || itemType == ItemType.방어구 || itemType == ItemType.장신구)
        {
            effectType = EffectType.Equip;
        }
        else if (itemType == ItemType.소모품)
        {
            if (consumableType == ConsumableType.체력포션)
            {
                effectType = EffectType.Hp;
            }
            else if (consumableType == ConsumableType.마나포션)
            {
                effectType = EffectType.Mp;
            }
            else if (consumableType == ConsumableType.버프)
            {
                effectType = EffectType.Buff;
            }
            else
            {
                effectType = EffectType.None;
            }
        }
    }

    //아이템 이름에 따라 사이즈 설정
    public void SetItemSize(Item item)
    {
        if(item.name.Contains("대형"))
        {
            itemSize = ItemSize.Big;
        }
        else if(item.name.Contains("중형"))
        {
            itemSize = ItemSize.Medium;
        }
        else if(item.name.Contains("소형"))
        {
            itemSize = ItemSize.Small;
        }
        else
        {
            itemSize = ItemSize.None;
        }
    }

    //item 사이즈에 따라 effectAmount양 추가
    public void SetEffectAmount(Item item)
    {
        if (!sizeAmount.TryGetValue(item.effect.itemSize, out int effectAmount))
        {
            Debug.Log($"{item.effect.itemSize}에 대한 {effectAmount} 값이 없음");
            return;
        }

        //사이즈에 따른 효과량 설정
        item.effectAmount = effectAmount;
    }

    //item 사이즈에 따라 버프 효과량 및 지속시간 설정
    public void SetAmountDuration(Item item)
    {
        if (!sizeAmountDuration.TryGetValue(item.effect.itemSize, out MultiValue effectAmountDuration))
        {
            Debug.Log($"{item.grade}에 대한 효과량, 지속시간 값이 없음");
            return;
        }

        //효과량과 버프 지속시간 설정
        item.effectAmount += Mathf.RoundToInt(item.effectAmount * (effectAmountDuration.bonusPercnetage));
        duration = effectAmountDuration.duration;
    }

    //아이템 등급에 따른 스탯 보너스 값 적용
    public void ApplyGradeBonus(Item item)
    {
        if (!gradeBonusPercentage.TryGetValue(item.grade, out float bonusPercentage))
        {
            Debug.Log($"{item.grade} 등급에 대한 보너스 값이 없습니다.");
            return;
        }

        ItemStat itemStat = item.itemStat;

        itemStat.Strength += Mathf.RoundToInt(itemStat.Strength * (bonusPercentage));
        itemStat.Dexterity += Mathf.RoundToInt(itemStat.Dexterity * (bonusPercentage));
        itemStat.Intelligence += Mathf.RoundToInt(itemStat.Intelligence * (bonusPercentage));
        itemStat.Vitality += Mathf.RoundToInt(itemStat.Vitality * (bonusPercentage));
        itemStat.Luck += Mathf.RoundToInt(itemStat.Luck * (bonusPercentage));
        
        itemStat.MaxHealth += Mathf.RoundToInt(itemStat.MaxHealth * (bonusPercentage));
        itemStat.MaxMana += Mathf.RoundToInt(itemStat.MaxMana * (bonusPercentage));
        itemStat.PhysicalAttack += Mathf.RoundToInt(itemStat.PhysicalAttack * (bonusPercentage));
        itemStat.MagicAttack += Mathf.RoundToInt(itemStat.MagicAttack * (bonusPercentage));
        itemStat.PhysicalDefense += Mathf.RoundToInt(itemStat.PhysicalDefense * (bonusPercentage));
        itemStat.MagicDefense += Mathf.RoundToInt(itemStat.MagicDefense * (bonusPercentage));
        
        itemStat.CriticalChance += Mathf.RoundToInt(itemStat.CriticalChance * (bonusPercentage));
        itemStat.AttackSpeed += Mathf.RoundToInt(itemStat.AttackSpeed * (bonusPercentage));
        itemStat.Evasion += Mathf.RoundToInt(itemStat.Evasion * (bonusPercentage));
    }
    

    //이펙트 타입에 따라 파티클 프리팹 적용
    public void AddEffectParticle()
    {
        switch (effectType)
        {
            case EffectType.Equip:
                effectParticle = Resources.Load<GameObject>("Particles/EquipEffect");
                break;
            case EffectType.Hp:
                effectParticle = Resources.Load<GameObject>("Particles/HpEffect");
                break;
            case EffectType.Mp:
                effectParticle = Resources.Load<GameObject>("Particles/MpEffect");
                break;
            case EffectType.Buff:
                effectParticle = Resources.Load<GameObject>("Particles/BuffEffect");
                break;
            default:
                Debug.Log($"아이템타입의 이펙트 프리팹 없음");
                break;
        }
    }
}

public enum EffectType
{
    None,
    Equip,
    Hp,
    Mp,
    Buff
}

public enum ItemSize
{
    None,
    Small,
    Medium,
    Big
}