using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItemTextDisplay : MonoBehaviour
{
    public GameObject itemTextPrefab;
    public Transform itemListParent;
    public float displayPickUPItemTextTime = 1f;

    private Dictionary<GameObject, float> PicUpitemTextList = new Dictionary<GameObject, float>();

    private void Start()
    {
    }

    public void AddItem(string itemName, Sprite sprite = null)
    {
        GameObject newItemText = Instantiate(itemTextPrefab, itemListParent);
        newItemText.GetComponent<TextMeshProUGUI>().text = itemName;
        if (sprite != null ) newItemText.GetComponentInChildren<Image>().sprite = sprite;
        PicUpitemTextList.Add(newItemText, Time.time);
        UpdateItemListLayout();
        Invoke(nameof(CheckAndRemoveItems), displayPickUPItemTextTime);
    }


    private void UpdateItemListLayout()
    {
   
    }

    private void CheckAndRemoveItems()
    {
        // 현재 시간
        float currentTime = Time.time;
        List<GameObject> itemsToRemove = new List<GameObject>();
        foreach (var item in PicUpitemTextList)
        {
            if (currentTime - item.Value >= displayPickUPItemTextTime)
            {
                itemsToRemove.Add(item.Key);
            }
        }
        foreach (var item in itemsToRemove)
        {
            PicUpitemTextList.Remove(item);
            Destroy(item);
        }
        UpdateItemListLayout();
    }
}
