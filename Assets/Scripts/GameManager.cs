using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    public Light2D playerLight;


    [Header("UI")]
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;
    public GameObject inventoryPanel;
    public GameObject moveWoodsUIPanel;
    public GameObject moveCampUIPanel;
    public GameObject subMenuUIPanel;
    public GameObject helpMenuPanel;
    public GameObject QuestClearText;
    public GameObject enterFightUIPanel;

    [Header("Manager")]
    public TalkManager talkManager;
    public QuestManager_Lagacy questManager_lagacy;
    public InventoryManager inventoryManager;

    [Header("Camera")]
    public Camera MainCamera;
    public Camera FightCamera;


    [Header("SpawnArea")]
    public GameObject CampSpawnArea;
    public GameObject WestSpawnArea;
    public GameObject EastSpawnArea;
    public GameObject NorthSpawnArea;

    [Header("Talk")]
    public int talkIndex;

    [Header("Material")]
    public Material unlitMaterial;
    public Material litMaterial;

    [Header("EnterFight")]
    public Transform enterFightPos;
    [HideInInspector]public bool isEnterFight;

    [Header("Quest3")]
    public GameObject fightWall_Quest3;
    public GameObject enemyGroup_Quest3;

    [Header("Quest4")]
    public GameObject fightWall_Quest4;
    public GameObject enemyGroup_Quest4;

    [Header("ScreenFadeOutAndLoadEnding")]
    public Image fadeOutImage;
    public float fadeOutMaxTime;


    [HideInInspector]public bool isAction;
    [HideInInspector]public bool activeInventory = false;
    private bool activeSubMenu;
    private bool activeHelpMenu;
    //private bool isEnding;

    private int spawnNum;
    private float fadeOutCurTime;
    private float fadeInCurTime;

    public int getWeaponTradeTalkIndex;
    public int getPosionTradeTalkIndex;
    public int isGetAlreadyPosionNum;

    private void Start()
    {
        activeHelpMenu = true;
        //SoundManager.instance.bgSound.volume = 1;

    }

    public void talkAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isQuestNpc, objData.isWeaponTraderNpc, objData.isPosionTraderNpc);
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isQuestNpc, bool isWeaponTradeNpc, bool isPosionTradeNpc)
    {
        if (questManager_lagacy.eliminateHenchmanNum_Quest3 == 6)
            enemyGroup_Quest3.SetActive(false);

        if (questManager_lagacy.eliminateBossNum_Quest4 == 2 && questManager_lagacy.eliminateHenchmanNum_Quest4 == 6)
            enemyGroup_Quest4.SetActive(false);

        inventoryManager.DestroyQuestItemAndTradeEtcItem();

        // ��ȭ ������ ����, �� npc id���� �ۼ� ��
        if(id == 2000)
        {
            int questTalkIndex = questManager_lagacy.GetQuestTalkIndex(id);
            string questTalkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

            if (questTalkData == null && questManager_lagacy.questId == 50)
            {
                //isEnding = true;
            }
            if (questTalkData == null && id == 2000 && !isWeaponTradeNpc && !isPosionTradeNpc)
            {
                isAction = false;
                talkIndex = 0;
                questManager_lagacy.checkQuest(id);
                return;
            }


            if (isQuestNpc && id == 2000)
            {
                talkText.text = questTalkData;
            }

        }
        else if(id == 1000)
        {
            int weaponTradeTalkIndex = getWeaponTradeTalkIndex;
            string weaponTradeTalkData = talkManager.GetTalk(id + weaponTradeTalkIndex, talkIndex);

             if (isWeaponTradeNpc && id == 1000)
            {
                talkText.text = weaponTradeTalkData;
            }


            if (weaponTradeTalkData == null && id == 1000)
            {
                isAction = false;
                talkIndex = 0;
                return;
            }

        }
        else if(id == 3000)
        {
            int posionTradeTalkIndex = getPosionTradeTalkIndex;
            string posionTradeTalkData = talkManager.GetTalk(id + posionTradeTalkIndex, talkIndex);
            isGetAlreadyPosionNum++;

            if (posionTradeTalkData == null && id == 3000)
            {
                isAction = false;
                talkIndex = 0;
                return;
            }

            else if (isPosionTradeNpc && id == 3000)
            {
                talkText.text = posionTradeTalkData;
            }


        }


        // ĳ������ �� ��ȭ�� ������ ��

        // ��ȭ �б���(npc, ������ ���� ���� ����)
        //else
        //{
        //    talkText.text = questTalkData;
        //}


        isAction = true;
        talkIndex++;

        Debug.Log(isGetAlreadyPosionNum);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        activeInventory = !activeInventory;
    //        inventoryPanel.SetActive(activeInventory);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Escape) && !activeHelpMenu)        // helpmenu �������� ����(����ó��)
    //        OnOffSubMenuPanel();

    //    if (activeHelpMenu)     // ����ó��
    //    {
    //        if(Input.GetKeyDown(KeyCode.Escape))
    //            OnOffHelpMenuPanel();
    //    }


    //    if(questManager_lagacy.locateAtQuestAreaNum_Quest1 == 2)   // ����Ʈ 1 ó��
    //    {
    //        Invoke("setActiveQuestClearText", 1f);
    //        questManager_lagacy.NextQuest();
    //        questManager_lagacy.required_Area_Quest1.SetActive(false);
    //        questManager_lagacy.locateAtQuestAreaNum_Quest1++;
    //        Player.GetComponent<PlayerController>().questionMark.SetActive(false);
    //        Player.GetComponent<PlayerController>().exMark.SetActive(true);
    //        questManager_lagacy.Direction_Up.SetActive(true);
    //        questManager_lagacy.Direction_Right.SetActive(false);
    //    }

    //    if (questManager_lagacy.getItemNum_Quest2 == 0)        // ����Ʈ2 ó��
    //    {
    //        Invoke("setActiveQuestClearText", 1f);
    //        questManager_lagacy.NextQuest();
    //        questManager_lagacy.required_ItemGroup_Quest2.SetActive(false);
    //        questManager_lagacy.Direction_Up.SetActive(true);
    //        questManager_lagacy.Direction_Right.SetActive(false);

    //        questManager_lagacy.getItemNum_Quest2 += 100;      // ���ǹ� �� ���� ȣ��
    //    }

    //    if(questManager_lagacy.eliminateHenchmanNum_Quest3 == 5)      // ����Ʈ3 ó��
    //    {
    //        ConvertcameraFightToNormal();
    //        Invoke("setActiveQuestClearText", 1f);
    //        questManager_lagacy.NextQuest();
    //        //isEnterFight = false;
    //        //fightWall_Quest3.SetActive(false);
    //        Player.GetComponent<PlayerController>().BeforeText_eliminateHenchman_Quest3.SetActive(false);
    //        Player.GetComponent<PlayerController>().AfterText_eliminateHenchman_Quest3.SetActive(true);
    //        questManager_lagacy.Direction_Up.SetActive(true);
    //        questManager_lagacy.Direction_Right.SetActive(false);

    //        questManager_lagacy.eliminateHenchmanNum_Quest3++;        // ���ǹ� �� ���� ȣ���ϱ� ����
    //    }

    //    // ����Ʈ 4 ó��
    //    if (questManager_lagacy.eliminateBossNum_Quest4 == 1)
    //    {
    //        Player.GetComponent<PlayerController>().BeforeText_eliminateBoss.SetActive(false);
    //        Player.GetComponent<PlayerController>().AfterText_eliminateBoss.SetActive(true);
    //    }

    //    if(questManager_lagacy.eliminateHenchmanNum_Quest4 == 6)
    //    {
    //        Player.GetComponent<PlayerController>().BeforeText_eliminateHenchman_Quest4.SetActive(false);
    //        Player.GetComponent<PlayerController>().AfterText_eliminateHenchman_Quest4.SetActive(true);
    //    }

    //    if (questManager_lagacy.eliminateHenchmanNum_Quest4 == 6 && questManager_lagacy.eliminateBossNum_Quest4 == 1)      // ����Ʈ 4 ó��
    //    {
    //        ConvertcameraFightToNormal();
    //        SoundManager.instance.EnterWoods();
    //        Invoke("setActiveQuestClearText", 1f);
    //        questManager_lagacy.NextQuest();
    //        isEnterFight = false;
    //        fightWall_Quest4.SetActive(false);
    //        questManager_lagacy.Direction_Up.SetActive(true);
    //        questManager_lagacy.Direction_Right.SetActive(false);
    //        questManager_lagacy.Direction_Quest3AndQuest4.SetActive(false);


    //        questManager_lagacy.eliminateBossNum_Quest4++;        // ���ǹ� �� ���� ȣ���ϱ� ����
    //    }

    //    if (isEnding)
    //    {
    //        if (fadeOutCurTime < fadeOutMaxTime)
    //        {
    //            fadeOutCurTime += Time.deltaTime;
    //            PlayFadeOut();
    //        }
    //        if (fadeOutCurTime > fadeOutMaxTime)
    //            SceneManager.LoadScene("Ending");
    //    }

    //    if (isGetAlreadyPosionNum == 2)
    //        isGetAlreadyPosionNum = 0;

    //    if(SoundManager.instance.getStartNum == 1)
    //    {
    //        if (fadeOutCurTime < fadeOutMaxTime)
    //        {
    //            fadeOutCurTime += Time.deltaTime;
    //            PlayFadeIn();
    //        }
    //        if (fadeOutCurTime > fadeOutMaxTime)
    //            SoundManager.instance.getStartNum--;
    //    }
    //}


    public void setActiveQuestClearText()
    {
        //  ĵ���� �ȿ� UI text ����
            Instantiate(QuestClearText, QuestClearText.transform.position, Quaternion.identity, GameObject.Find("UI").transform);
    }

    public void LocatePlayerAtCamp()      // ķ���� �̵�
    {
        isAction = false;
        Player.transform.position = CampSpawnArea.transform.position;
        playerLight.falloffIntensity = 0.6f;
        SoundManager.instance.EnterCamp();
        
        MainCamera.GetComponent<CameraController>().center = new Vector2(0, 0);
        MainCamera.GetComponent<CameraController>().size = new Vector2(18, 10);
        Player.GetComponent<PlayerController>().isPlayerInWoods = false;
        //Player.GetComponent<SpriteRenderer>().material = litMaterial;

        moveCampUIPanel.SetActive(false);

        WestSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);
        EastSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);
        NorthSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);

        Time.timeScale = 1f;
    }

    public void LocatePlayerAtWoods()
    {
        isAction = false;
        GetRandomSpawnNum();
        playerLight.falloffIntensity = 0.9f;
        SoundManager.instance.EnterWoods();

        switch (spawnNum)
        {
            case 1:
                Player.transform.position = WestSpawnArea.transform.position;
                break;
            case 2:
                Player.transform.position = NorthSpawnArea.transform.position;
                break;
            case 3:
                Player.transform.position = EastSpawnArea.transform.position;
                break;
        }

        MainCamera.GetComponent<CameraController>().center = new Vector2(62.5f, 9);
        MainCamera.GetComponent<CameraController>().size = new Vector2(54, 28);
        moveWoodsUIPanel.SetActive(false);
        Player.GetComponent<PlayerController>().isPlayerInWoods = true;
        //Player.GetComponent<SpriteRenderer>().material = unlitMaterial;

        WestSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);
        EastSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);
        NorthSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);

        Time.timeScale = 1f;
    }

    private void GetRandomSpawnNum()
    {
        spawnNum = Random.Range(1, 4);
    }

    public void CloseMoveWoodsUIPanelandResume()
    {
        moveWoodsUIPanel.SetActive(false);
        isAction = false;
        Time.timeScale = 1f;
    }

    public void CloseMoveCampUIPanelandResume()
    {
        moveCampUIPanel.SetActive(false);
        isAction = false;
        Time.timeScale = 1f;
    }

    public void CloseEnterFightUIPanelAndResume()
    {
        enterFightUIPanel.SetActive(false);
        isAction = false;
        Time.timeScale = 1f;
    }

    public void ResumeGame()        // ���� �Ͻ����� ����
    {
        activeSubMenu = false;
        subMenuUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadCamp()
    {
        SceneManager.LoadScene("Camp");
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void TurnOffGame()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void OnOffSubMenuPanel()
    {
        activeSubMenu = !activeSubMenu;
        subMenuUIPanel.SetActive(activeSubMenu);
        Time.timeScale = activeSubMenu ? 0 : 1;
    }

    public void OnOffHelpMenuPanel()
    {
        activeHelpMenu = !activeHelpMenu;
        helpMenuPanel.SetActive(activeHelpMenu);
        Time.timeScale = activeHelpMenu ? 0 : 1;
    }

    //enterfight
    #region
    public void ConverCameraNormalToFight()
    {
        MainCamera.depth = 0;
        FightCamera.depth = 1;
    }

    public void ConvertcameraFightToNormal()
    {
        MainCamera.depth = 1;
        FightCamera.depth = 0;
    }

    public void EnterFightSetting()
    {
        Player.transform.position = enterFightPos.position;
        SoundManager.instance.EnterFight();
        isEnterFight = true;
    }

    #endregion


    void PlayFadeOut()
    {
        Color color = fadeOutImage.color;
        color.a = Mathf.Lerp(0f, 1f, fadeOutCurTime / 3.5f);
        fadeOutImage.color = color;
    }

    void PlayFadeIn()
    {
        Color color = fadeOutImage.color;
        color.a = Mathf.Lerp(1f, 0f, fadeOutCurTime / 3.5f);
        fadeOutImage.color = color;
    }

    public void OpenSoundControlPanel()
    {
        SoundManager.instance.OpenSoundControlPanel();
    }

    public void ClosdSoundControlPanel()
    {
        SoundManager.instance.CloseSoundControllPanel();

    }

}
