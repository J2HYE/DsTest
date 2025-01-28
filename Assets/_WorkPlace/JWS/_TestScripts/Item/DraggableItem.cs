using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Canvas mainCanvas;
    private GameObject dragImageObject;
    private Image dragImage;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas.renderMode != RenderMode.ScreenSpaceOverlay && mainCanvas.worldCamera == null)
        {
            mainCanvas.worldCamera = Camera.main;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (mainCanvas == null) return;
        canvasGroup.blocksRaycasts = false;
        CreateDragImage();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mainCanvas == null || dragImageObject == null) return;
        dragImageObject.GetComponent<RectTransform>().position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (mainCanvas == null) return;
        canvasGroup.blocksRaycasts = true;
        if (!eventData.pointerEnter || eventData.pointerEnter.GetComponent<DropZone>() == null)
        {
            DestroyDragImage();
        }
    }

    private void CreateDragImage()
    {
        dragImageObject = new GameObject("Drag Image", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        dragImageObject.transform.SetParent(mainCanvas.transform, false);
        dragImage = dragImageObject.GetComponent<Image>();
        dragImage.sprite = transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        dragImage.color = new Color(1, 1, 1, 0.6f);
        CanvasGroup dragCanvasGroup = dragImageObject.GetComponent<CanvasGroup>();
        dragCanvasGroup.blocksRaycasts = false;
    }

    private void DestroyDragImage()
    {
        if (dragImageObject != null)
        {
            Destroy(dragImageObject);
            dragImageObject = null;
        }
    }
}
