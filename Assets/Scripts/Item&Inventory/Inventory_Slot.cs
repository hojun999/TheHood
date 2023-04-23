using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Slot : MonoBehaviour, IDropHandler
{

    // Drag and Drop
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            Inventory_Item inventory_item = eventData.pointerDrag.GetComponent<Inventory_Item>();
            inventory_item.parentAfterDrag = transform;
        }
    }
}
