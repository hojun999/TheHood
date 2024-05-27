using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    // 변수
    #region
    [HideInInspector] public QuestManager questManager;

    public GameObject bulletPrefab;
    public AudioClip shotSFX;

    // 스탯
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public float shootTime = 1.2f;
    private bool isDie;
    private bool isShot;

    // 공격 관련 변수
    private Transform playerTransform;
    private Transform leftShotTransform;
    private Transform rightShotTransform;
    private float distanceOfPlayerAndEnemy;
    [HideInInspector] public bool startFight;
    private Vector2 shootPosition;

    // 움직임 관련 변수
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
        // 플레이어 위치
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

    public void Shoot()     // 캐릭터 x 좌표 값의 +- 1 사이 임의의 값으로 총알 발사
    {
        float shootAngle = Random.Range(playerTransform.position.x - 1f, playerTransform.position.x + 1f);
        Vector2 shootAngleVector = new Vector2(shootAngle, playerTransform.position.y);
        Vector2 bulletDirection = (shootAngleVector - shootPosition).normalized;
        SoundManager.instance.SFXPlayer("EnemyShot", shotSFX);
        Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(bulletDirection));
    }

    // 플레이어 위치에 따른 애니메이션 실행
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

    // 공격 방향 지정
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
    }   // 피격 처리

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

        RefreshQuestConditionByProgress();      // 퀘스트 조건 달성

        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        anim.SetBool("isDie", true);
        moveSpeed = 0;
        StopCoroutine(coAlphaBlink);
        sr.color = new Color(1, 1, 1, 1);

        CancelInvoke("Attack");
        CancelInvoke("ActionSequence");
    }       // 사망 처리

    // 퀘스트 조건, UI 등 최신화
    public abstract void RefreshQuestConditionByProgress();
}
