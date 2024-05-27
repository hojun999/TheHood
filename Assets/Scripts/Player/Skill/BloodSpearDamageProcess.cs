using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpearDamageProcess : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBoss"))
        {
            collision.gameObject.GetComponent<EnemyBoss>().OnHit(damage);
        }

        if (collision.CompareTag("EnemyNormal"))
        {
            collision.gameObject.GetComponent<EnemyNormal>().OnHit(damage);
        }

    }
}
