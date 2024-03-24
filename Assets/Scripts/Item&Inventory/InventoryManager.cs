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
    public Item[] canGetItems;
    public Dictionary<int, Item> itemDicByID = new Dictionary<int, Item>();   // 퀘스트 아이템 ID : 1번~ / 소모성 아이템(포션) : 100번~ 
                                                // 기타 아이템 ID : 1000번~
    [SerializeField] private int maxStackedItems = 99;

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
        //if (gameManager.isGetAlreadyPosionNum == 2)
        //    gameManager.isGetAlreadyPosionNum = 0;
    }

    private void SetItemDic()
    {
        foreach (Item item in canGetItems)      // Item은 scriptableobject라서 호환이 안되는것 같음.---------------------------------------------
        {
            itemDicByID.Add(item.itemID, item);
        }
    }

    public void GetItem(int ID)
    {
        AddItemInInventory(itemDicByID[ID]);
    }

    public void AddItemInInventory(Item item)
    {
        for (int i = 0; i < inventory_slots.Length; i++)         // 빈 슬롯 찾기
        {
            Inventory_Slot slot = inventory_slots[i];
            Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();

            if (itemInSlot == null)
            {
                Debug.Log("아이템 획득");
                SetNewItem(item, slot);
                break;
                //return true;
            }
            else if(itemInSlot != null &&                   // 슬롯에 아이템이 있으면
                    item.stackable &&                       // 중첩 가능한 아이템인지 확인
                    itemInSlot.count < maxStackedItems &&   // 최대 중첩 수보다 작은지 확인
                    itemInSlot.item == item)                // 해당 아이템 확인
            {
                Debug.Log("아이템 중첩");
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                break;
            }
        }

        Debug.Log("인벤토리가 다 찼고, 중첩 가능한 아이템 없음");

        //return false;
    }

    void SetNewItem(Item item, Inventory_Slot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        Inventory_Item inventory_Item = newItem.GetComponent<Inventory_Item>();
        inventory_Item.InitialiseItem(item);
    }

    void UseItem(int index)
    {
        // 소비 개수에 따른 카운트 차감
        Inventory_Slot slot = inventory_slots[index];       // 해당 부분 퀵슬롯 변수 따로 선언하여 등록 및 수정-----------------------------------
        Inventory_Item itemInSlot = slot.GetComponentInChildren<Inventory_Item>();
        if(itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(item.consumable)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }

                // 사용 효과
                if (item.actionType == ActionType.Healing)        // 체력 포션
                {
                    Player.GetComponent<PlayerController>().curHp += 10;
                }
                else if (item.actionType == ActionType.enegyUp)
                {
                    Player.GetComponent<PlayerController>().curMp += 15;
                }
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
                //        Player.GetComponent<PlayerController>().moveSpeed = 3f;     // 10초 뒤 원래 이동속도로. (movespeed는 인스펙터창에서 조정하므로 잘 확인)
                //}
            }       
            
        }
    }

    private void HandleInputUseQuickSlotItem()      // 퀵슬롯 아이템 사용 입력키 관리
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

    //public void DestroyQuestItemAndTradeEtcItem()       // 거래 아이템 개수에 따른 각 상인npc의 대화 출력 및 아이템 교환
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
