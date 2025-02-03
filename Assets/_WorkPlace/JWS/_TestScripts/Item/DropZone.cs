using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    private QuickSlotsUI quickSlotsUI;

    private void Awake()
    {
        quickSlotsUI = transform.parent.GetComponent<QuickSlotsUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null && droppedObject.GetComponent<DraggableItem>() != null)
        {
            RectTransform rect = droppedObject.GetComponent<RectTransform>();
            if (rect != null)
            {
                string dropItem = droppedObject.GetComponent<InventoryTooltip>().currentItem.id;
                if (dropItem != string.Empty && dropItem != "")
                {
                    switch ( dropItem )
                    {
                        case "소형 체력포션":
                            quickSlotsUI.SetSlotItem(0);

                            break;
                        case "소형 마나포션":
                            quickSlotsUI.SetSlotItem(1);
                            break;
                    }
                }
                else
                {
                    Destroy(droppedObject);
                }
            }
        }
    }
}
