using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBloodLine : MonoBehaviour
{
    Camera MainCamera;

    public GameObject BloodLine;

    Vector2 firstMousePos, secondMousePos;
    public int mouseClickScore;

    public float axis;

    public bool isUsingTime;
    public bool isExistLineObject;


    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mouseClickScore = 0;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isUsingTime)      // 코루틴을 시작하여 스킬 사용을 위한 마우스 클릭 두 번을 입력 받을 상태를 만듬
        {
            StartCoroutine(usingTimeOfBloodLine(2.5f));
        }

        if (Input.GetMouseButtonDown(0) && mouseClickScore == 0 && isUsingTime)
        {
            firstMousePos = Input.mousePosition;
            firstMousePos = MainCamera.ScreenToWorldPoint(firstMousePos);
            mouseClickScore++;
        }
        else if (Input.GetMouseButtonDown(0) && mouseClickScore == 1 && isUsingTime)
        {
            secondMousePos = Input.mousePosition;
            secondMousePos = MainCamera.ScreenToWorldPoint(secondMousePos);
            mouseClickScore++;
        }


        if (mouseClickScore == 2 && !isExistLineObject)    // 마우스 클릭 두 번째일 때 공격obj 생성
            StartCoroutine(createAndDestroyAttackObj());        // ★update문에서 함수 한 번만 발동시키고 싶을 때 메소드보다 bool값 이용하여 코루틴으로 작성
            
    }

    IEnumerator usingTimeOfBloodLine(float endTime)     // 스킬 사용 후 범위 지정 가능 시간(공격 가능 시간)
    {
        // linerenderer, 마우스 포인터 따라가는 코드 작성해서 범위 UI 표시하기
        // 코루틴 내부의 함수는 다른 스크립트에서 참조할 수 없는 것 같음..
        isUsingTime = true;
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
        mouseClickScore = 0;
        isUsingTime = false;
    }

    private void designateRange()       // 마우스 클릭 두 번으로 범위 지정
    {   
        if (Input.GetMouseButtonDown(0) && mouseClickScore == 0 && isUsingTime)
        {
            firstMousePos = Input.mousePosition;
            mouseClickScore++;
        }
        else if (Input.GetMouseButtonDown(0) && mouseClickScore == 1 && isUsingTime)
        {
            secondMousePos = Input.mousePosition;
            mouseClickScore++;
        }
    }

    IEnumerator createAndDestroyAttackObj()
    {
        mouseClickScore = 0;            // 스킬 오브젝트가 중복 생성되지 않기 위함
        isExistLineObject = true;       // 없으면 스킬 시전 때 오브젝트 무수히 많이 생성됨

        Vector2 attackDir = secondMousePos - firstMousePos;
        Vector3 quaternionToTarget = Quaternion.Euler(0, 0, 90) * attackDir;        // 스킬 오브젝트를 secondMousePos 방향쪽으로 조정하는 코드

        var bloodLineCopy = Instantiate(BloodLine, firstMousePos, Quaternion.LookRotation(forward: Vector3.forward, upwards: quaternionToTarget));
        yield return new WaitForSeconds(1.2f);        // WaitForSeconds(a) >> a시간은 스킬 오브젝트 생성 시간동안만 할당

        Destroy(bloodLineCopy);
        isExistLineObject = false;
    }






}
