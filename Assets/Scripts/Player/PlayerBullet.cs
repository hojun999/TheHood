using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;

    private Vector3 mousePos;

    public int damage;
    public float fireMovePower;


    void Start()
    {
        SetOnStart();
        SetBulletState();

        Destroy(gameObject, 1.5f);      // 피격되지 않았을 때 오브젝트 제거
    }

    void SetOnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void SetBulletState()
    {
        // bullet 오브젝트의 움직임
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shotDir = mousePos - transform.position;
        Vector3 shotRotation = transform.position - mousePos;
        rb.velocity = new Vector2(shotDir.x, shotDir.y).normalized * fireMovePower;

        // bullet obj 발사 각도
        float rot = Mathf.Atan2(shotRotation.y, shotRotation.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.AngleAxis(rot, Vector3.forward);
        transform.rotation = bulletRotation;
        transform.localEulerAngles += new Vector3(0, 0, 180);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBoss"))
        {
            collision.gameObject.GetComponent<EnemyBoss>().OnHit(damage);

            Destroy(gameObject);

        }

        if (collision.CompareTag("EnemyNormal"))
        {
            collision.gameObject.GetComponent<EnemyNormal>().OnHit(damage);

            Destroy(gameObject);
        }
    }

}
