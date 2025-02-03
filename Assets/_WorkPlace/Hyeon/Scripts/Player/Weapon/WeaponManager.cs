//using System.Collections.Generic;
//using UnityEngine;

//public class WeaponManager : MonoBehaviour
//{
//    [SerializeField] private List<GameObject> weapons;
//    private GameObject currentWeaponOb;
//    private int prevWeapon = -1;

//    private int currentWeaponIndex = -1;

//    public string currentWeaponId;
//    [SerializeField] private ItemList weaponItemList;
//    private Item currentWeaponItem;
//    private ItemStat weaponItemStat;

//    private void Start()
//    {
//        InitializeWeapons();
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.H))
//        {
//            SwitchToNextWeapon();
//        }
//        if (Input.GetKeyDown(KeyCode.G))
//        {
//            SwitchToPreviousWeapon();
//        }
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            ItemManager.Instance.AddItemLogic("Weapon001");
//            ItemManager.Instance.AddItemLogic("Weapon002");
//            ItemManager.Instance.AddItemLogic("Main_Quest002");
//            ItemManager.Instance.AddItemLogic("Main_Quest003");
//            ItemManager.Instance.AddItemLogic("Main_Quest004");
//        }
//    }

//    private void InitializeWeapons()
//    {
//        foreach(var weapon in weapons)
//        {
//            weapon.SetActive(false);
//        }
//    }

//    public void EquipWeapon(int index)
//    {
//        if(index < 0 || index >= weapons.Count)
//        {
//            Debug.LogWarning("Invalid weapon index");
//            return;
//        }
//        currentWeaponIndex = index;
//        SwitchWeapon(currentWeaponIndex);
//    }

//    // 다음 인덱스 무기로 스위칭
//    public void SwitchToNextWeapon()
//    {
//        if (!GameManager.playerTransform.GetComponent<PlayerController>().CanWeaponSwitch)
//            return;

//        int nextIndex = (currentWeaponIndex + 1) % weapons.Count;
//        EquipWeapon(nextIndex);
//    }

//    public void SwitchToPreviousWeapon()
//    {
//        if (!GameManager.playerTransform.GetComponent<PlayerController>().CanWeaponSwitch)
//            return;

//        int prevIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
//        EquipWeapon(prevIndex);
//    }

//    public void SwitchWeapon(int index = -1, bool hasWeapon = false)
//    {
//        currentWeaponOb = null;

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            // i번째 자식 가져오기
//            GameObject gob = transform.GetChild(i).gameObject;

//            gob.SetActive(index == i);
//            // 현재 인덱스와 비교하여 활성화/비활성화 처리
//            if (hasWeapon && i == prevWeapon)
//            {
//                gob.SetActive(i == prevWeapon);
//                prevWeapon = -1;
//            }

//            // 활성화된 무기를 플레이어의 weaponCollider로 설정
//            if (gob.activeSelf)
//            {
//                currentWeaponOb = gob;
//                prevWeapon = i;
//                GameManager.playerTransform.GetComponent<PlayerCombat>().weaponCollider = gob.GetComponent<Collider>();
//                if (currentWeaponItem != null)
//                {
//                    ItemEffectManager.Instance.UnequipmentEffect(currentWeaponItem);
//                    //ItemManager.Instance.AddItemLogic(currentWeaponId);
//                    currentWeaponItem = null;
//                    Debug.Log("해제");
//                }
//                currentWeaponId = gob.GetComponent<WeaponAttack>().weaponId;
//                if (currentWeaponId != "")
//                {
//                    InventoryManager.Instance.FindExistingItem(currentWeaponId);
//                    currentWeaponItem = InventoryManager.InventoryList[InventoryManager.Instance.selectedItem];
//                    ItemEffectManager.Instance.ApplyItemEffect(currentWeaponItem);
//                    Debug.Log("장착");
//                }
//            }
//        }

//        GameManager.playerTransform.GetComponent<PlayerCombat>().hasWeapon = currentWeaponOb == null? false : true;

//    }

//    public void applyWeaponEffect()
//    {

//    }
//}



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// JWS 수정                                                                                                            ///  
/// 2025.01.27 17:20 인벤토리에서 더블클릭으로 무기 장착 시스템 완성                                                    ///  
/// 이후 클릭이 아닌 소켓을 이용할때도 그대로 이용만 하면 됨.                                                           ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Transform weaponObjectPosition;
    private GameObject currentWeaponObject = null;

    private void Start()
    {
        InitializeWeapons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ItemEffectManager.Instance.UnequipmentEffect(EquipmentSlot.손);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            var itemsToAdd = new[] { "완드", "나뭇가지", "나무완드", "도끼", "낡은검", "양손검"};
            foreach (var item in itemsToAdd) ItemManager.Instance.AddItemLogic(item);
        }
    }

    private void InitializeWeapons()
    {
        if (weaponObjectPosition != null)
        {
            foreach (Transform transOb in weaponObjectPosition)
            {
                string objectName = transOb.name;
                if (System.Enum.TryParse(objectName, out WeaponObjectName weaponName))
                {
                    int index = (int)weaponName;
                    transOb.SetSiblingIndex(index);
                    Debug.Log($"'{objectName}'은 WeaponObjectName의 인덱스 {index}입니다.");
                }
                else
                {
                    transOb.SetSiblingIndex(weaponObjectPosition.childCount - 1);
                    Debug.LogWarning($"'{objectName}'은 WeaponObjectName에 존재하지 않으므로 마지막으로 이동합니다.");
                }
                transOb.gameObject.SetActive(false);
            }
        }
    }

    public void EquipWeapon(Item weaponItem = null)
    {
        if (weaponItem != null && weaponItem.equipmentSlot != EquipmentSlot.손)
            return;
        if (weaponItem == null)
        {
            currentWeaponObject.SetActive(false);
            currentWeaponObject = null;
        }
        else
        {
            if (System.Enum.TryParse(weaponItem.id, out WeaponName weaponName))
            {
                SwitchWeapon((int)weaponName);
            }
            GameManager.playerTransform.GetComponent<PlayerCombat>().hasWeapon = true;
        }
    }

    public void SwitchWeapon(int index = -1)
    {
        Debug.Log($"Index = {index}");
        if(index == -1)
        {
            if(currentWeaponObject != null)
            {
                currentWeaponObject.SetActive(!currentWeaponObject.activeSelf);
            }
            return;
        }
        currentWeaponObject = transform.GetChild(index).gameObject;
        currentWeaponObject.SetActive(true);
        var playerCombat = GameManager.playerTransform.GetComponent<PlayerCombat>();
        playerCombat.weaponCollider = currentWeaponObject.GetComponent<Collider>();
    }

    private enum WeaponObjectName
    {
        Staff_Basic,
        Staff_Medium,
        Wand_Basic,
        Simple_Axe_Variant,
        Sword1_1_3,
        MoonSword_6b
    }

    private enum WeaponName
    {
        완드,
        나뭇가지,
        나무완드,
        도끼,
        낡은검,
        양손검
    }
}


