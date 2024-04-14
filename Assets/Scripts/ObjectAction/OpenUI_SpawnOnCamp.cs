using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI_SpawnOnCamp : MonoBehaviour
{
    public GameObject spawn_camp_UI;
    public PlayerInteract _playerInteract;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            spawn_camp_UI.SetActive(true);
            _playerInteract.isPlayerInteracting = true;
        }
    }
}
