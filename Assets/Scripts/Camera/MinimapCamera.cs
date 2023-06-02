using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("Minimap rotations")]
    public Transform PlayerPos;


    void Update()
    {
        if(PlayerPos != null)
        {
            transform.position = new Vector3(PlayerPos.position.x, PlayerPos.position.y, PlayerPos.position.z - 20);
        }
    }}
