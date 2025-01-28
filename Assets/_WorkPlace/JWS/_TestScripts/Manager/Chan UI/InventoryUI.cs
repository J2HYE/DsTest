using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject itemInfoObject;
    [SerializeField] private TextMeshProUGUI[] itemInfoTextField;
    [SerializeField] private Image itemInfoImageField;

    private Button[] buttons;
    private int currentButtonIndex = 6;
    private Dictionary<string, List<Item>> categorizedItems = new Dictionary<string, List<Item>>();

   

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        AddButtonListeners();
        OnButtonClick(currentButtonIndex);
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
        currentButtonIndex = 6;
    }

    private void Start()
    {

    }

    private void CategorizeItems()
    {
        categorizedItems.Clear();

        foreach (var item in InventoryManager.InventoryList)
        {
            string itemtag = item.type.ToString();
            if (!categorizedItems.ContainsKey(itemtag))
            {
                categorizedItems[itemtag] = new List<Item>();
            }
            categorizedItems[itemtag].Add(item);
        }
    }

    public void UpdateUI()
    {
        CategorizeItems();
        ClearUI();
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("버튼이 설정되지 않았거나 버튼 배열이 비어있습니다.");
            return;
        }
        switch (currentButtonIndex)
        {
            case 7:
                Debug.Log("제거 처리로직 필요");
                break;

            case 8:
                GameStateMachine.Instance.ChangeState(GameSystemState.MainMenu);
                break;

            case 9:
                Debug.Log("장착 처리로직 필요");
                break;

            default:
                if (currentButtonIndex >= 0 && currentButtonIndex < 7)
                {
                    string selectedTag = ((CategotyItemType)currentButtonIndex).ToString();

                    if (selectedTag == CategotyItemType.전체아이템.ToString())
                    {
                        foreach (var category in categorizedItems.Values)
                        {
                            foreach (var item in category)
                            {
                                CreateItemUI(item);
                            }
                        }
                    }
                    else if (categorizedItems.ContainsKey(selectedTag))
                    {
                        foreach (var item in categorizedItems[selectedTag])
                        {
                            CreateItemUI(item);
                        }
                    }
                }
                break;
        }
    }


    private void ClearUI()
    {
        foreach (Transform child in itemsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateItemUI(Item item)
    {
        var inventoryItem = Instantiate(itemPrefab, itemsParent);
        inventoryItem.GetComponent<InventoryTooltip>().currentItem = item;
        inventoryItem.GetComponent<InventoryTooltip>().InventorytooltipWindow = itemInfoObject;
        inventoryItem.GetComponent<InventoryTooltip>().textPoint = itemInfoTextField;
        inventoryItem.GetComponent<InventoryTooltip>().ItemImage = itemInfoImageField;
    }

    public void AddButtonListeners()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            if (index >= 0 && index < 7)
            {
                Animator buttonAnimator = buttons[index].animator;
                buttonAnimator.SetTrigger("Idle");
            }
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
        currentButtonIndex = buttonIndex;
        if (currentButtonIndex >= 0 && currentButtonIndex < 7)
        {
            Animator buttonAnimator = buttons[buttonIndex].animator;
            buttonAnimator.SetTrigger("Selected");
        }
        UpdateUI();
    }
}

public enum CategotyItemType
{
    무기,         // Weapon
    방어구,       // Armor
    소모품,       // Consumable
    퀘스트,       // QuestItem
    제작재료,     // Material
    장신구,        // Accessory
    전체아이템
}
