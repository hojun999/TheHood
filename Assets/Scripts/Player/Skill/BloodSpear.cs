using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpear : PlayerSkill
{
    public GameObject attackPrefab;

    Vector2 firstMousePos, secondMousePos;

    int mouseClickCount;

    private void Update()
    {
        AttackSequence();
    }

    public override void Use()
    {
        ActionOnUse();
        StartCoroutine(UsingTimeSequence(usingTime));
    }

    // ���� �ð� ����
    IEnumerator UsingTimeSequence(float endTime)
    {
        canUse = false;                     // ��ų ��� ����
        isProgress = true;                  // ��ų ���� ��
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
        }

        mouseClickCount = 0;
        isProgress = false;
        _playerAttack.canAttack = true;
    }

    void AttackSequence()
    {
        if (isProgress)
        {
            if (Input.GetMouseButtonDown(0) && mouseClickCount == 0)                // ������Ʈ ���� ��ġ ����
            {
                firstMousePos = Input.mousePosition;
                firstMousePos = mainCamera.ScreenToWorldPoint(firstMousePos);
                mouseClickCount++;
            }
            else if (Input.GetMouseButtonDown(0) && mouseClickCount == 1)           // ������Ʈ ���� �� ���� ����
            {
                secondMousePos = Input.mousePosition;
                secondMousePos = mainCamera.ScreenToWorldPoint(secondMousePos);
                mouseClickCount++;
            }

            if (mouseClickCount == 2 && !isAttacking)                               // ��ų ����
            {
                StartCoroutine(AttackObjectControl());
                cooltimeImage.fillAmount = 0;
                StartCoroutine(CooltimeSequence());
            }
        }
    }

    IEnumerator AttackObjectControl()
    {
        mouseClickCount = 0;
        isAttacking = true;

        Vector2 attackDir = secondMousePos - firstMousePos;
        Vector3 rotationValueToTarget = Quaternion.Euler(0, 0, 90) * attackDir;

        var attackObject = Instantiate(attackPrefab,
            firstMousePos,
            Quaternion.LookRotation(forward: Vector3.forward, upwards: rotationValueToTarget));
        yield return new WaitForSeconds(1.2f);

        Destroy(attackObject);

        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }

    IEnumerator CooltimeSequence()      // ��Ÿ�� ����
    {
        while (cooltimeImage.fillAmount < 1)
        {
            cooltimeImage.fillAmount += 1 * Time.smoothDeltaTime / cooltime;
            yield return null;
        }

        canUse = true;
        cooltimeImage.fillAmount = 1;
        yield break;
    }

}
