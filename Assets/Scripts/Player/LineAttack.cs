using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttack : MonoBehaviour
{
    public int lineAttackDamage;



    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))// && isCanHitLineAttack)
        {
            collision.GetComponent<BossAI>().Hurt(lineAttackDamage);
        }

        if (collision.CompareTag("Henchman"))// && isCanHitLineAttack)
        {
            collision.GetComponent<HenchmanAI>().Hurt(lineAttackDamage);
        }

        if (collision.CompareTag("Henchman_Quest3"))
        {
            collision.GetComponent<HenchmanAI_Quest3>().Hurt(lineAttackDamage);
        }
    }


}
