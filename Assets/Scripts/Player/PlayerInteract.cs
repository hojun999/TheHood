using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMove i_playerMove;
    [SerializeField]private DialogueManager i_dialogueManager;

    public GameObject scannedObject;
    [HideInInspector] public GameObject scannedObjectHolder;    // 직후에 대화한 npc오브젝트를 저장하기 위한 변수

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
            //Debug.Log("오브젝트 감지(상호작용)");
        }
        else
        {
            scannedObject = null;
            //Debug.Log("오브젝트 감지 안됨");
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
