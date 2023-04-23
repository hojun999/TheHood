using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float fireForce;

    void Start()
    {
        // bullet 속도
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootingDir = mousePos - transform.position;
        Vector3 shootingRot = transform.position - mousePos;
        rb.velocity = new Vector2(shootingDir.x, shootingDir.y).normalized * fireForce;


        // bullet obj 발사 각도
        float rot = Mathf.Atan2(shootingRot.y, shootingRot.x) * Mathf.Rad2Deg;
        Quaternion bulletRot = Quaternion.AngleAxis(rot, Vector3.forward);
        transform.rotation = bulletRot;
        transform.localEulerAngles += new Vector3(0, 0, 180);

        Destroy(gameObject, 2f);
    }

}
