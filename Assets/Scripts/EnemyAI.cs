using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
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
    private float AttackTime;
    private float rayLength = 1f;

    // 이후에 디폴트로 바꾸고 싸움 지역에 입장하면 true 호출하게 변경
    private bool isStartFight = true;      // 플레이어가 싸움 지역을 에 입장하면 true, 퀘스트 3클리어시 false, 플레이어 사망 시 false, 퀘스트 4 클리어 시 false

    private bool doAttack;

    private Vector2 movePos;
    private Vector3 attackAngleVector;

    private void Awake()
    {
        if (gameObject.CompareTag("Henchman"))
            AttackTime = 1.5f;
        else
            AttackTime = 1.2f;


        Action();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        rb.velocity = movePos;
        movePos = new Vector2(xMove, yMove) * moveSpeed;

        //RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayLength, LayerMask.GetMask("Wall"));
        //RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("Wall"));
        //RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, rayLength, LayerMask.GetMask("Wall"));
        //RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Wall"));

        //if (hitRight.collider || hitLeft.collider)
        //{
        //    xMove = xMove * -1f;
        //    Debug.Log(hitRight.collider.name);
        //    Debug.Log(hitLeft.collider.name);
        //}
        //else if (hitUp.collider || hitDown.collider)
        //{
        //    yMove = yMove * -1f;
        //    Debug.Log(hitUp.collider.name);
        //    Debug.Log(hitDown.collider.name);
        //}


    }

    private void Update()
    {
        // 애니메이션        // enemyDir > 0 오른쪽보기 enemyDir < 0 왼쪽보기
        if (anim.GetFloat("enemyDir") != DistOfXPositionPlayerAndEnemy)
            anim.SetFloat("enemyDir", DistOfXPositionPlayerAndEnemy);
        else if (anim.GetFloat("yMove") != yMove && gameObject.CompareTag("Boss"))
            anim.SetFloat("yMove", yMove);
        else if (doAttack)
            anim.SetBool("doAttack", true);
        else if (!doAttack)
            anim.SetBool("doAttack", false);

        // 총구 위치 결정
        if (xMove <= 0)
            ShootPos = LeftShootPos.position;
        else if (xMove > 0)
            ShootPos = RightShootPos.position;

        DistOfXPositionPlayerAndEnemy = playerPos.position.x - gameObject.GetComponent<Transform>().position.x;



    }

    public void RandomMove()        // 텐트 오브젝트에 가까이 갈 경우 무조건 그 반대 방향으로 이동하게 구현
    {

        xMove = Random.Range(-1f, 1f);
        yMove = Random.Range(-1f, 1f);
        // x값에 따른 방향 애니메이션 추가
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

        if (gameObject.CompareTag("Henchman"))
            MoveTime = Random.Range(1f, 1.8f);
        else
            MoveTime = Random.Range(0.8f, 1.5f);

        if (gameObject.CompareTag("Henchman"))
            ActionTime = MoveTime + AttackTime + 0.12f;
        else
            ActionTime = MoveTime + AttackTime + 0.15f;
        RandomMove();
        Invoke("Attack", MoveTime);
        Invoke("Action", ActionTime); 
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Wall") && xMove < 0 || xMove > 0)
    //        xMove = xMove * -1;
    //    else if (collision.gameObject.CompareTag("Wall") && yMove > 0 || yMove < 0)
    //        yMove = yMove * -1;
    //}


}
