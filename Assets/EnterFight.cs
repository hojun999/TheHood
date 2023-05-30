using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFight : MonoBehaviour
{
    public Camera mainCamera;
    public Camera fightCamera;

    public GameObject EnterFightUIPanel;

    public GameObject Player;
    public Transform enterFightPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnterFightUIPanel.SetActive(true);
        }
    }

    public void ConverCameraNormalToFight()
    {
        mainCamera.enabled = false;
        fightCamera.enabled = true;
    }

    public void ConvertcameraFightToNormal()
    {
        mainCamera.enabled = true;
        fightCamera.enabled = false;

    }

    public void EnterFightSetting()
    {
        Player.transform.position = enterFightPos.position;
    }

}
