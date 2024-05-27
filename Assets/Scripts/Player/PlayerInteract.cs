using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMove _playerMove;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private InventoryManager _inventoryManager;

    public GameObject scannedObject;
    [HideInInspector] public GameObject scannedObjectHolder;    // ������ ��ȭ�� npc������Ʈ�� �����ϱ� ���� ����

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
            //Debug.Log("������Ʈ ����(��ȣ�ۿ�)");
        }
        else
        {
            scannedObject = null;
            //Debug.Log("������Ʈ ���� �ȵ�");
        }
    }       // �÷��̾� ������ �������� ray ���

    private void HandleInputInteractWithNPC()       // npc���� ��ȭ ��ȣ�ۿ�
    {
        if (Input.GetKeyDown(KeyCode.Space) && scannedObject != null && scannedObject.CompareTag("NPC"))
        {
            _playerMove.rb.velocity = new Vector2(0, 0); // ��ȭ ���� �� �ܿ� ������ ����

            _dialogueManager.StartDialogueOnInteract(scannedObject);
            scannedObjectHolder = scannedObject;
        }
    }

    private void HandleInteracWithItem()            // ������ ���� ��ȣ�ۿ�
    {
        if (Input.GetKeyDown(KeyCode.E) && scannedObject != null && scannedObject.CompareTag("Item"))
        {
            ItemObject itemObj = scannedObject.GetComponent<ItemObject>();

            if(itemObj != null)
            {
                _inventoryManager.GetItem(itemObj.itemData.itemID);
                scannedObject.SetActive(false);     // ȹ���� ������ ������Ʈ ��Ȱ��ȭ
            }
            else
            {
                Debug.Log("��ȣ�ۿ��� ������Ʈ�� ������ �����Ͱ� �Ҵ�Ǿ� ���� ����");
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && scannedObject == null)
        {
            Debug.Log("�������� ��ó�� ����");
        }
    }
}
