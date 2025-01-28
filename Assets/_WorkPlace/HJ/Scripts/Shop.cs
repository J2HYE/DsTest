using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Variables
    [SerializeField] private string shopId;
    [SerializeField] private ShopData shopData;
    private float valueReductionRate;

    private bool isPlayerInRange = false;

    #endregion

    private void Start()
    {
        //상점데이터 초기화
        shopData = ShopManager.shopsDataList.FirstOrDefault(i => i.shopId == shopId);
        shopData.Initialize();

        //가치감소율
        valueReductionRate = shopData.valueReductionRate;
    }

    private void Update()
    {
        if (isPlayerInRange && InputManager.InputActions.actions["Interact"].triggered)
        {
            // ShopManager.Instance.OpenShop(shopData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UIManager.Instance.InteractTextPopup("F12", shopData.shopName + "상점", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.Instance.InteractTextPopup("F12", shopData.shopName + "상점", false);
            UIManager.Instance.UIClose();
        }
    }

    public void PurchaseItemButton()
    {
    }

    public void SellItemButton()
    {
    }
}
