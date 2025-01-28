using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform shopItemListParent;
    [SerializeField] private GameObject shopItemListPrefab;
    [SerializeField] private Transform shopItemInfoPosition;
    [SerializeField] private NPCData shopNpcData;

    private int currentShopType = 0;
    private TextMeshProUGUI[] uiText;
    private Button[] buttons;
    private Item currentInfoItem;

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<Button>(true);
        uiText = transform.GetComponentsInChildren<TextMeshProUGUI>(true);
        shopItemInfoPosition.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    public void UpdateUI()
    {
        ShowItemInfoClear();
        uiText[5].text = CharacterManager.PlayerCharacterData.gold.ToString();
        if (shopNpcData != null)
        {
            uiText[2].text = shopNpcData.name;
            ClearShopList();
            switch (currentShopType)
            {
                case 0:
                    for (int i = 0; i < shopNpcData.shopData.availableItems.Count; i++)
                    {
                        Item item = shopNpcData.shopData.availableItems[i];
                        GameObject gob = Instantiate(shopItemListPrefab, shopItemListParent);
                        gob.transform.GetComponent<ShopitemUI>().SetItemInfo(item, this, i, currentShopType);
                    }
                    break;
                case 1:
                    for (int i = 0; i < InventoryManager.InventoryList.Count; i++)
                    {
                        Item item = InventoryManager.InventoryList[i];
                        if (item.isEquired) continue;
                        GameObject gob = Instantiate(shopItemListPrefab, shopItemListParent);
                        gob.transform.GetComponent<ShopitemUI>().SetItemInfo(item, this, i, currentShopType);
                    }
                    break;
            }
        }
    }

    public void AddButtonListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    public void RemoveButtonListeners()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick(int buttonIndex)
    {
        if (buttonIndex < 2)
        {
            currentShopType = buttonIndex;
        }
        else
        {
            switch (currentShopType)
            {
                case 0:
                    if (shopItemInfoPosition.gameObject.activeSelf)
                    {
                        if (CharacterManager.PlayerCharacterData.gold < currentInfoItem.costValue * currentInfoItem.quantity)
                        {
                            GameStateMachine.Instance.ChangeState(GameSystemState.InfoMessage, "보유한 금액이 부족합니다.", true);
                            return;
                        }
                        ItemManager.Instance.PurchaseItem(currentInfoItem, currentInfoItem.quantity);
                    }
                    break;

                case 1:
                    if (shopItemInfoPosition.gameObject.activeSelf)
                    {
                        ItemManager.Instance.SellItem(currentInfoItem, currentInfoItem.quantity);
                    }
                    break;
            }
        }
        UpdateUI();

    }

    private void ClearShopList()
    {
        foreach (Transform t in shopItemListParent)
        {
            Destroy(t.gameObject);
        }
    }

    public void SetShopInfo(NPCData nPCData)
    {
        shopNpcData = nPCData;
        OnButtonClick(0);
    }

    private void ShowItemInfoClear()
    {
        currentInfoItem = null;
        shopItemInfoPosition.GetComponentInChildren<Image>().sprite = null;
        uiText[0].text = "";
        uiText[1].text = "";
        shopItemInfoPosition.gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);
    }

    public void ShowItemInfo(int index, int currentType)
    {
        shopItemInfoPosition.gameObject.SetActive(true);
        currentInfoItem = currentType == 0 ? shopNpcData.shopData.availableItems[index] : InventoryManager.InventoryList[index];
        shopItemInfoPosition.GetComponentInChildren<Image>().sprite = currentInfoItem.sprite;
        uiText[0].text = currentInfoItem.name;
        uiText[1].text = currentInfoItem.ToStringTMPro();
        buttons[2].gameObject.SetActive(true);
        buttons[2].transform.GetComponentInChildren<TextMeshProUGUI>().text = currentType == 0 ? "구입" : "판매";
    }
}
