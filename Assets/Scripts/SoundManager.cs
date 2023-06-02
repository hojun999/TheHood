using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mixer;
    public AudioSource bgSound;
    public AudioClip[] bglist;

    //private void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(instance);
    //        //SceneManager.sceneLoaded += OnSceneLoad;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            EnterMainMenu();

        if (SceneManager.GetActiveScene().name == "Camp")
            EnterCamp();
    }

    //private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    //{
    //    for (int i = 0; i < bglist.Length; i++)
    //    {
    //        if (arg0.name == bglist[i].name)
    //            BgSoundPlay(bglist[i]);
    //    }
    //}

    public void EnterWoods()
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (bglist[i].name == "Woods")
                BgSoundPlay(bglist[i]);
        }
    }

    public void EnterCamp()
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (bglist[i].name == "Camp")
                BgSoundPlay(bglist[i]);
        }
    }

    public void EnterMainMenu()
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (bglist[i].name == "MainMenu")
                BgSoundPlay(bglist[i]);
        }
    }

    public void EnterFight()
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (bglist[i].name == "FightSound")
                BgSoundPlay(bglist[i]);
        }
    }

    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGSoundVolume", MathF.Log10(val) * 20);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.5f;
        bgSound.Play();
    }
}
