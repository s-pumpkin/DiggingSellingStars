using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public ItemData MyItemData;

    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    private Vector3 _originalPosition;
    public bool isDrag;
    public bool isCommodity;

    public Transform originalParent;

    public Image MyImage;
    private void Awake()
    {
        if (isCommodity)
            return;
        BackPackManager.Instance.RegisterGameObjectItemUIDictionary(this.gameObject, this);
    }

    private void OnDestroy()
    {
        if (isCommodity)
            return;
        BackPackManager.Instance.UnRegisterGameObjectItemUIDictionary(this.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCommodity)
            return;
        if (_originalPosition == Vector3.zero)
            _originalPosition = rectTransform.anchoredPosition;
        transform.SetParent(BackPackManager.Instance.DragTemp.transform);
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCommodity)
            return;
        canvasGroup.blocksRaycasts = false;
        DragPos(eventData);
    }
    private void DragPos(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCommodity)
            return;
        canvasGroup.blocksRaycasts = true;

        Debug.Log(eventData.pointerCurrentRaycast.depth);
        if (eventData.pointerCurrentRaycast.depth == 0 || !eventData.pointerCurrentRaycast.gameObject.CompareTag("BuyUI"))
        {
            BackToTheBackpack();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("BuyUI"))
        {
            transform.SetParent(ShopManager.Instance.SellObjectPool.transform);
        }

        isDrag = false;
    }

    public void BackToTheBackpack()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = _originalPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCommodity)
            return;
        BackPackManager.Instance.InfoText.text = MyItemData.ItemInfo;
    }

    public void SetIamge()
    {
        MyImage.sprite = MyItemData.ItemImage;
    }
}
