using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //����
    #region
    private GameManager _gameManager;
    private QuestManager _questManager;

    public GameObject Player;
    public GameObject inventoryItemPrefab;
    public Inventory_Slot[] inventorySlots;
    public ItemData[] canGetItems;
    public Dictionary<int, ItemData> itemDicByID = new Dictionary<int, ItemData>();   // ����Ʈ ������ ID : 1��~ / �Ҹ� ������(����) : 100��~ / ��Ÿ ������ ID : 1000��~
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
        for (int i = 0; i < inventorySlots.Length; i++)         // �� ���� ã��
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
             
            if (itemInSlot == null && !CheckItem(item.itemID))  // �κ��丮 ���Կ� �ش� �������� �������� ���� ��
            {
                SetNewItem(item, slot);
                break;
            }
            else if(itemInSlot != null &&                           // ���Կ� �������� ������
                    CheckItem(item.itemID) &&                       // �κ��丮 ���Կ� �ش� �������� ������ ��
                    item.stackable &&                               // ��ø ������ ���������� Ȯ��
                    itemInSlot.count < maxStackedItemsCount &&      // �ִ� ��ø ������ ������ Ȯ��
                    itemInSlot.item == item)                        // �ش� �������� Ȯ��
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

    // �ش� �������� ȹ�� ���� Ȯ�� - ȹ�� �� ���� ���翡 ���
    bool CheckItem(int targetItemID)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot != null &&                           // ���Կ� �ش� �������� 1�� �̻� �����ϸ�
                itemInSlot.item.itemID == targetItemID &&
                itemInSlot.count >= 1)
            {
                return true;
            }
        }

        return false;
    }

    // �ش� �������� ���� ���� ȹ�� ���� Ȯ�� - �ŷ��� ���
    public bool CheckItem(int targetItemID, int requiredCount)      
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Inventory_Slot slot = inventorySlots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot != null &&                           // ���Կ� �������� �ְ�
                itemInSlot.item.itemID == targetItemID &&       // �ش� �������� ã������ �������̰�
                itemInSlot.count >= requiredCount)                      // ���� ���� �̻��� ���� ���̸� true
            {
                beTradedItem = itemInSlot;                      // ã�� ������ ������ ����
                return true;
            }
        }

        return false;
    }

    // �ŷ� �� �ش� ������ ���� ����
    public void TradeItem()
    {
        if (beTradedItem.item is TradedItem tradedItem)
        {
            beTradedItem.count -= tradedItem.subtractCountOnTrade;
            beTradedItem.RefreshCount();
        }
    }

    void HandleInputUseQuickSlotItem()      // ������ ������ ��� �Է�Ű ����
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
