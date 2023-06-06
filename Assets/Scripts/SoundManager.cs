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
    public AudioSource audioSource;
    public AudioClip[] bglist;
    public GameObject soundControlPanelInMainMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
        }
    }

    private void Start()
    {
        //if (SceneManager.GetActiveScene().name == "MainMenu")
        //    EnterMainMenu();

        //if (SceneManager.GetActiveScene().name == "Camp")
        //    EnterCamp();
    }


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
        mixer.SetFloat("BGSound", MathF.Log10(val) * 20);
    }

    public void SFXVolume(float val)
    {
        mixer.SetFloat("SFXVolume", MathF.Log10(val) * 20);
    }



    public void BgSoundPlay(AudioClip clip)
    {
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    public void SFXPlayer(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audiosource.clip = clip;
        audiosource.volume = 0.05f;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void OpenSoundControlPanel()
    {
        soundControlPanelInMainMenu.SetActive(true);
    }

    public void CloseSoundControllPanel()
    {
        soundControlPanelInMainMenu.SetActive(false);
    }

}
