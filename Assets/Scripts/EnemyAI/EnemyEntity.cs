using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    private GameManager gameManager;
    [HideInInspector] public QuestManager questManager;

    public GameObject bulletPrefab;
    public AudioClip shootSFX;

    // 스탯
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public float shootTime = 1.2f;
    private bool isDie;
    private bool isShoot;
    private bool startFight;

    // 공격 관련 변수
    private Transform playerTransform;
    private Transform leftShootTransform;
    private Transform rightShootTransform;
    private float distanceOfPlayerAndEnemy;
    private Vector2 shootPosition;

    // 움직임 관련 변수
    private Vector2 movePosition;
    [SerializeField] public float xMove;
    [SerializeField] public float yMove;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Coroutine coAlphaBlink;

    private void Start()
    {
        Set();
    }

    private void Update()
    {
        if (isDie)
            Died();

        distanceOfPlayerAndEnemy = playerTransform.position.x - gameObject.transform.position.x;
        SetShootDistance();
        SetAnimationState();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Set()
    {
        // 변수
        gameManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        questManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<QuestManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // 플레이어 위치
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rightShootTransform = gameObject.transform.GetChild(0);
        leftShootTransform = gameObject.transform.GetChild(1);

        curHp = maxHp;

        ActionSequence();
    }

    void ActionSequence()
    {
        isShoot = false;
        float moveTime = Random.Range(0.8f, 1.5f);
        float actionTime = moveTime + shootTime + 0.12f;

        MoveRandomDirection();

        Invoke("Attack", moveTime);
        Invoke("Action", actionTime);
    }

    void Move()
    {
        rb.velocity = movePosition;
        movePosition = new Vector2(xMove, yMove) * moveSpeed;
    }

    public abstract void MoveRandomDirection();

    public void Shoot()     // 캐릭터 x 좌표 값의 +- 1 사이 임의의 값으로 총알 발사
    {
        float shootAngle = Random.Range(playerTransform.position.x - 1f, playerTransform.position.x + 1f);
        Vector2 shootAngleVector = new Vector2(shootAngle, playerTransform.position.y);
        Vector2 bulletDirection = (shootAngleVector - shootPosition).normalized;
        // 사운드 작성
        Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(bulletDirection));     // 오브젝트 풀링 활용
    }

    void SetAnimationState()
    {
        if (startFight && !isDie)
            anim.SetBool("isEnterFight", true);

        if (anim.GetFloat("enemyDir") != distanceOfPlayerAndEnemy && !isDie)
            anim.SetFloat("enemyDir", distanceOfPlayerAndEnemy);

        if (isShoot && !isDie)
            anim.SetBool("isShoot", true);
        else
            anim.SetBool("isShoot", false);
    }

    void SetShootDistance()
    {
        if (distanceOfPlayerAndEnemy <= 0)
            shootPosition = leftShootTransform.position;
        else
            shootPosition = rightShootTransform.position;
    }

    public void OnHit(int damage)
    {
        curHp -= damage;
        coAlphaBlink = StartCoroutine(alphaBlinkOnHit());

        if (curHp <= 0)
            Died();
    }

    IEnumerator alphaBlinkOnHit()
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
        isShoot = false;

        RefreshQuestConditionByProgress();      // 퀘스트 조건 달성

        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        anim.SetBool("isDie", true);
        moveSpeed = 0;
        StopCoroutine(coAlphaBlink);

        CancelInvoke("Attack");
        CancelInvoke("ActionSequence");

    }

    public abstract void RefreshQuestConditionByProgress();
}
