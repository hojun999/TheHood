using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroTextBlink : MonoBehaviour
{
    public GameObject introTextManager;

    public Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(FadeTextToZero());
    }

    public IEnumerator FadeTextToZero()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2));

            yield return null;
        }
    }
}