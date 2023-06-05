using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    public GameObject GameManager;

    private Vector3 mousePos;
    private Camera MainCamera;
    private Rigidbody2D rb;
    public float fireForce;


    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");


        if (GameManager.GetComponent<GameManager>().isEnterFight)
            MainCamera = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>();
        else
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // bullet 속도
        //MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootingDir = mousePos - transform.position;
        Vector3 shootingRot = transform.position - mousePos;
        rb.velocity = new Vector2(shootingDir.x, shootingDir.y).normalized * fireForce;


        // bullet obj 발사 각도
        float rot = Mathf.Atan2(shootingRot.y, shootingRot.x) * Mathf.Rad2Deg;
        Quaternion bulletRot = Quaternion.AngleAxis(rot, Vector3.forward);
        transform.rotation = bulletRot;
        transform.localEulerAngles += new Vector3(0, 0, 180);

        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossAI>().Hurt(damage);
            // 피격 사운드
            Destroy(gameObject);

        }

        if (collision.CompareTag("Henchman"))
        {
            collision.gameObject.GetComponent<HenchmanAI>().Hurt(damage);
            // 피격 사운드

            Destroy(gameObject);
        }
    }
}
