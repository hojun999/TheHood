using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
    public int explosionDamage;
    public float explosionPower;

    public bool isCanHitExplosion;

    Vector2 enemyPos;
    Vector2 explosionAttackObjPos;
    Vector2 dirBetweenThisAndEnemy;


    void Start()
    {
        explosionAttackObjPos = transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Boss") && isCanHitExplosion || collision.gameObject.CompareTag("Henchman") && isCanHitExplosion)
        {
            enemyPos = collision.gameObject.transform.position;
            dirBetweenThisAndEnemy = enemyPos - explosionAttackObjPos;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dirBetweenThisAndEnemy.normalized * explosionPower, ForceMode2D.Force);
        }

        if (collision.CompareTag("Boss") && isCanHitExplosion)
        {
            collision.GetComponent<BossAI>().Hurt(explosionDamage);
            isCanHitExplosion = false;
        }

        if (collision.CompareTag("Henchman") && isCanHitExplosion)
        {
            collision.GetComponent<HenchmanAI>().Hurt(explosionDamage);
            isCanHitExplosion = false;
        }
    }


    
}
