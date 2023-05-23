using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkill : MonoBehaviour
{
    Camera MainCamera;

    [Header("LineAttackObject")]
    public GameObject LineAttackObject;

    [Header("ExplosionObject")]
    public GameObject explosionAttackObj;
    public GameObject explosionOutline;

    Vector2 firstMousePosOfLineAttack, secondMousePosOfLineAttack, mousePosOfExplosion;
    public int mouseClickScoreOfLineAttack;
    public int mouseclickScoreOfExplosion;

    public float axis;

    public bool isLineAttackUsing;    // 스킬 시전이 가능한 시간 동안 true
    public bool isExplosionUsing;
    public bool isExistLineObject;      // 스킬이 사용중인지
    public bool isExistExplosionObject;


    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        // LineAttack
        #region
        if (Input.GetKeyDown(KeyCode.Q) && !isLineAttackUsing)      // 코루틴을 시작하여 스킬 사용을 위한 마우스 클릭 두 번을 입력 받을 상태를 만듬
        {
            StartCoroutine(usingTimeOfLineAttack(2.5f));
        }

        if (Input.GetMouseButtonDown(0) && mouseClickScoreOfLineAttack == 0 && isLineAttackUsing)
        {
            firstMousePosOfLineAttack = Input.mousePosition;
            firstMousePosOfLineAttack = MainCamera.ScreenToWorldPoint(firstMousePosOfLineAttack);
            mouseClickScoreOfLineAttack++;
        }
        else if (Input.GetMouseButtonDown(0) && mouseClickScoreOfLineAttack == 1 && isLineAttackUsing)
        {
            secondMousePosOfLineAttack = Input.mousePosition;
            secondMousePosOfLineAttack = MainCamera.ScreenToWorldPoint(secondMousePosOfLineAttack);
            mouseClickScoreOfLineAttack++;
        }


        if (mouseClickScoreOfLineAttack == 2 && !isExistLineObject)    // 마우스 클릭 두 번째일 때 공격obj 생성
            StartCoroutine(createAndDestroyLineAttackObj());        // ★update문에서 함수 한 번만 발동시키고 싶을 때 메소드보다 bool값 이용하여 코루틴으로 작성
        #endregion

        // Explosion
        #region
        if (Input.GetKeyDown(KeyCode.Z) && !isExplosionUsing)
        {
            StartCoroutine(usingTimeOfExplosion(2.5f));
        }

        if (Input.GetMouseButtonDown(0) && mouseclickScoreOfExplosion == 0 && isExplosionUsing)
        {
            mousePosOfExplosion = Input.mousePosition;
            mousePosOfExplosion = MainCamera.ScreenToWorldPoint(mousePosOfExplosion);
            mouseclickScoreOfExplosion++;
        }

        if (mouseclickScoreOfExplosion == 1 && !isExistExplosionObject)
            StartCoroutine(createAndDestroyExplosionObj());
        #endregion
    }

    // LineAttack Coroutine
    #region
    IEnumerator usingTimeOfLineAttack(float endTime)     // 스킬 사용 후 범위 지정 가능 시간(공격 가능 시간)
    {
        isLineAttackUsing = true;
        float startTime = 0f;

        while (true)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
                break;
            else if (isExistLineObject)     // 스킬을 사용했다면 즉시 사용시간 끝내기(사용하고 사용 시간 안에 마우스 클릭 두 번을 더 받을 시, 스킬 다시 사용하는 것 방지)
                startTime = endTime;
            yield return null;
        }
        mouseClickScoreOfLineAttack = 0;
        isLineAttackUsing = false;
    }

    IEnumerator createAndDestroyLineAttackObj()
    {
        mouseClickScoreOfLineAttack = 0;            // 스킬 오브젝트가 중복 생성되지 않기 위함
        isExistLineObject = true;       // 없으면 스킬 시전 때 오브젝트 무수히 많이 생성됨

        Vector2 attackDir = secondMousePosOfLineAttack - firstMousePosOfLineAttack;
        Vector3 quaternionToTarget = Quaternion.Euler(0, 0, 90) * attackDir;        // 스킬 오브젝트를 secondMousePos 방향쪽으로 조정하는 코드

        var attackObjectCopy = Instantiate(LineAttackObject, firstMousePosOfLineAttack, Quaternion.LookRotation(forward: Vector3.forward, upwards: quaternionToTarget));
        yield return new WaitForSeconds(1.2f);        // WaitForSeconds(a) >> a시간은 스킬 오브젝트 생성 시간동안만 할당

        Destroy(attackObjectCopy);
        isExistLineObject = false;
    }
    #endregion

    // Explosion Coroutine
    #region
    IEnumerator usingTimeOfExplosion(float endTime)
    {
        isExplosionUsing = true;
        float startTime = 0f;

        while (true)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
                break;
            else if (isExistExplosionObject)       // 스킬 사용시 즉시 쿨타임 돌리기
                startTime = endTime;
            yield return null;
        }
        mouseclickScoreOfExplosion = 0;
        isExplosionUsing = false;
    }

    IEnumerator createAndDestroyExplosionObj()
    {
        isExistExplosionObject = true;

        var attackObjectCopy = Instantiate(explosionAttackObj, mousePosOfExplosion, Quaternion.LookRotation(forward: Vector3.forward));
        var outlineObjectCopy = Instantiate(explosionOutline, mousePosOfExplosion, Quaternion.LookRotation(forward: Vector3.forward));

        yield return new WaitForSeconds(1.2f);

        Destroy(attackObjectCopy);
        Destroy(outlineObjectCopy);
        isExistExplosionObject = false;
    }
    #endregion




}
