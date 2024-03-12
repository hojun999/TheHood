using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMove i_playerMove;

    public GameObject scannedObject;

    [HideInInspector] public bool isPlayerInteracting;
    // Start is called before the first frame update
    void Start()
    {
        i_playerMove = gameObject.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        SetInteractWithNPC();
        SetInteractWithItem();
    }

    void Ray()
    {
        Debug.DrawRay(transform.position, i_playerMove.moveDirection * 0.6f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, i_playerMove.moveDirection, 1f, LayerMask.GetMask("Object"));

        if (rayHit.collider)
        {
            scannedObject = rayHit.collider.gameObject;
            Debug.Log("������Ʈ ����(��ȣ�ۿ�)");
        }
        else
        {
            scannedObject = null;
            Debug.Log("������Ʈ ���� �ȵ�");
        }
    }

    private void SetInteractWithNPC()
    {
        if(Input.GetKeyDown(KeyCode.Space) && scannedObject && !isPlayerInteracting)
        {

        }
    }

    private void SetInteractWithItem()
    {

    }
}
