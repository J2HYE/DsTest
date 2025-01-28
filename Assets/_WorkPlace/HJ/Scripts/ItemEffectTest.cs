using System;
using UnityEngine;

public class ItemEffectTest : MonoBehaviour
{
    [SerializeField] private string test = "Weapon001";

    private void Update()
    {
        if(InputManager.InputActions.actions["SkillUI"].triggered)
        {
            ItemEffectManager.Instance.ApplyItemEffect(InventoryManager.InventoryList[0]);
            Debug.Log("K");
        }

        if (InputManager.InputActions.actions["ESC"].triggered)
        {
            ItemManager.Instance.AddItemLogic(test);
            Debug.Log("ESC");
        }
    }
}
