using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody rb;

    public Transform playerPos;
    public Transform LeftShootPos;
    public Transform RightShootPos;

    private int stateNum;

    public float moveSpeed;

    private float xMove;
    private float yMove;
    private float movingTime;
    private float movingStandardTime;
    private float attackAngle;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //1~3초마다 공격/움직임 state 결정, state 중에는 다른 행동 호출 안하며 해당 행동이 끝나면 바로 다른 행동 호출
    }

    //public void GetStateNum()
    //{
    //    stateNum = Random.Range(1, 3);      // 1 attack 2 move
    //}

    

    //public void GetState()
    //{
    //    switch (stateNum)
    //    {
    //        case 1:
    //            break;
    //    }
    //}

    public void RandomMove()        // 텐트 오브젝트에 가까이 갈 경우 무조건 그 반대 방향으로 이동하게 구현
    {
        xMove = Random.Range(-1, 1);
        yMove = Random.Range(-1, 1);
        movingTime = Random.Range(0.5f, 1.5f);      // 움직이는 시간
        movingStandardTime += Time.timeScale;

        while (movingTime >= movingStandardTime)
        {
            Vector2 movePos = new Vector2(xMove, yMove) * moveSpeed;
            rb.velocity = movePos;
        }
    }

    public void Attack()    // 캐릭터 x값의 +-1 사이 랜덤값 으로 총알 발사
    {
        attackAngle = Random.Range(playerPos.position.x - 1, playerPos.position.x + 1);
    }

}
