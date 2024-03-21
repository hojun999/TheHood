using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerInteract i_playerInteract;
    private Rigidbody2D rb;

    private float h_value;
    private float v_value;

    [SerializeField] private float i_moveSpeed;

    [SerializeField] private float i_teleportPower;
    [SerializeField] private float teleport_cooltime;


    private bool isWalk;
    private bool isTeleporting;

    [HideInInspector] public Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        i_playerInteract = gameObject.GetComponent<PlayerInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveDirection();
        SetMove();

        if (Input.GetKeyDown(KeyCode.Space) && !isTeleporting && !i_playerInteract.isPlayerInteracting && i_playerInteract.scannedObject == null)
        {
            Teleport();
        }

    }

    private void FixedUpdate()
    {

    }

    private void GetMoveDirection()     // 입력된 값에 따른 이동 방향값 세팅
    {
        h_value = i_playerInteract.isPlayerInteracting ? 0 : Input.GetAxisRaw("Horizontal");
        v_value = i_playerInteract.isPlayerInteracting ? 0 : Input.GetAxisRaw("Vertical");

        if (h_value == 1)
            moveDirection = Vector2.right;
        else if (h_value == -1)
            moveDirection = Vector2.left;
        else if (v_value == 1)
            moveDirection = Vector2.up;
        else if (v_value == -1)
            moveDirection = Vector2.down;
    }

    private void SetMove()
    {
        if (h_value != 0 || v_value != 0)       // 이동 조건 세팅
            isWalk = true;
        else
        {
            isWalk = false;
            rb.velocity = Vector2.zero;
        }

        if (isWalk)     // 이동 세팅
        {
            rb.velocity = new Vector2(h_value, v_value).normalized * i_moveSpeed;
        }
    }

    private void Teleport()     // 순간이동 실행
    {
        StartCoroutine(coTeleport());
    }

    IEnumerator coTeleport()        // 순간이동 구현
    {
        WaitForSeconds cooltime = new WaitForSeconds(teleport_cooltime);

        rb.AddForce(new Vector2(h_value, v_value).normalized * i_teleportPower, ForceMode2D.Impulse);

        // 쿨타임
        isTeleporting = true;
        yield return cooltime;
        isTeleporting = false;
    }

    private void SetCharacterSpriteAnimByMoveDirection()
    {
        // 이동 방향에 따른 캐릭터 애니메이션 설정
    }
}
