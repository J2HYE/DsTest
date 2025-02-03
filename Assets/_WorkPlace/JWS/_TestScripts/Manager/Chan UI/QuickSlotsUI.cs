using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    [SerializeField] private Animator[] quickSlotsAnimator;
    [SerializeField] private Image[] quickSlotImages;
    [SerializeField] private TextMeshProUGUI[] quickSlotText;
    private PlayerData playerData;
    private Item[] quickSlotItems = new Item[2];
    private BasicTimer[] quickSlotTimer = new BasicTimer[2];
    private string[] condition = { "소형 체력포션", "소형 마나포션" };

    private void Start()
    {
        InitQuickSlotItems();
        if (playerData == null) { playerData = CharacterManager.PlayerCharacterData; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TryUseQuickSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TryUseQuickSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ItemManager.Instance.AddItemLogic(condition[0], 1);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ItemManager.Instance.AddItemLogic(condition[1], 1);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            playerData.currentHp = 1;
            playerData.currentMp = 1;
            playerData.staminaCurrent = 1;
        }

        CheckQuickSlotItems();
    }

    private void TryUseQuickSlot(int slotIndex)
    {
        bool isUseQuickSlot = false;
        bool isHpnMpCheck = slotIndex == 0 ? playerData.currentHp == playerData.maxHp : playerData.currentMp == playerData.maxMp;
        if (slotIndex < 0 || slotIndex >= quickSlotTimer.Length || isHpnMpCheck) return;

        string itemType = condition[slotIndex];
        if (!isUseQuickSlot && InventoryManager.Instance.GetItemQuantity(itemType) > 0)
        {
            isUseQuickSlot = true;
            UsedQuickSlot(itemType, slotIndex);
        }
        isUseQuickSlot = false;
    }

    private void UsedQuickSlot(string itemType, int slotIndex)
    {
        if (quickSlotItems[slotIndex] == null || quickSlotTimer[slotIndex] == null || quickSlotTimer[slotIndex].IsRunning) return;
        TimerManager.Instance.StartTimer(quickSlotTimer[slotIndex]);
        ItemManager.Instance.UseItem(InventoryManager.Instance.FindInventoryItem(itemType));
    }

    private void InitQuickSlotItems()
    {
        for (int i = 0; i < quickSlotItems.Length; i++)
        {
            quickSlotTimer[i] = null;
            quickSlotItems[i] = null;
            quickSlotText[i].text = string.Empty;
        }
    }

    private void CheckQuickSlotItems()
    {
        for (int i = 0; i < quickSlotTimer.Length; i++)
        {
            int quantiy = InventoryManager.Instance.GetItemQuantity(condition[i]);
            bool hasItem = quickSlotItems[i] != null;
            if (quickSlotsAnimator[i] != null)
            {
                if (quickSlotTimer[i] != null && quickSlotTimer[i].IsRunning)
                {
                    quickSlotsAnimator[i].SetTrigger("Hover");
                    if (!hasItem)
                    {
                        TimerManager.Instance.StopTimer(quickSlotTimer[i]);
                    }
                }
                else
                {
                    quickSlotsAnimator[i].SetTrigger("Normal");
                }
                UpdateQuickSlotUI(i, hasItem, quantiy);
            }
        }
    }

    private void UpdateQuickSlotUI(int slotIndex, bool isActive, int itemQuantity)
    {
        if (isActive)
        {
            quickSlotImages[slotIndex].enabled = true;
            quickSlotImages[slotIndex].fillAmount = quickSlotTimer[slotIndex].IsRunning  ? 
                quickSlotTimer[slotIndex].RemainingPercent :
                itemQuantity > 0 ? 0 : 1;
            quickSlotText[slotIndex].transform.parent.gameObject.SetActive(true);
            quickSlotText[slotIndex].text = itemQuantity.ToString();
        }
        else
        {
            quickSlotImages[slotIndex].enabled = false;
            quickSlotText[slotIndex].transform.parent.gameObject.SetActive(false);
            quickSlotText[slotIndex].text = string.Empty;
        }
    }

    public void SetSlotItem(int index)
    {
        quickSlotItems[index] = InventoryManager.Instance.FindInventoryItem(condition[index]);
        if (quickSlotTimer[index] == null)
        {
            quickSlotTimer[index] = new BasicTimer(10f);
        }
    }

    public bool GetQuicSlot(int index)
    {
        return quickSlotItems[index] != null;
    }
}
