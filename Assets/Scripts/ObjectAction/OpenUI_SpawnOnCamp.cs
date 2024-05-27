using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI_SpawnOnCamp : MonoBehaviour
{
    GameManager _gameManager;
    public GameObject spawn_camp_UI;
    private PlayerInteract _playerInteract;

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        _playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            spawn_camp_UI.SetActive(true);
            _playerInteract.isPlayerInteracting = true;
            _gameManager.isUIInteract = true;
        }
    }
}
