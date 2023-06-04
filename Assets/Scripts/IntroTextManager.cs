using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTextManager : MonoBehaviour
{
    private GameObject introText;

    [SerializeField] private int introTextIndex;

    public float time;
    float cooltime;

    public List<GameObject> introTextList = new List<GameObject>();

    private void Awake()
    {
        Invoke("LoadCamp", time * introTextList.Count);
    }

    void Start()
    {
        introText = introTextList[0];
        introText.SetActive(true);

    }

    private void Update()
    {
        cooltime += Time.deltaTime;

        if (cooltime >= time)
        {
            introTextList[introTextIndex].gameObject.SetActive(false);
            introTextIndex++;
            GetNextText();
            cooltime = 0;      
        }
    }

    void GetNextText()
    {
        introText = introTextList[introTextIndex];
        introText.gameObject.SetActive(true);
    }

    public void LoadCamp()
    {
        SceneManager.LoadScene("Camp");
    }

}
