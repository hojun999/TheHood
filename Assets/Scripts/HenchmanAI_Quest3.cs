using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HenchmanAI_Quest3 : MonoBehaviour
{
    public GameManager gameManager;
    public QuestManager_Lagacy questManager_lagacy;
    public AudioClip clip;

    [Header("Stats")]
    public int maxHp;
    public int curHp;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    public GameObject EnemyBullet;

    public Text text_Quest3;
    public Text text_Quest4;

    public Transform playerPos;
    public Transform LeftShootPos;
    public Transform RightShootPos;

    private Vector3 ShootPos;

    public float moveSpeed;

    private float xMove;
    private float yMove;
    private float attackAngle;
    private float DistOfXPositionPlayerAndEnemy;


    private float ActionTime;
    private float MoveTime;
    private float AttackTime = 1.2f;

    public bool isStartFight;     // �÷��̾ �ο� ������ �� �����ϸ� true, ����Ʈ 3Ŭ����� false, �÷��̾� ��� �� false, ����Ʈ 4 Ŭ���� �� false
    private bool doAttack;
    private bool isDie;

    private Vector2 movePos;
    public Vector3 attackAngleVector;

    Color halfA = new Color(1, 1, 1, 0);
    Color fullA = new Color(1, 1, 1, 1);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Action();
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        curHp = maxHp;
    }

    private void FixedUpdate()
    {
        rb.velocity = movePos;
        movePos = new Vector2(xMove, yMove) * moveSpeed;
    }

    private void Update()
    {
        if (isDie)
        {
            anim.SetBool("isDie", true);
            moveSpeed = 0;
            CancelInvoke("Attack");
            CancelInvoke("Action");
            doAttack = false;
            if (questManager_lagacy.eliminateHenchmanNum_Quest3 < 5)
                text_Quest3.text = "���� ������ִ� ���� " + questManager_lagacy.eliminateHenchmanNum_Quest3 + " / 5";

            if (questManager_lagacy.eliminateHenchmanNum_Quest4 < 6)
                text_Quest4.text = "���� " + questManager_lagacy.eliminateHenchmanNum_Quest4 + " / 6";
        }

        //if (gameManager.isEnterFight)
        //    isStartFight = true;
        //else
        //    isStartFight = false;

        DistOfXPositionPlayerAndEnemy = playerPos.position.x - gameObject.gameObject.transform.position.x;


        // �ִϸ��̼�        // enemyDir > 0 �����ʺ��� enemyDir < 0 ���ʺ���
        if (isStartFight && !isDie)
            anim.SetBool("isEnterFight", true);

        if (anim.GetFloat("enemyDir") != DistOfXPositionPlayerAndEnemy && !isDie)
            anim.SetFloat("enemyDir", DistOfXPositionPlayerAndEnemy);


        if (doAttack && !isDie)
            anim.SetBool("doAttack", true);
        else
            anim.SetBool("doAttack", false);


        // �ѱ� ��ġ ����
        if (DistOfXPositionPlayerAndEnemy > 0)
            ShootPos = LeftShootPos.position;
        else if (DistOfXPositionPlayerAndEnemy < 0)
            ShootPos = RightShootPos.position;
    }

    public void RandomMove()
    {
        xMove = Random.Range(-0.8f, 0.8f);
        yMove = Random.Range(-0.8f, 0.8f);
    }

    public void Attack()
    {
        doAttack = true;
    }

    public void ShootBullet()    // ĳ���� x���� +-1 ���� ������ ���� �Ѿ� �߻�
    {
        attackAngle = Random.Range(playerPos.position.x - 1f, playerPos.position.x + 1f);
        attackAngleVector = new Vector3(attackAngle, playerPos.position.y, 0);
        Vector3 dir = (attackAngleVector - ShootPos).normalized;
        SoundManager.instance.SFXPlayer("EnemyShoot", clip);
        Instantiate(EnemyBullet, ShootPos, Quaternion.LookRotation(dir));
    }

    void Action()
    {
        doAttack = false;
        MoveTime = Random.Range(0.8f, 1.5f);
        ActionTime = MoveTime + AttackTime + 0.12f;

        RandomMove();

        Invoke("Attack", 1f);
        Invoke("Action", ActionTime);
    }

    public void Hurt(int damage)
    {
        curHp -= damage;
        StartCoroutine(alphablink());

        if (curHp <= 0)
        {
            isDie = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            if (questManager_lagacy.questId == 30)
                questManager_lagacy.eliminateHenchmanNum_Quest3++;
            else if (questManager_lagacy.questId == 40)
                questManager_lagacy.eliminateHenchmanNum_Quest4++;
        }
    }


    IEnumerator alphablink()    // �ǰ� �� �����̴� ȿ��
    {
        sr.color = halfA;
        yield return new WaitForSeconds(0.05f);
        sr.color = fullA;
        yield return null;
    }
}
