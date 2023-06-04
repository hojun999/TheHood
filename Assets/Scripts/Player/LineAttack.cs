using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttack : MonoBehaviour
{
    public int lineAttackDamage;

    public bool isCanHitLineAttack;


    private void Awake()
    {
        //Invoke("EnableCollider2D", 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss") && isCanHitLineAttack)
        {
            collision.GetComponent<BossAI>().Hurt(lineAttackDamage);
            isCanHitLineAttack = false;
        }

        if (collision.CompareTag("Henchman") && isCanHitLineAttack)
        {
            collision.GetComponent<HenchmanAI>().Hurt(lineAttackDamage);
            isCanHitLineAttack = false;
        }
    }

    void EnableCollider2D()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

}
