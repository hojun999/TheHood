using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI_SpawnOnWoods : MonoBehaviour
{
    public GameObject spawn_woods_UI;
    public PlayerInteract _playerInteract;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            spawn_woods_UI.SetActive(true);
            _playerInteract.isPlayerInteracting = true;
        }
    }
}
