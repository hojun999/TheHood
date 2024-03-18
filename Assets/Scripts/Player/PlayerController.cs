using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public float moveSpeed;
    public float dodgeCooltime;        // dodge 쿨타임
    public float natureHealingCoolTime;
    float natureHealingTime;

    [Header("Manager")]
    public GameManager gameManager;
    public InventoryManager inventoryManager;
    public QuestManager_Lagacy questManager_lagacy;

    [Header("Attack")]
    public GameObject shootObject;

    [Header("CanGetItemList")]
    public Item[] fieldItems;

    [Header("BeforeGetItemTextUI")]
    public GameObject BeforeGetText_BigQuestArea;
    public GameObject BeforeGetText_SmallQuestArea;
    public GameObject BeforeGetText_Clothes_With_Blood;
    public GameObject BeforeGetText_Injector;
    public GameObject BeforeGetText_RustyGun;
    public GameObject BeforeText_eliminateBoss;
    public GameObject BeforeText_eliminateHenchman_Quest3;
    public GameObject BeforeText_eliminateHenchman_Quest4;

    [Header("AfterGetItemTextUI")]
    public GameObject AfterGetText_BigQuestArea;
    public GameObject AfterGetText_SmallQuestArea;
    public GameObject AfterGetText_Clothes_With_Blood;
    public GameObject AfterGetText_Injector;
    public GameObject AfterGetText_RustyGun;
    public GameObject AfterText_eliminateBoss;
    public GameObject AfterText_eliminateHenchman_Quest3;
    public GameObject AfterText_eliminateHenchman_Quest4;

    [Header("AfterGetItemText_useAlpha")]
    public GameObject AfterGetItemText_Alpha_Clothes_With_Blood;
    public GameObject AfterGetItemText_Alpha_Injector;
    public GameObject AfterGetItemText_Alpha_RustyGun;
    public GameObject AfterLocateAtBigArea_Alpha;
    public GameObject AfterLocateAtSmallArea_Alpha;

    [Header("QuestUI")]
    public GameObject questionMark;
    public GameObject exMark;
    public GameObject QuestClearText;
    public GameObject Direction_BigArea;
    public GameObject Direction_SmallArea;

    public Slider hpBar;
    public Slider mpBar;

    public GameObject diePanel;

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
    private bool isDie;
    [HideInInspector] public bool isCanUseLineAttack;
    [HideInInspector] public bool isCanUseExplosion;

    [HideInInspector]public Vector3 moveDir;
    GameObject scanObject;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        isReadyDash = true;     // 시작부터 대쉬 가능

        curHp = Math.Clamp(maxHp, 0, maxHp);
        curMp = Math.Clamp(maxMp, 0, maxMp);

        hpBar.value = 1;
        hpBar.value = 1;

    }

    private void Update()
    {
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        curHp = Math.Clamp(curHp, 0, maxHp);
        curMp = Math.Clamp(curMp, 0, maxMp);

        if (curMp < 8)
            isCanUseLineAttack = false;
        else
            isCanUseLineAttack = true;

        if (curMp < 15)
            isCanUseExplosion = false;
        else
            isCanUseExplosion = true;

        // 자동회복
        if (natureHealingTime < natureHealingCoolTime)
            natureHealingTime += Time.deltaTime;

        if (natureHealingTime > natureHealingCoolTime)
        {
            curHp += 2;
            curMp += 4;
            natureHealingTime = 0;
        }

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
                questManager_lagacy.getItemNum_Quest2--;
                BeforeGetText_Clothes_With_Blood.SetActive(false);
                AfterGetText_Clothes_With_Blood.SetActive(true);
                AfterGetItemText_Alpha_Clothes_With_Blood.SetActive(true);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "Injector")
            {
                GetItem(2);
                questManager_lagacy.getItemNum_Quest2--;
                BeforeGetText_Injector.SetActive(false);
                AfterGetText_Injector.SetActive(true);
                AfterGetItemText_Alpha_Injector.SetActive(true);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "RustyGun")
            {
                GetItem(3);
                questManager_lagacy.getItemNum_Quest2--;
                BeforeGetText_RustyGun.SetActive(false);
                AfterGetText_RustyGun.SetActive(true);
                AfterGetItemText_Alpha_RustyGun.SetActive(true);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "RedFlower")
            {
                GetItem(4);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "BlueFlower")
            {
                GetItem(5);
                scanObject.SetActive(false);
            }
            else if (scanObject.name == "Crystal")
            {
                GetItem(7);
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
            mpBar.value = Utils.Percent(curMp, maxMp);
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
        // 1번 퀘스트 
        if (collision.gameObject.CompareTag("BigQuestArea"))
        {
            questManager_lagacy.locateAtQuestAreaNum_Quest1++;
            collision.gameObject.SetActive(false);
            BeforeGetText_BigQuestArea.SetActive(false);
            AfterGetText_BigQuestArea.SetActive(true);
            AfterLocateAtBigArea_Alpha.SetActive(true);
            Direction_BigArea.SetActive(false);
        }

        if (collision.gameObject.CompareTag("SmallQuestArea"))
        {
            questManager_lagacy.locateAtQuestAreaNum_Quest1++;
            collision.gameObject.SetActive(false);
            BeforeGetText_SmallQuestArea.SetActive(false);
            AfterGetText_SmallQuestArea.SetActive(true);
            AfterLocateAtSmallArea_Alpha.SetActive(true);
            Direction_SmallArea.SetActive(false);
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

            if (curHp <= 0)
            {
                gameManager.isAction = true;
                GetComponent<BoxCollider2D>().enabled = false;
                anim.SetTrigger("isDie");
                Invoke("OpenDiePanel", 2f);

            }
            else
            {
                Debug.Log("hp : " + curHp);
                StartCoroutine(HurtRoutine());
                StartCoroutine(alphablink());
            }
        }
    }

    public void UseSkill(int Mp)
    {
        curMp -= Mp;
    }

    IEnumerator Dash()
    {
        float dodgePower = 70f;
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

    void OpenDiePanel()
    {
        diePanel.SetActive(true);
        Time.timeScale = 0;
    }
}


