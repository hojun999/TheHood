using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    

    public Image itemImage;
    public Text countText;
    public GameObject selectedSlot;

    public ItemData item;           // �ش� ĭ�� �ִ� ������ ����
    public int count = 1;           // ������ ���� ī��Ʈ

    public Transform parentBeforeDrag;      // �̵� �� ��ġ ����
    public Transform parentAfterDrag;       // �̵��� ��ġ ����

    public void InitialiseItem(ItemData newItem)
    {
        item = newItem;
        itemImage.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 0;
        countText.gameObject.SetActive(textActive);

        if (!textActive)
            Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = false;
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
        itemImage.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;        // �������� �̵� �� ��ġ ����
        selectedSlot.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedSlot.SetActive(false);
    }
}
