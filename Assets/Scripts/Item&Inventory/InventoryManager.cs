using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //변수
    #region
    private GameManager _gameManager;
    private QuestManager _questManager;

    public GameObject Player;
    public GameObject inventoryItemPrefab;
    public Inventory_Slot[] inventorySlots;
    public ItemData[] canGetItems;
    public Dictionary<int, ItemData> itemDicByID = new Dictionary<int, ItemData>();   // 퀘스트 아이템 ID : 1번~ / 소모성 아이템(포션) : 100번~ / 기타 아이템 ID : 1000번~
    [SerializeField] private int maxStackedItemsCount = 99;
    [HideInInspector] public Inventory_Item beTradedItem;
    #endregion
    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        _gameManager = gameObject.GetComponent<GameManager>();
        _questManager = gameObject.GetComponent<QuestManager>();

        SetItemDic();
    }

    private void Update()
    {
        HandleInputUseQuickSlotItem();
    }

    void PrintItemDic()
    {
        foreach (var item in itemDicByID)
        {
            Debug.Log($"Key: {item.Key}, Value: {item.Value}");
        }
    }

    private void SetItemDic()
    {
        foreach (ItemData item in canGetItems)
        {
            itemDicByID.Add(item.itemID, item);
        }
    }

    public void GetItem(int ID)
    {
        AddItemInInventory(itemDicByID[ID]);
    }

    public void AddItemInInventory(ItemData item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)         // 빈 슬롯 찾기
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
             
            if (itemInSlot == null && !CheckItem(item.itemID))  // 인벤토리 슬롯에 해당 아이템이 존재하지 않을 때
            {
                SetNewItem(item, slot);
                break;
            }
            else if(itemInSlot != null &&                           // 슬롯에 아이템이 있으면
                    CheckItem(item.itemID) &&                       // 인벤토리 슬롯에 해당 아이템이 존재할 때
                    item.stackable &&                               // 중첩 가능한 아이템인지 확인
                    itemInSlot.count < maxStackedItemsCount &&      // 최대 중첩 수보다 작은지 확인
                    itemInSlot.item == item)                        // 해당 아이템을 확인
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                break;
            }
        }
        //return false;
    }

    void SetNewItem(ItemData item, Inventory_Slot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        Inventory_Item inventory_Item = newItem.GetComponent<Inventory_Item>();
        inventory_Item.InitialiseItem(item);
    }

    // 해당 아이템의 획득 여부 확인 - 획득 시 슬롯 조사에 사용
    bool CheckItem(int targetItemID)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot != null &&                           // 슬롯에 해당 아이템이 1개 이상 존재하면
                itemInSlot.item.itemID == targetItemID &&
                itemInSlot.count >= 1)
            {
                return true;
            }
        }

        return false;
    }

    // 해당 아이템의 일정 개수 획득 여부 확인 - 거래에 사용
    public bool CheckItem(int targetItemID, int requiredCount)      
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot != null &&                           // 슬롯에 아이템이 있고
                itemInSlot.item.itemID == targetItemID &&       // 해당 아이템이 찾으려는 아이템이고
                itemInSlot.count >= requiredCount)                      // 일정 개수 이상을 보유 중이면 true
            {
                beTradedItem = itemInSlot;                      // 찾은 아이템 변수에 저장
                return true;
            }
        }

        return false;
    }

    // 거래 시 해당 아이템 개수 차감
    public void TradeItem()
    {
        if (beTradedItem.item is TradedItem tradedItem)
        {
            beTradedItem.count -= tradedItem.subtractCountOnTrade;
            beTradedItem.RefreshCount();
        }
    }

    void HandleInputUseQuickSlotItem()      // 퀵슬롯 아이템 사용 입력키 관리
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseItem(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            UseItem(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            UseItem(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            UseItem(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            UseItem(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            UseItem(5);
    }

    public void UseItem(int quickSlotIndex)
    {
        Inventory_Slot slot = inventorySlots[quickSlotIndex];
        Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

        if(itemInSlot != null && itemInSlot.item.itemType == ItemData.type.usable)
            if (itemInSlot.item is IUsable usableItem)
            {
                usableItem.UseItem();
                itemInSlot.count--;
                itemInSlot.RefreshCount();
            }
    }

}
