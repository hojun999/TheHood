using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttack : MonoBehaviour
{
    public int lineAttackDamage;

    public int damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBoss"))
        {
            collision.gameObject.GetComponent<EnemyBoss>().OnHit(damage);

            Destroy(gameObject);

        }

        if (collision.CompareTag("EnemyNormal"))
        {
            collision.gameObject.GetComponent<EnemyNormal>().OnHit(damage);

            Destroy(gameObject);
        }
    }


}
