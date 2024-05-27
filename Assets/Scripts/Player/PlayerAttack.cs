using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    GameManager _gameManager;
    PlayerStat _playerStat;

    public GameObject skillManager;
    BloodSpear bloodSpear;
    Explosion explosion;


    [HideInInspector] public Camera mainCamera;

    public GameObject bullet;

    private Vector3 mousePos;

    //public PlayerSkill;

    public Transform magicSquareHolder;
    public Transform magicSquare;
    public AudioClip attackSFX;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool isPlayerInWoods;
    public float timeBetweenAttack;

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        _playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        bloodSpear = skillManager.GetComponent<BloodSpear>();
        explosion = skillManager.GetComponent<Explosion>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();     // quest4���� ���� ���� �� ī�޶� ���� ������ GameManager���� ����
        canAttack = true;
    }

    private void Update()
    {
        SetAttackDirection();
        HandleAttack();
        HandleUsingSkill();
    }


    void SetAttackDirection()
    {
        if (!_gameManager.isUIInteract)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - magicSquareHolder.position;
            float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;      // 1. ���콺 ��ġ�� ����(����) ��ȯ
                                                                                        // 2. ���� > �� ��ȯ
                                                                                        // 3. rotation ������ ������ ����
            magicSquareHolder.rotation = Quaternion.Euler(0, 0, rotationZ);             // 4. ���콺 ��ġ���� ���� ������ ȸ���� ����
        }
    }

    void CooltimeOnAttack()
    {
            StartCoroutine(Cooltime());
    }

    IEnumerator Cooltime()
    {
        yield return new WaitForSeconds(timeBetweenAttack);
        canAttack = true;
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && !_gameManager.isUIInteract && isPlayerInWoods)
        {
            canAttack = false;
            SoundManager.instance.SFXPlayer("PlayerShot", attackSFX);
            Vector3 dir = (Input.mousePosition - gameObject.transform.position).normalized;
            Instantiate(bullet, magicSquare.position, Quaternion.LookRotation(dir));

            CooltimeOnAttack();
        }
    }

    void HandleUsingSkill()
    {
        if (isPlayerInWoods)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                if (_playerStat.curMp >= skillManager.GetComponent<BloodSpear>().requiredMp && bloodSpear.canUse)
                    bloodSpear.Use();
            }
            else if (Input.GetMouseButtonDown(2))
            {
                if (_playerStat.curMp >= explosion.requiredMp && explosion.canUse)
                    explosion.Use();
            }

        }
    }
}
