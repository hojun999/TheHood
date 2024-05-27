using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerSkill : MonoBehaviour
{
    [HideInInspector] public PlayerAttack _playerAttack;
    [HideInInspector] public PlayerStat _playerStat;
    [HideInInspector] public Camera mainCamera;

    public Image cooltimeImage;

    public int requiredMp;
    public float usingTime;
    public float cooltime;
    [HideInInspector] public bool canUse = true;
    [HideInInspector] public bool isProgress;    // ��ų�� ���� ������
    [HideInInspector] public bool isAttacking;   // ��ų ������ ���� ������

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        _playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        _playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
    }

    public void SetMainCameraOnUse()
    {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void ActionOnUse()
    {
        _playerAttack.canAttack = false;
        _playerStat.curMp -= requiredMp;
        SetMainCameraOnUse();
    }

    public abstract void Use();

}
