using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    GameManager gameManager;
    PlayerAttack _playerAttack;

    SpriteRenderer sr;
    Animator anim;

    public GameObject magicSquareHolder;

    public Slider hpBar;
    public Slider mpBar;

    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;

    public int naturalHealingPower_Hp;
    public int naturalHealingPower_Mp;
    public float naturalHealingCooltime;
    float naturalHealingTime;

    bool isHit;

    private void Start()
    {
        SetOnStart();
    }

    void SetOnStart()
    {
        gameManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        _playerAttack = gameObject.GetComponent<PlayerAttack>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        curHp = Mathf.Clamp(maxHp, 0, maxHp);
        curMp = Mathf.Clamp(maxMp, 0, maxMp);

        hpBar.value = 1;
        mpBar.value = 1;
    }

    private void Update()
    {
        HandleCurrentStats();
        HandleNaturalHealing();
        HandleStatsUI();
    }

    void HandleCurrentStats()
    {
        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curMp = Mathf.Clamp(curMp, 0, maxMp);
    }

    void HandleStatsUI()
    {
        if (hpBar != null)
            hpBar.value = Utils.Percent(curHp, maxHp);

        if (mpBar != null)
            mpBar.value = Utils.Percent(curMp, maxMp);
    }

    void HandleNaturalHealing()
    {
        if(naturalHealingTime < naturalHealingCooltime)
        {
            naturalHealingTime += Time.deltaTime;
        }
        else
        {
            curHp += naturalHealingPower_Hp;
            curMp += naturalHealingPower_Mp;
            naturalHealingTime = 0;
        }
    }

    public void OnHit(int damage)
    {
        if (!isHit)
        {
            isHit = true;
            curHp -= damage;

            if (curHp <= 0)
                Die();
            else
                HitSequence();
        }
    }

    void HitSequence()
    {
        StartCoroutine(HandleHitState());
        StartCoroutine(AlphaBlinkOnHit());
    }

    IEnumerator HandleHitState()
    {
        yield return new WaitForSeconds(2f);
        isHit = false;
    }

    IEnumerator AlphaBlinkOnHit()
    {
        while (isHit)
        {
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 1);
        }
    }

    void Die()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _playerAttack.enabled = false;
        magicSquareHolder.SetActive(false);
        anim.SetTrigger("isDie");
        Invoke("UIControlOnDie", 2f);
    }

    void UIControlOnDie()
    {
        gameManager.OpenDieUI();
    }
}
