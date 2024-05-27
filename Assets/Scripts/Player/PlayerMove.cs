using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerInteract _playerInteract;
    [HideInInspector] public Rigidbody2D rb;
    private Animator anim;

    [HideInInspector] public float moveX;
    [HideInInspector] public float moveY;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    [HideInInspector] public Vector2 moveDirection;     // playerinteract에서 lay의 방향을 결정하기 위한 변수

    [SerializeField] private float moveSpeed;
    [SerializeField] private float teleportPower;
    [SerializeField] private float teleportCooltime;


    private bool isWalk;
    private bool isTeleporting;


    void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerInteract = gameObject.GetComponent<PlayerInteract>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMoveSetting();
        SetCharacterSpriteAnimByMoveDirection();
    }

    private void HandleMoveSetting()
    {
            GetMoveDirection();
            SetMove();
            CheckUsingTelepote();
    }

    private void GetMoveDirection()     // 입력된 값에 따른 이동 방향값 세팅
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY);
        moveVelocity = moveInput.normalized * moveSpeed;

        if (!_playerInteract.isPlayerInteracting)
        {
            if (moveX == 1)
                moveDirection = Vector2.right;
            else if (moveX == -1)
                moveDirection = Vector2.left;
            else if (moveY == 1)
                moveDirection = Vector2.up;
            else if (moveY == -1)
                moveDirection = Vector2.down;
        }

        isWalk = moveVelocity != Vector2.zero;
    }

    private void SetMove()
    {
        if(!_playerInteract.isPlayerInteracting)
            rb.velocity = moveVelocity;
    }

    private void SetCharacterSpriteAnimByMoveDirection()
    {
        if (anim.GetInteger("hAxisRaw") != moveX && Time.timeScale != 0 && !_playerInteract.isPlayerInteracting)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)moveX);
        }
        else if (anim.GetInteger("vAxisRaw") != moveY && Time.timeScale != 0 && !_playerInteract.isPlayerInteracting)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)moveY);
        }
        else
            anim.SetBool("isChange", false);

    }

    private void CheckUsingTelepote()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            !isTeleporting &&                           // 쿨타임 중 아니고
            isWalk == true &&                           // 걷는 중이고
            !_playerInteract.isPlayerInteracting &&     // 상호작용 중이지 않고
            _playerInteract.scannedObject == null)      // 상호작용 오브젝트 인식 범위 밖일 때
        {
            Teleport();
        }
    }

    private void Teleport()     // 순간이동 실행
    {
        StartCoroutine(coTeleport());
    }

    IEnumerator coTeleport()        // 순간이동 구현
    {
        Debug.Log("텔레포트");
        isTeleporting = true;

        rb.AddForce(moveVelocity * teleportPower, ForceMode2D.Impulse);

        // 쿨타임
        yield return new WaitForSeconds(teleportCooltime);
        isTeleporting = false;
    }

}
