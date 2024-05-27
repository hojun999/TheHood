using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageProcess : MonoBehaviour
{
    public int explosionDamage;
    public float explosionPower;

    Vector2 enemyPos;
    Vector2 explosionAttackObjectPos;
    Vector2 dirBetweenThisAndEnemy;

    private void Start()
    {
        explosionAttackObjectPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyBoss") || collision.gameObject.CompareTag("EnemyNormal"))
        {
            enemyPos = collision.gameObject.transform.position;
            dirBetweenThisAndEnemy = enemyPos - explosionAttackObjectPos;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dirBetweenThisAndEnemy.normalized * explosionPower, ForceMode2D.Force);
        }

        if (collision.gameObject.CompareTag("EnemyBoss"))
        {
            collision.gameObject.GetComponent<EnemyBoss>().OnHit(explosionDamage);
        }

        if (collision.gameObject.CompareTag("EnemyNormal"))
        {
            collision.gameObject.GetComponent<EnemyNormal>().OnHit(explosionDamage);
        }
    }
}
