using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseSkill : MonoBehaviour
{
    Camera MainCamera;

    public GameObject gameManager;
    public PlayerController playerController;
    public GameObject enterFightArea;
    public AudioClip clip;

    [Header("LineAttackObject")]
    public GameObject LineAttackObject;
    public int requiredLineAttackMp;

    [Header("ExplosionObject")]
    public GameObject explosionAttackObj;
    public GameObject explosionOutline;
    public int requiredExplosionMp;

    [Header("CooltimeImage")]
    public Image LineAttackCoolTimeImage;
    public Image explosionCoolTimeImage;

    Vector2 firstMousePosOfLineAttack, secondMousePosOfLineAttack, mousePosOfExplosion;
    [HideInInspector] public int mouseClickScoreOfLineAttack;
    [HideInInspector] public int mouseclickScoreOfExplosion;

    [HideInInspector] public float axis;

    [HideInInspector] public bool isLineAttackUsing;    // ��ų ������ ������ �ð� ���� true
    [HideInInspector] public bool isExplosionUsing;
    [HideInInspector] public bool isExistLineObject;      // ��ų�� ���������
    [HideInInspector] public bool isExistExplosionObject;


    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        //if (gameManager.GetComponent<GameManager>().isEnterFight)
        //    MainCamera = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>();
        //else
        //    MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // LineAttack
        #region
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isLineAttackUsing && playerController.isCanUseLineAttack)      // �ڷ�ƾ�� �����Ͽ� ��ų ����� ���� ���콺 Ŭ�� �� ���� �Է� ���� ���¸� ����
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


        if (mouseClickScoreOfLineAttack == 2 && !isExistLineObject)    // ���콺 Ŭ�� �� ��°�� �� ����obj ����
        {
            StartCoroutine(createAndDestroyLineAttackObj());        // ��update������ �Լ� �� ���� �ߵ���Ű�� ���� �� �޼ҵ庸�� bool�� �̿��Ͽ� �ڷ�ƾ���� �ۼ�
            LineAttackCoolTimeImage.fillAmount = 0;
            StartCoroutine(LineAttackCoolTime(3f));
        }
            #endregion

        // Explosion
        #region
        if (Input.GetMouseButtonDown(2) && !isExplosionUsing && playerController.isCanUseExplosion)
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
        {
            SoundManager.instance.SFXPlayer("ExplosionAttack", clip);
            StartCoroutine(createAndDestroyExplosionObj());
            explosionCoolTimeImage.fillAmount = 0;
            StartCoroutine(ExplosionCoolTime(5f));
        }
        #endregion

    }

    // LineAttack Coroutine
    #region
    IEnumerator usingTimeOfLineAttack(float endTime)     // ��ų ��� �� ���� ���� ���� �ð�(���� ���� �ð�)
    {
        isLineAttackUsing = true;
        float startTime = 0f;

        while (true)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
                break;
            else if (isExistLineObject)     // ��ų�� ����ߴٸ� ��� ���ð� ������(����ϰ� ��� �ð� �ȿ� ���콺 Ŭ�� �� ���� �� ���� ��, ��ų �ٽ� ����ϴ� �� ����)
                startTime = endTime;
            yield return null;
        }
        mouseClickScoreOfLineAttack = 0;
        isLineAttackUsing = false;
    }

    IEnumerator createAndDestroyLineAttackObj()
    {
        mouseClickScoreOfLineAttack = 0;            // ��ų ������Ʈ�� �ߺ� �������� �ʱ� ����
        isExistLineObject = true;       // ������ ��ų ���� �� ������Ʈ ������ ���� ������

        Vector2 attackDir = secondMousePosOfLineAttack - firstMousePosOfLineAttack;
        Vector3 quaternionToTarget = Quaternion.Euler(0, 0, 90) * attackDir;        // ��ų ������Ʈ�� secondMousePos ���������� �����ϴ� �ڵ�

        var attackObjectCopy = Instantiate(LineAttackObject, firstMousePosOfLineAttack, Quaternion.LookRotation(forward: Vector3.forward, upwards: quaternionToTarget));
        playerController.UseSkill(requiredLineAttackMp);
        yield return new WaitForSeconds(1.2f);        // WaitForSeconds(a) >> a�ð��� ��ų ������Ʈ ���� �ð����ȸ� �Ҵ�

        Destroy(attackObjectCopy);

        yield return new WaitForSeconds(2f);        // ��Ÿ�� ����
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
            else if (isExistExplosionObject)       // ��ų ���� ��� ��Ÿ�� ������
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
        playerController.UseSkill(requiredExplosionMp);

        yield return new WaitForSeconds(1.2f);

        Destroy(attackObjectCopy);
        Destroy(outlineObjectCopy);

        yield return new WaitForSeconds(4f);        // ��Ÿ�� ����
        isExistExplosionObject = false;
    }
    #endregion


    IEnumerator LineAttackCoolTime(float coolTime)
    {
        while (LineAttackCoolTimeImage.fillAmount < 1)
        {
            LineAttackCoolTimeImage.fillAmount += 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }

        //LineAttackObject.GetComponent<LineAttack>().isCanHitLineAttack = true;
        LineAttackCoolTimeImage.fillAmount = 1;
        yield break;
    }

    IEnumerator ExplosionCoolTime(float coolTime)
    {
        while (explosionCoolTimeImage.fillAmount < 1)
        {
            explosionCoolTimeImage.fillAmount += 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }

        //explosionOutline.GetComponent<ExplosionAttack>().isCanHitExplosion = true;
        explosionCoolTimeImage.fillAmount = 1;
        yield break;
    }





}
