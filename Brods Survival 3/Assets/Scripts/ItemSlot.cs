using UnityEngine.EventSystems;
using UnityEngine;


public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");

        DragDrop itemDragHandler = eventData.pointerDrag.GetComponent<DragDrop>();

        // If there is no ItemDragHandler component, return
        if (!itemDragHandler)
            return;

        // Set the parent of the dragged item to this slot
        itemDragHandler.transform.SetParent(transform);

        // Set the local position of the dragged item to the center of this slot
        itemDragHandler.transform.localPosition = Vector3.zero;

        // Ensure the RectTransform's pivot and anchors are set to center
        RectTransform rectTransform = itemDragHandler.GetComponent<RectTransform>();
        if (rectTransform)
        {
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }
}