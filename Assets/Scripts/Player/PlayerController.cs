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
    public float dodgeCooltime;        // dodge ��Ÿ��
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

        isReadyDash = true;     // ���ۺ��� �뽬 ����

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

        // �ڵ�ȸ��
        if (natureHealingTime < natureHealingCoolTime)
            natureHealingTime += Time.deltaTime;

        if (natureHealingTime > natureHealingCoolTime)
        {
            curHp += 2;
            curMp += 4;
            natureHealingTime = 0;
        }

        // �ִϸ��̼�
        #region
        if (anim.GetInteger("hAxisRaw") != h && Time.timeScale != 0)        // timescale�� ui �������� �� �÷��̾� �ִϸ��̼� ������� �ʱ� ����
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


        // �÷��̾� ������ ����
        #region
        #endregion

        // ������Ʈ ��ĵ
        if (Input.GetButtonDown("Jump") && scanObject != null)
            gameManager.talkAction(scanObject);

        // Ray - Object Layer�� scanObject�� �Ҵ�
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


        // �������� �ν��ϰ� E�� ���� ȹ���ϸ�, �ʵ� �������� �κ��丮 ���������� ��ȯ
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

        if (gameManager.isAction || gameManager.activeInventory || !isPlayerInWoods)    // �κ��丮 Ȱ��ȭ, ��ȭ ��, ķ������ ���� �Ұ���
            shootObject.SetActive(false);
        else
            shootObject.SetActive(true);

        // hp, mp ��Ʈ��
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
        else // ������ �Է��� ���� �� rb.velocity ���� ���� ����
        {
            isWalk = false;
            rb.velocity = new Vector2(0, 0);
        }

        if (isWalk)     // ������ �Է��� ���� ���� �̵�
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
        // 1�� ����Ʈ 
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
        //  ĵ���� �ȿ� UI text ����
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
        rb.AddForce(new Vector2(h, v).normalized * dodgePower, ForceMode2D.Impulse);     // ������ ���� ���� addforce ���� ����
        isReadyDash = false;
        yield return new WaitForSeconds(2f);        // �뽬 ��Ÿ��
    }

    public void GetItem(int id)
    {
        inventoryManager.AddItem(fieldItems[id]);
    }

    IEnumerator HurtRoutine()   // ���� ���� ����
    {
        yield return new WaitForSeconds(2f);
        isHurt = false;
    }

    IEnumerator alphablink()    // �ǰ� �� �����̴� ȿ��
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


