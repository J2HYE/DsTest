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
                        case "Consumable001":
                            quickSlotsUI.SetSlotItem(0);

                            break;
                        case "Consumable002":
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
