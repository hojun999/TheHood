using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    

    public Image image;
    public Text countText;
    public GameObject selectedSlot;

    [HideInInspector] public ItemData item;         // �ش� ĭ�� �ִ� ������ ����
    public int count = 1;     // ������ ���� ī��Ʈ
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(ItemData newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        selectedSlot.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        selectedSlot.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedSlot.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedSlot.SetActive(false);
    }
}
