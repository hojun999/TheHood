using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector]public GameObject Player;
    [HideInInspector]public Transform playerPos;

    private Vector3 mousePos;
    private Vector3 attackAngleVector;

    private Camera mainCamera;
    private Rigidbody2D rb;
    public float fireForce;
    private float attackAngle;

    public int damage;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        // bullet 속도
        rb = GetComponent<Rigidbody2D>();
        playerPos = Player.GetComponent<Transform>();

        attackAngle = Random.Range(playerPos.position.x - 1f, playerPos.position.x + 1f);
        attackAngleVector = new Vector3(attackAngle, playerPos.position.y, 0);

        Vector3 shootingDir = attackAngleVector - transform.position;
        Vector3 shootingRot = transform.position - attackAngleVector;
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
        if (collision.CompareTag("Player"))
        {
            Player.GetComponent<PlayerController>().Hurt(damage);
            Destroy(gameObject);
        }
    }

}
