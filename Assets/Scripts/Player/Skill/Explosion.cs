using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PlayerSkill
{
    public GameObject attackPrefab;
    public GameObject attackObjectOutlinePrefab;

    public AudioClip explosionSFX;

    Vector2 mousePos;

    private void Update()
    {
        AttackSequence();
    }

    public override void Use()
    {
        ActionOnUse();
        StartCoroutine(UsingTimeSequence(usingTime));
    }

    IEnumerator UsingTimeSequence(float endTime)
    {
        canUse = false;
        isProgress = true;
        float startTime = 0f;

        while (true)                       
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
            {
                canUse = true;
                break;
            }
            else if (isAttacking)           // 스킬 사용 시 즉시 시전 종료
                startTime = endTime;

            yield return null;
        } // 스킬 시전 가능 시간 제어

        isProgress = false;
        _playerAttack.canAttack = true;
    }


    void AttackSequence()
    {
        if (isProgress)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
                mousePos = mainCamera.ScreenToWorldPoint(mousePos);

                SoundManager.instance.SFXPlayer("Explosion", explosionSFX);
                StartCoroutine(AttackObjectControl());
                cooltimeImage.fillAmount = 0;
                StartCoroutine(CooltimeSequence());
            }
        }
    }

    IEnumerator AttackObjectControl()       // 오브젝트 생성 및 존재 시간 관리
    {
        isAttacking = true;

        var attackObject = Instantiate(attackPrefab,
            mousePos,
            Quaternion.LookRotation(forward: Vector3.forward));
        var attackObjectOutline = Instantiate(attackObjectOutlinePrefab,
            mousePos,
            Quaternion.LookRotation(forward: Vector3.forward));
        yield return new WaitForSeconds(1.2f);

        Destroy(attackObject);
        Destroy(attackObjectOutline);

        yield return new WaitForSeconds(1.3f);
        isAttacking = false;
    }

    IEnumerator CooltimeSequence()
    {
        while (cooltimeImage.fillAmount < 1)
        {
            cooltimeImage.fillAmount += 1 * Time.smoothDeltaTime / cooltime;
            yield return null;
        }

        canUse = true;
        cooltimeImage.fillAmount = 1;
        yield break;
    }       // 쿨타임 적용

}
