using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public GameObject EnemyBullet;

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
    private float rayLength = 1f;

    // 이후에 디폴트로 바꾸고 싸움 지역에 입장하면 true 호출하게 변경
    private bool isStartFight = true;      // 플레이어가 싸움 지역을 에 입장하면 true, 퀘스트 3클리어시 false, 플레이어 사망 시 false, 퀘스트 4 클리어 시 false

    private bool doAttack;

    private Vector2 movePos;
    private Vector3 attackAngleVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Action();
    }


    private void FixedUpdate()
    {
        rb.velocity = movePos;
        movePos = new Vector2(xMove, yMove) * moveSpeed;
    }

    private void Update()
    {
        // 애니메이션        // enemyDir > 0 오른쪽보기 enemyDir < 0 왼쪽보기
        if (anim.GetFloat("enemyDir") != DistOfXPositionPlayerAndEnemy)
            anim.SetFloat("enemyDir", DistOfXPositionPlayerAndEnemy);

        if (anim.GetFloat("yMove") != yMove)
            anim.SetFloat("yMove", yMove);

        if (doAttack)
            anim.SetBool("doAttack", true);
        else
            anim.SetBool("doAttack", false);

        DistOfXPositionPlayerAndEnemy = playerPos.position.x - gameObject.GetComponent<Transform>().position.x;

        // 총구 위치 결정
        if (DistOfXPositionPlayerAndEnemy <= 0)
            ShootPos = LeftShootPos.position;
        else if (DistOfXPositionPlayerAndEnemy > 0)
            ShootPos = RightShootPos.position;




    }

    public void RandomMove()
    {
        xMove = Random.Range(-1f, 1f);
        yMove = Random.Range(-1f, 1f);
    }

    public void Attack()
    {
        doAttack = true;
    }

    public void ShootBullet()    // 캐릭터 x값의 +-1 사이 랜덤값 으로 총알 발사
    {
        attackAngle = Random.Range(playerPos.position.x - 1f, playerPos.position.x + 1f);
        attackAngleVector = new Vector3(attackAngle, playerPos.position.y, 0);
        Vector3 dir = (attackAngleVector - ShootPos).normalized;
        Instantiate(EnemyBullet, ShootPos, Quaternion.LookRotation(dir));
    }

    void Action()
    {
        doAttack = false;
        MoveTime = Random.Range(0.8f, 1.5f);
        ActionTime = MoveTime + AttackTime + 0.12f;

        RandomMove();

        Invoke("Attack", MoveTime);
        Invoke("Action", ActionTime); 
    }



}
