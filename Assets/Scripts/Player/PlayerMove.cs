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
    [HideInInspector] public Vector2 moveDirection;     // playerinteract���� lay�� ������ �����ϱ� ���� ����

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

    private void GetMoveDirection()     // �Էµ� ���� ���� �̵� ���Ⱚ ����
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
            !isTeleporting &&                           // ��Ÿ�� �� �ƴϰ�
            isWalk == true &&                           // �ȴ� ���̰�
            !_playerInteract.isPlayerInteracting &&     // ��ȣ�ۿ� ������ �ʰ�
            _playerInteract.scannedObject == null)      // ��ȣ�ۿ� ������Ʈ �ν� ���� ���� ��
        {
            Teleport();
        }
    }

    private void Teleport()     // �����̵� ����
    {
        StartCoroutine(coTeleport());
    }

    IEnumerator coTeleport()        // �����̵� ����
    {
        Debug.Log("�ڷ���Ʈ");
        isTeleporting = true;

        rb.AddForce(moveVelocity * teleportPower, ForceMode2D.Impulse);

        // ��Ÿ��
        yield return new WaitForSeconds(teleportCooltime);
        isTeleporting = false;
    }

}
