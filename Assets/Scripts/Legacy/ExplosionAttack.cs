using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
    public int explosionDamage;
    public float explosionPower;

    //public bool isCanHitExplosion;

    Vector2 enemyPos;
    Vector2 explosionAttackObjPos;
    Vector2 dirBetweenThisAndEnemy;
    void Start()
    {
        explosionAttackObjPos = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 剐摹扁 贸府
        if (collision.gameObject.CompareTag("Boss")|| collision.gameObject.CompareTag("Henchman") || collision.gameObject.CompareTag("Henchman_Quest3"))
        {
            enemyPos = collision.gameObject.transform.position;
            dirBetweenThisAndEnemy = enemyPos - explosionAttackObjPos;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dirBetweenThisAndEnemy.normalized * explosionPower, ForceMode2D.Force);
        }

        // 单固瘤 贸府
        if (collision.CompareTag("Boss")) //&& isCanHitExplosion)
            collision.GetComponent<BossAI>().Hurt(explosionDamage);

        if (collision.CompareTag("Henchman")) //&& isCanHitExplosion)
            collision.GetComponent<HenchmanAI>().Hurt(explosionDamage);

        if (collision.CompareTag("Henchman_Quest3"))
            collision.GetComponent<HenchmanAI_Quest3>().Hurt(explosionDamage);
    }



}
