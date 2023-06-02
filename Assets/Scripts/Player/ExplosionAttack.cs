using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
    public float explosionPower;

    Vector2 enemyPos;
    Vector2 explosionAttackObjPos;

    Vector2 dirBetweenThisAndEnemy;

    void Start()
    {
        explosionAttackObjPos = transform.position;
        Debug.Log(explosionAttackObjPos);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Henchman"))
        {
            Debug.Log(collision.gameObject.name);
            enemyPos = collision.gameObject.transform.position;
            Debug.Log(enemyPos);
            dirBetweenThisAndEnemy = enemyPos - explosionAttackObjPos;
            Debug.Log(dirBetweenThisAndEnemy);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dirBetweenThisAndEnemy.normalized * explosionPower, ForceMode2D.Force);
        }


    }


    
}
