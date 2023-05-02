using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float hp;
    public float energy;

    public GameManager gameManager;
    public InventoryManager inventoryManager;

    public GameObject shootObject;

    public Item[] fieldItems;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    private float h, v;

    public float moveSpeed;

    private float timer;
    [SerializeField]
    private float dodgeCooltime;        // dodge 쿨타임

    public bool isPlayerInWoods;
   
    private bool isWalk;
    private bool isHurt;
    private bool isDashButtonDown;
    private bool isReadyDash;

    [HideInInspector]public Vector3 moveDir;
    GameObject scanObject;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        isReadyDash = true;     // 시작부터 대쉬 가능

    }

    private void Update()
    {
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        // 애니메이션
        #region
        if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else if (anim.GetInteger("vAxisRaw") != v)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else
            anim.SetBool("isChange", false);
        #endregion


        // 플레이어 움직임 방향
        #region
        if (v == 1)
            moveDir = Vector2.up;
        else if (v == -1)
            moveDir = Vector2.down;
        else if (h == 1)
            moveDir = Vector2.right;
        else if (h == -1)
            moveDir = Vector2.left;
        #endregion

        // 오브젝트 스캔
        if (Input.GetButtonDown("Jump") && scanObject != null)
            gameManager.talkAction(scanObject);

        // Ray - Object Layer만 scanObject에 할당
        #region
        Debug.DrawRay(rb.position, moveDir * 0.6f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, moveDir, 1f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject;
        else
            scanObject = null;
        #endregion

        // dash
        if (Input.GetKeyDown(KeyCode.Space) && !gameManager.isAction && isReadyDash)
            isDashButtonDown = true;


        // 아이템을 인식하고 E를 눌러 획득하면, 필드 아이템을 인벤토리 아이템으로 전환
        if (Input.GetKeyDown(KeyCode.E) && scanObject.gameObject.CompareTag("Item"))
        {
            if (scanObject.name == "Clothes_With_Blood")
            {
                GetItem(0);
                scanObject.SetActive(false);
            }
            else if(scanObject.name == "HpPosion")
            {
                GetItem(1);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "Injector")
            {
                GetItem(2);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "MalfunctionedGun")
            {
                GetItem(3);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "SpeedUpPosion")
            {
                GetItem(4);
                scanObject.SetActive(false);
            }
        }

        if (gameManager.isAction || gameManager.activeInventory || !isPlayerInWoods)    // 인벤토리 활성화, 대화 중, 캠프에서 공격 불가능
            shootObject.SetActive(false);
        else
            shootObject.SetActive(true);

            
    }

    private void FixedUpdate()
    {
        //move
        #region

        if (h != 0 || v != 0)
            isWalk = true;
        else // 움직임 입력이 없을 때 rb.velocity 값이 없게 설정
        {
            isWalk = false;
            rb.velocity = new Vector2(0, 0);
        }

        if (isWalk)     // 움직임 입력이 있을 때만 이동
            rb.velocity = new Vector2(h, v).normalized * moveSpeed;
        #endregion

        //dash
        #region
        if (!isReadyDash && !gameManager.isAction)
        {
            timer += Time.deltaTime;
            if (timer > dodgeCooltime)
            {
                isReadyDash = true;
                timer = 0;
            }
        }

        if (isDashButtonDown && isReadyDash)
        {
            StartCoroutine(Dash());
            isDashButtonDown = false;
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAtk"))
        {
            Hurt(collision.GetComponentInParent<Enemy>().damage);
        }
    }

    public void Hurt(int damage)
    {
        if (!isHurt)
        {
            isHurt = true;
            hp = hp - damage;
            if (hp <= 0)
            {
                //dead
            }
            else
            {
                Debug.Log("hp : " + hp);
                StartCoroutine(HurtRoutine());
                StartCoroutine(alphablink());
            }
        }
    }

    IEnumerator Dash()
    {
        float dodgePower = 90f;
        rb.AddForce(new Vector2(h, v).normalized * dodgePower, ForceMode2D.Impulse);     // 움직임 값에 따른 addforce 방향 설정
        isReadyDash = false;
        yield return new WaitForSeconds(2f);        // 대쉬 쿨타임
        //isDashButtonDown = false;
    }

    public void GetItem(int id)
    {
        inventoryManager.AddItem(fieldItems[id]);
    }

    IEnumerator HurtRoutine()   // 무적 상태 조정
    {
        yield return new WaitForSeconds(2.5f);
        isHurt = false;
    }

    IEnumerator alphablink()    // 피격 시 깜빡이는 효과
    {
        while (isHurt)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = halfA;
            yield return new WaitForSeconds(0.1f);
            sr.color = fullA;
        }
    }
}


