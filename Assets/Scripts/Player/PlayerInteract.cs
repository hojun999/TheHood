using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMove _playerMove;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private InventoryManager _inventoryManager;

    public GameObject scannedObject;
    [HideInInspector] public GameObject scannedObjectHolder;    // 직전에 대화한 npc오브젝트를 저장하기 위한 변수

    [HideInInspector] public bool isPlayerInteracting;
    // Start is called before the first frame update
    void Start()
    {
        _playerMove = gameObject.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRay();
        HandleInputInteractWithNPC();
        HandleInteracWithItem();
    }

    void HandleRay()
    {
        Debug.DrawRay(transform.position, _playerMove.moveDirection * 0.6f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, _playerMove.moveDirection, 1f, LayerMask.GetMask("Object"));

        if (rayHit.collider)
        {
            scannedObject = rayHit.collider.gameObject;
            //Debug.Log("오브젝트 감지(상호작용)");
        }
        else
        {
            scannedObject = null;
            //Debug.Log("오브젝트 감지 안됨");
        }
    }       // 플레이어 움직임 방향으로 ray 쏘기

    private void HandleInputInteractWithNPC()       // npc와의 대화 상호작용
    {
        if (Input.GetKeyDown(KeyCode.Space) && scannedObject != null && scannedObject.CompareTag("NPC"))
        {
            _playerMove.rb.velocity = new Vector2(0, 0); // 대화 시작 시 잔여 움직임 방지

            _dialogueManager.StartDialogueOnInteract(scannedObject);
            scannedObjectHolder = scannedObject;
        }
    }

    private void HandleInteracWithItem()            // 아이템 습득 상호작용
    {
        if (Input.GetKeyDown(KeyCode.E) && scannedObject != null && scannedObject.CompareTag("Item"))
        {
            ItemObject itemObj = scannedObject.GetComponent<ItemObject>();

            if(itemObj != null)
            {
                _inventoryManager.GetItem(itemObj.itemData.itemID);
                scannedObject.SetActive(false);     // 획득한 아이템 오브젝트 비활성화
            }
            else
            {
                Debug.Log("상호작용한 오브젝트에 아이템 데이터가 할당되어 있지 않음");
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && scannedObject == null)
        {
            Debug.Log("아이템이 근처에 없음");
        }
    }
}
