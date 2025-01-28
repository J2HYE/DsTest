using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.MaterialProperty;

public class ShopitemUI : MonoBehaviour,IPointerClickHandler
{
    private Item thisItem;
    private ShopUI shopUI;
    private int currentItemIndex;
    private int currentShopType;

    private void ShowItem()
    {
        TextMeshProUGUI[] text = transform.GetComponentsInChildren<TextMeshProUGUI>();
        transform.GetChild(2).GetComponent<Image>().sprite = thisItem.sprite;
        text[0].text = thisItem.name;
        text[1].text = thisItem.costValue.ToString();
    }

    public void SetItemInfo(Item item, ShopUI shopUI, int index, int shopType)
    {
        this.thisItem = item;
        this.shopUI = shopUI;
        this.currentItemIndex = index;
        this.currentShopType = shopType;
        ShowItem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopUI.ShowItemInfo(currentItemIndex, currentShopType);
    }
}
