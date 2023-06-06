using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanFightTrigger : MonoBehaviour
{
    //public GameObject Henchman;     // 자기 자신의 정보만 가져오기 위함( HenchmanAI로 가져오면 멀리 있는 애들도 다 전투상태 될것 같음)
    //부모component가져오는 함수로 대체 가능했음.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<HenchmanAI_Quest3>().enabled = true;
            GetComponentInParent<HenchmanAI_Quest3>().isStartFight = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}
