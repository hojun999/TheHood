using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWoods : MonoBehaviour
{
    public GameObject EnterUIPanel;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            EnterUIPanel.SetActive(true);
        }
    }
}
