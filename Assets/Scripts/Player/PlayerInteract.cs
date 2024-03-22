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
    }

    private void HandleInputInteractWithNPC()
    {
        if (Input.GetKeyDown(KeyCode.Space) && scannedObject != null && scannedObject.CompareTag("NPC"))
        {
            _dialogueManager.StartDialogueOnInteract(scannedObject);
            scannedObjectHolder = scannedObject;
        }
    }

    private void HandleInteracWithItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && scannedObject != null && scannedObject.CompareTag("Item"))
        {
            _inventoryManager.GetItem(scannedObject.GetComponent<Item>().itemID);
        }
    }
}
