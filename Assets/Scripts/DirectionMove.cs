using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMove : MonoBehaviour
{
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {

        if (gameObject.CompareTag("UpDirection"))
        {
            if (transform.position.y > startPos.y + 0.5f)
                transform.position = startPos;

            transform.Translate(Vector2.up * Time.deltaTime * 0.6f);
        }

        if (gameObject.CompareTag("LeftDirection"))
        {
            if(transform.position.x < startPos.x - 0.5f)
                transform.position = startPos;

            transform.Translate(Vector2.left * Time.deltaTime * 0.6f);
        }

        if (gameObject.CompareTag("RightDirection"))
        {
            if (transform.position.x > startPos.x + 0.5f)
                transform.position = startPos;

            transform.Translate(Vector2.right * Time.deltaTime * 0.6f);
        }
    }
}
