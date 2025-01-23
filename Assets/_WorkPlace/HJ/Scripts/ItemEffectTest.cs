using System;
using UnityEngine;

public class ItemEffectTest : MonoBehaviour
{
    [SerializeField] private string testItemId = "Item041";

    private void Start()
    {
        ItemManager.Instance.AddItemLogic(testItemId);
    }

    private void Update()
    {
        if (InputManager.InputActions.actions["Interact"].triggered)
        {
            Debug.Log("이펙트 매니저 초기화 테스트");
            foreach(var item in InventoryManager.InventoryList)
            {
                item.effect.Initialize(item);
            }
        }

        if(InputManager.InputActions.actions["SkillUI"].triggered)
        {
            ItemEffectManager.Instance.ApplyItemEffect(InventoryManager.InventoryList[0]);
        }

        if (InputManager.InputActions.actions["ESC"].triggered)
        {
            //ItemEffectManager.Instance.UnequipmentEffect(ItemEffectManager.Instance.EquippedWeapon);
        }
    }
}
