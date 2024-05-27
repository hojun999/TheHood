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
            else if (isAttacking)           // ��ų ��� �� ��� ���� ����
                startTime = endTime;

            yield return null;
        } // ��ų ���� ���� �ð� ����

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

    IEnumerator AttackObjectControl()       // ������Ʈ ���� �� ���� �ð� ����
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
    }       // ��Ÿ�� ����

}
