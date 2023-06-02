using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHp;
    public int maxMp;
    [HideInInspector]public int curHp;
    [HideInInspector]public int curMp;
    public float moveSpeed;
    public float dodgeCooltime;        // dodge 쿨타임

    [Header("Manager")]
    public GameManager gameManager;
    public InventoryManager inventoryManager;
    public QuestManager questManager;

    [Header("Attack")]
    public GameObject shootObject;

    [Header("CanGetItemList")]
    public Item[] fieldItems;

    [Header("BeforeGetItemTextUI")]
    public GameObject BeforeGetText_QuestArea;
    public GameObject BeforeGetText_Clothes_With_Blood;
    public GameObject BeforeGetText_Injector;
    public GameObject BeforeGetText_RustyGun;

    [Header("AfterGetItemTextUI")]
    public GameObject AfterGetText_QuestArea;
    public GameObject AfterGetText_Clothes_With_Blood;
    public GameObject AfterGetText_Injector;
    public GameObject AfterGetText_RustyGun;

    [Header("AfterGetItemText_useAlpha")]
    public GameObject AfterGetItemText_Alpha_Clothes_With_Blood;
    public GameObject AfterGetItemText_Alpha_Injector;
    public GameObject AfterGetItemText_Alpha_RustyGun;


    [Header("QuestUI")]
    public GameObject questionMark;
    public GameObject exMark;
    public GameObject QuestClearText;

    public Slider hpBar;
    public Slider mpBar;


    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    private float h, v;
    private float timer;

    private float hpSliderValue;
    private float mpSliderValue;

    [HideInInspector] public bool isPlayerInWoods;
   
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

        curHp = maxHp;
        curMp = maxMp;

        hpBar.value = 1;
        hpBar.value = 1;

    }

    private void Update()
    {
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        // 애니메이션
        #region
        if (anim.GetInteger("hAxisRaw") != h && Time.timeScale != 0)        // timescale은 ui 조작중일 때 플레이어 애니메이션 출력하지 않기 위함
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else if (anim.GetInteger("vAxisRaw") != v && Time.timeScale != 0)
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
        if (Input.GetKeyDown(KeyCode.Space) && !gameManager.isAction && isReadyDash) //&& !scanObject)
            isDashButtonDown = true;


        // 아이템을 인식하고 E를 눌러 획득하면, 필드 아이템을 인벤토리 아이템으로 전환
        if (Input.GetKeyDown(KeyCode.E) && scanObject.CompareTag("Item"))
        {
            if (scanObject.name == "Clothes_With_Blood")
            {
                GetItem(0);
                questManager.getItemNum_Quest2--;
                BeforeGetText_Clothes_With_Blood.SetActive(false);
                AfterGetText_Clothes_With_Blood.SetActive(true);
                AfterGetItemText_Alpha_Clothes_With_Blood.SetActive(true);
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
                questManager.getItemNum_Quest2--;
                BeforeGetText_Injector.SetActive(false);
                AfterGetText_Injector.SetActive(true);
                AfterGetItemText_Alpha_Injector.SetActive(true);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "RustyGun")
            {
                GetItem(3);
                questManager.getItemNum_Quest2--;
                BeforeGetText_RustyGun.SetActive(false);
                AfterGetText_RustyGun.SetActive(true);
                AfterGetItemText_Alpha_RustyGun.SetActive(true);
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

        // hp, mp 컨트롤
        if (hpBar != null)
            hpBar.value = Utils.Percent(curHp, maxHp);
        if (mpBar != null)
            mpBar.value = Utils.Percent(curMp, curMp);
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
        // 플레이어 피격
        if (collision.CompareTag("EnemyAtk"))
        {
            //Hurt(collision.GetComponentInParent<Enemy>().damage);
        }

        // 1번 퀘스트 
        if (collision.gameObject.CompareTag("QuestArea"))
        {
            questManager.GetComponent<QuestManager>().NextQuest();
            questManager.required_Area_Quest1.SetActive(false);
            BeforeGetText_QuestArea.SetActive(false);
            AfterGetText_QuestArea.SetActive(true);
            questionMark.SetActive(false);
            exMark.SetActive(true);
            setActiveQuestClearText();
        }
    }

    public void setActiveQuestClearText()
    {
        //  캔버스 안에 UI text 생성
        Instantiate(QuestClearText, QuestClearText.transform.position, Quaternion.identity, GameObject.Find("UI").transform);
    }

    public void Hurt(int damage)
    {
        if (!isHurt)
        {
            isHurt = true;
            curHp -= damage;
            //HandleHp();
            if (curHp <= 0)
            {
                //dead
            }
            else
            {
                Debug.Log("hp : " + curHp);
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
    }

    public void GetItem(int id)
    {
        inventoryManager.AddItem(fieldItems[id]);
    }

    IEnumerator HurtRoutine()   // 무적 상태 조정
    {
        yield return new WaitForSeconds(2f);
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


