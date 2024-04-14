using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextBlink : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.DOFade(0, 2f);
    }
}
