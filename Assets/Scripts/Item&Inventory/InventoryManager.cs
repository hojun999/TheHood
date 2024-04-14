using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameManager _gameManager;
    private QuestManager _questManager;

    public GameObject Player;
    public GameObject inventoryItemPrefab;
    public Inventory_Slot[] inventory_slots;
    public ItemData[] canGetItems;
    public Dictionary<int, ItemData> itemDicByID = new Dictionary<int, ItemData>();   // ����Ʈ ������ ID : 1��~ / �Ҹ� ������(����) : 100��~ / ��Ÿ ������ ID : 1000��~
    [SerializeField] private int maxStackedItemsCount = 99;
    [HideInInspector] public Inventory_Item beTradedItem;

    private void Start()
    {
        _gameManager = gameObject.GetComponent<GameManager>();
        _questManager = gameObject.GetComponent<QuestManager>();

        SetItemDic();

        foreach (var item in itemDicByID)
        {
            Debug.Log($"Key: {item.Key}, Value: {item.Value}");
        }
    }

    private void Update()
    {
        HandleInputUseQuickSlotItem();
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
        for (int i = 0; i < inventory_slots.Length; i++)         // �� ���� ã��
        {
            Inventory_Slot slot = inventory_slots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
             
            if (itemInSlot == null)
            {
                Debug.Log("������ ȹ��");
                SetNewItem(item, slot);
                break;
                //return true;
            }
            else if(itemInSlot != null &&                   // ���Կ� �������� ������
                    item.stackable &&                       // ��ø ������ ���������� Ȯ��
                    itemInSlot.count < maxStackedItemsCount &&   // �ִ� ��ø ������ ������ Ȯ��
                    itemInSlot.item == item)                // �ش� ������ Ȯ��
            {
                Debug.Log("������ ��ø");
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                break;
            }
        }

        Debug.Log("�κ��丮�� �� á��, ��ø ������ ������ ����");

        //return false;
    }

    void SetNewItem(ItemData item, Inventory_Slot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        Inventory_Item inventory_Item = newItem.GetComponent<Inventory_Item>();
        inventory_Item.InitialiseItem(item);
    }

    public bool CheckItem(int targetItemID, int count)      // ���������� �κ��丮�� �˻��Ͽ� targetitem�� ���� ���� ���� ������ Ȯ��
    {
        for (int i = 0; i < inventory_slots.Length; i++)
        {
            Inventory_Slot slot = inventory_slots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot != null &&                           // ���Կ� �������� �ְ�
                itemInSlot.item.itemID == targetItemID &&       // �ش� �������� ã������ �������̰�
                itemInSlot.count >= count)                      // ���� ���� �̻��� ���� ���̸� true
            {
                beTradedItem = itemInSlot;                      // ã�� ������ ������ ����
                return true;
            }
        }

        return false;
    }

    //public void TradeItem(int targetItemID, int substractedCount)
    //{
    //    for (int i = 0; i < inventory_slots.Length; i++)
    //    {
    //        Inventory_Slot slot = inventory_slots[i];
    //        Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

    //        if (itemInSlot != null &&                           // ���Կ� �������� �ְ�
    //            itemInSlot.item.itemID == targetItemID &&       // �ش� �������� ã������ �������̰�
    //            itemInSlot.count >= substractedCount)           // ���� ���� �̻��� ���� ���̸� true
    //        {     
    //            itemInSlot.count -= substractedCount;           // �ش� �������� ���� ����
    //            itemInSlot.RefreshCount();                      // ui�� �ݿ�
    //        }
    //    }
    //}

    public void TradeItem()
    {

        if (beTradedItem.item is TradedItem tradedItem)
        {
            Debug.Log("True");
            beTradedItem.count -= tradedItem.subtractCountOnTrade;
            beTradedItem.RefreshCount();
        }
    }

    //void UseItem(int index)
    //{
    //    // �Һ� ������ ���� ī��Ʈ ����
    //    Inventory_Slot slot = inventory_slots[index];       // �ش� �κ� ������ ���� ���� �����Ͽ� ��� �� ����-----------------------------------
    //    Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
    //    if(itemInSlot != null)
    //    {
    //        Item item = itemInSlot.item;
    //        if(item.consumable)
    //        {
    //            itemInSlot.count--;
    //            if(itemInSlot.count <= 0)
    //            {
    //                Destroy(itemInSlot.gameObject);
    //            }
    //            else
    //            {
    //                itemInSlot.RefreshCount();
    //            }

    //            // ��� ȿ��
    //            if (item.actionType == ActionType.Healing)        // ü�� ����
    //            {
    //                Player.GetComponent<PlayerController>().curHp += 10;
    //            }
    //            else if (item.actionType == ActionType.enegyUp)
    //            {
    //                Player.GetComponent<PlayerController>().curMp += 15;
    //            }
    //else if (item.actionType == ActionType.speedUp)
    //{
    //    float useTime = 0;
    //    float endTime = 10f;
    //    bool beingBuffed = true;

    //    useTime += Time.deltaTime;
    //    if(useTime >= endTime)
    //    {
    //        useTime = 0;
    //        beingBuffed = false;
    //    }

    //    if (beingBuffed)
    //        Player.GetComponent<PlayerController>().moveSpeed = 4.5f;
    //    else
    //        Player.GetComponent<PlayerController>().moveSpeed = 3f;     // 10�� �� ���� �̵��ӵ���. (movespeed�� �ν�����â���� �����ϹǷ� �� Ȯ��)
    //}
    //        }       

    //    }
    //}

    private void HandleInputUseQuickSlotItem()      // ������ ������ ��� �Է�Ű ����
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    UseItem(0);
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //    UseItem(1);
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    UseItem(2);
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //    UseItem(3);
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //    UseItem(4);
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //    UseItem(5);
    }

    //public void DestroyQuestItemAndTradeEtcItem()       // �ŷ� ������ ������ ���� �� ����npc�� ��ȭ ��� �� ������ ��ȯ
    //{
    //    for (int i = 0; i < inventory_slots.Length; i++)
    //    {
    //        Inventory_Slot slot = inventory_slots[i];
    //        Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

    //        if(itemInSlot != null)
    //        {
    //            Item item = itemInSlot.item;
    //            if(item.itemType == ItemType.Quest && questManager_lagacy.getItemNum_Quest2 == 100)
    //            {
    //                Destroy(itemInSlot.gameObject);
    //            }

    //            if (item.itemType == ItemType.etc_HpPosion && itemInSlot.count >= 2 && gameManager.scanObject.GetComponent<ObjData>().isPosionTraderNpc && gameManager.isGetAlreadyPosionNum == 0)
    //            {
    //                //gameManager.getPosionTradeTalkIndex = 1;
    //                playerController.GetItem(1);
    //                itemInSlot.count -= 2;
    //                itemInSlot.RefreshCount();
    //                if (itemInSlot.count == 0)
    //                {
    //                    Destroy(itemInSlot.gameObject);
    //                    //gameManager.getPosionTradeTalkIndex = 2;
    //                }
    //            }

    //            if (item.itemType == ItemType.etc_EnergyPosion && itemInSlot.count >= 2 && gameManager.scanObject.GetComponent<ObjData>().isPosionTraderNpc && gameManager.isGetAlreadyPosionNum == 0)
    //            {
    //                //gameManager.getPosionTradeTalkIndex = 1;
    //                playerController.GetItem(6);
    //                itemInSlot.count -= 2;
    //                itemInSlot.RefreshCount();
    //                if (itemInSlot.count == 0)
    //                    Destroy(itemInSlot.gameObject);
    //            }

    //            if (item.itemType == ItemType.etc_Stone && itemInSlot.count >= 3 && gameManager.scanObject.GetComponent<ObjData>().isWeaponTraderNpc)
    //            {
    //                //gameManager.getWeaponTradeTalkIndex = 1;
    //                playerController.maxHp += 15;
    //                //playerController.curHp += 15;
    //                itemInSlot.count -= 3;
    //                itemInSlot.RefreshCount();
    //                if (itemInSlot.count == 0)
    //                    Destroy(itemInSlot.gameObject);
    //            }
    //        }
    //    }
    //}
}
