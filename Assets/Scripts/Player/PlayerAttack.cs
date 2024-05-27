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
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();     // quest4에서 전투 시작 시 카메라 시점 변경은 GameManager에서 관리
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
            float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;      // 1. 마우스 위치값 각도(라디안) 변환
                                                                                        // 2. 라디안 > 도 변환
                                                                                        // 3. rotation 포지션 값으로 저장
            magicSquareHolder.rotation = Quaternion.Euler(0, 0, rotationZ);             // 4. 마우스 위치값에 따른 마법진 회전값 적용
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
