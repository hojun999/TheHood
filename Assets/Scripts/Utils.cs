using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float Percent(float current, float max)
    {
        return current != 0 && max != 0 ? current / max : 0;
     }
}
