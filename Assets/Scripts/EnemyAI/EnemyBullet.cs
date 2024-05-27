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
        SetOnStart();
        SetBulletState();
    }

    void SetOnStart()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerPos = Player.GetComponent<Transform>();
    }

    void SetBulletState()
    {
        // bullet 속도
        attackAngle = Random.Range(playerPos.position.x - 1f, playerPos.position.x + 1f);
        attackAngleVector = new Vector3(attackAngle, playerPos.position.y, 0);

        Vector3 shotingDir = attackAngleVector - transform.position;
        Vector3 shotingRotation = transform.position - attackAngleVector;
        rb.velocity = new Vector2(shotingDir.x, shotingDir.y).normalized * fireForce;

        // bullet obj 발사 각도
        float rot = Mathf.Atan2(shotingRotation.y, shotingRotation.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.AngleAxis(rot, Vector3.forward);
        transform.rotation = bulletRotation;
        transform.localEulerAngles += new Vector3(0, 0, 180);

        Destroy(gameObject, 1.5f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStat>().OnHit(damage);
            Destroy(gameObject);
        }
    }

}
