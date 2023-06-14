using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTextManager : MonoBehaviour
{
    public typewriterUI_v2 typingManager;

    //private GameObject introText;

    //[SerializeField] private int introTextIndex;

    //public float time;
    //float cooltime;

    //public List<GameObject> introTextList = new List<GameObject>();

    private void Awake()
    {
    }

    //void Start()
    //{
    //    introText = introTextList[0];
    //    introText.SetActive(true);

    //}

    private void Update()
    {
        //cooltime += Time.deltaTime;

        //if (cooltime >= time)
        //{
        //    introTextList[introTextIndex].gameObject.SetActive(false);
        //    introTextIndex++;
        //    GetNextText();
        //    cooltime = 0;      
        //}

        if(typingManager.text.text.Length == 382)
        {
            //typingManager.tmpProText.GetComponent<IntroTextBlink>().enabled = true;

            typingManager.text.GetComponent<IntroTextBlink>().enabled = true;
            typingManager.text.GetComponent<AudioFadeOut>().enabled = true;
            Invoke("LoadCamp", 4);
        }
    }

    //void GetNextText()
    //{
    //    introText = introTextList[introTextIndex];
    //    introText.gameObject.SetActive(true);
    //}

    public void LoadCamp()
    {
        SceneManager.LoadScene("Camp");
        SoundManager.instance.getStartNum = 1;
    }

}
