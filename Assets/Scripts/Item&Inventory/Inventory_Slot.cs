using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Slot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        Inventory_Item inventory_item = eventData.pointerDrag.GetComponent<Inventory_Item>();

        if (transform.childCount == 0)
            inventory_item.parentAfterDrag = transform;
        else
        {
            inventory_item.parentAfterDrag = transform;
            transform.GetChild(0).SetParent(inventory_item.parentBeforeDrag);       // 드롭한 곳에 아이템이 있으면
                                                                                    // 해당 아이템을 이동 전 슬롯의
                                                                                    // 자식으로 설정
        }
    }
}
