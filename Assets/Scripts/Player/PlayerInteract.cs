using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMove i_playerMove;
    [SerializeField]private DialogueManager i_dialogueManager;

    public GameObject scannedObject;
    [HideInInspector] public GameObject scannedObjectHolder;    // ���Ŀ� ��ȭ�� npc������Ʈ�� �����ϱ� ���� ����

    [HideInInspector] public bool isPlayerInteracting;
    // Start is called before the first frame update
    void Start()
    {
        i_playerMove = gameObject.GetComponent<PlayerMove>();
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
        Debug.DrawRay(transform.position, i_playerMove.moveDirection * 0.6f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, i_playerMove.moveDirection, 1f, LayerMask.GetMask("Object"));

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
    }


    private void SetInteractWithNPC()
    {

    }

    private void SetInteractWithItem()
    {

    }

    private void HandleInputInteractWithNPC()
    {
        if (Input.GetKeyDown(KeyCode.Space) && scannedObject != null && scannedObject.CompareTag("NPC"))
        {
            i_dialogueManager.StartDialogueOnInteract(scannedObject);
            scannedObjectHolder = scannedObject;
        }
    }

    private void HandleInteracWithItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && scannedObject != null && scannedObject.CompareTag("Item"))
            SetInteractWithItem();
    }
}
