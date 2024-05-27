using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log(gameObject.transform.parent.gameObject.GetComponent<EnemyNormal>().startFight);
            gameObject.transform.parent.gameObject.GetComponent<EnemyNormal>().enabled = true;
            gameObject.transform.parent.gameObject.GetComponent<EnemyNormal>().startFight = true;
            //Debug.Log(gameObject.transform.parent.gameObject.GetComponent<EnemyNormal>().startFight);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
