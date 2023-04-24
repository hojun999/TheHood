using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerPos;

    private void Update()
    {
        gameObject.transform.position = PlayerPos.position;
    }


}
