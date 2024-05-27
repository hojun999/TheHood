using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    // ����
    #region
    [HideInInspector] public QuestManager questManager;

    public GameObject bulletPrefab;
    public AudioClip shotSFX;

    // ����
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public float shootTime = 1.2f;
    private bool isDie;
    private bool isShot;

    // ���� ���� ����
    private Transform playerTransform;
    private Transform leftShotTransform;
    private Transform rightShotTransform;
    private float distanceOfPlayerAndEnemy;
    [HideInInspector] public bool startFight;
    private Vector2 shootPosition;

    // ������ ���� ����
    private Vector2 movePosition;
    public float xMove;
    public float yMove;

    Rigidbody2D rb;
    [SerializeField] private Animator anim;
    SpriteRenderer sr;
    Coroutine coAlphaBlink;
    #endregion

    private void OnEnable()
    {
        SetOnEnabled();
    }

    private void Update()
    {
        SetOnUpdate();
        distanceOfPlayerAndEnemy = playerTransform.position.x - gameObject.transform.position.x;
        SetAnimationState();
        SetShotDistance();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void SetOnEnabled()
    {
        questManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<QuestManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        curHp = maxHp;

        ActionSequence();
    }

    void SetOnUpdate()
    {
        // �÷��̾� ��ġ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rightShotTransform = gameObject.transform.GetChild(0);
        leftShotTransform = gameObject.transform.GetChild(1);
    }

    void ActionSequence()
    {
        isShot = false;
        float moveTime = Random.Range(0.8f, 1.5f);
        float actionTime = moveTime + shootTime + 0.12f;

        MoveRandomDirection();

        Invoke("Attack", moveTime);
        Invoke("ActionSequence", actionTime);
    }

    void Move()
    {
        rb.velocity = movePosition;
        movePosition = new Vector2(xMove, yMove) * moveSpeed;
    }

    public abstract void MoveRandomDirection();

    public void Attack()
    {
        isShot = true;
    }

    public void Shoot()     // ĳ���� x ��ǥ ���� +- 1 ���� ������ ������ �Ѿ� �߻�
    {
        float shootAngle = Random.Range(playerTransform.position.x - 1f, playerTransform.position.x + 1f);
        Vector2 shootAngleVector = new Vector2(shootAngle, playerTransform.position.y);
        Vector2 bulletDirection = (shootAngleVector - shootPosition).normalized;
        SoundManager.instance.SFXPlayer("EnemyShot", shotSFX);
        Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(bulletDirection));
    }

    // �÷��̾� ��ġ�� ���� �ִϸ��̼� ����
    public void SetAnimationState()
    {
        if (startFight && !isDie)
            anim.SetBool("isEnterFight", true);

        if (anim.GetFloat("enemyDir") != distanceOfPlayerAndEnemy && !isDie)
            anim.SetFloat("enemyDir", distanceOfPlayerAndEnemy);

        if (isShot && !isDie)
        {
            anim.SetBool("isShoot", true);
        }
        else
        {
            anim.SetBool("isShoot", false);
        }
    }   

    // ���� ���� ����
    public void SetShotDistance()
    {
        if (distanceOfPlayerAndEnemy <= 0)
            shootPosition = leftShotTransform.position;
        else
            shootPosition = rightShotTransform.position;
    }

    public void OnHit(int damage)
    {
        curHp -= damage;
        coAlphaBlink = StartCoroutine(AlphaBlinkOnHit());

        if (curHp <= 0)
            Died();
    }   // �ǰ� ó��

    IEnumerator AlphaBlinkOnHit()
    {
        Color halfA = new Color(1, 1, 1, 0);
        Color fullA = new Color(1, 1, 1, 1);

        sr.color = halfA;
        yield return new WaitForSeconds(0.05f);
        sr.color = fullA;
        yield return null;
    }

    public void Died()
    {
        isDie = true;
        isShot = false;

        RefreshQuestConditionByProgress();      // ����Ʈ ���� �޼�

        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        anim.SetBool("isDie", true);
        moveSpeed = 0;
        StopCoroutine(coAlphaBlink);
        sr.color = new Color(1, 1, 1, 1);

        CancelInvoke("Attack");
        CancelInvoke("ActionSequence");
    }       // ��� ó��

    // ����Ʈ ����, UI �� �ֽ�ȭ
    public abstract void RefreshQuestConditionByProgress();
}
