using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("Minimap rotations")]
    public Transform playerReference;


    void Update()
    {
        if(playerReference != null)
        {
            transform.position = playerReference.position;
        }
    }
}
