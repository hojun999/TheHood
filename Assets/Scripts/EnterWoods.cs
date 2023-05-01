using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWoods : MonoBehaviour
{
    public GameObject EnterUIPannel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            EnterUIPannel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
