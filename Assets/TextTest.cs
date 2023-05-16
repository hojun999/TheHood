using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    Text text;
    float time = 2.5f;
    float cooltime;

    private void Awake()
    {
        text = GetComponent<Text>();
        StartCoroutine(FadeTextToZero());
    }

    private void Update()
    {
        cooltime += Time.deltaTime;

        if(cooltime > time)
        {
            gameObject.SetActive(false);
            cooltime = 0;
        }
    }


    public IEnumerator FadeTextToZero()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while(text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.2f));
            yield return null;
        }
    }
}
