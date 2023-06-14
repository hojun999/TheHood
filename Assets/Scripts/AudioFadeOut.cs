using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;

    private float volume;
    private float timer;

    private void Start()
    {
        volume = SoundManager.instance.bgSound.volume;
        timer = fadeDuration;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            float time = Mathf.Clamp01(timer / fadeDuration);
            SoundManager.instance.bgSound.volume = volume * time;
        }
    }
}
