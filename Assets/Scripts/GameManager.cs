using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Material")]
    public Material unlitMaterial;
    public Material litMaterial;

    [Header("EnterFight")]
    public Transform enterFightPos;
    [HideInInspector]public bool isEnterFight;

    [HideInInspector]public bool activeInventory = false;

    //------------------------

    private InventoryManager _inventoryManager;
    private PlayerInteract _playerInteract;

    [Header("Player")]
    public GameObject player;
    public Light2D playerLight;

    [Header("Camera")]
    public Camera MainCamera;
    public Camera FightCamera;

    [Header("UI")]
    public GameObject inventoryUI;          //인벤토리 UI
    public GameObject enterWoodsUI;         // 숲 진입 UI
    public GameObject enterCampUI;          // 캠프 진입 UI
    public GameObject subMenuUI;         // 옵션 메뉴 UI
    public GameObject helpMenuUI;           // 도움말 UI
    public GameObject questClearText;
    public GameObject enterFightUI;         // 전투 시작 UI (퀘스트3)
    public Image fadeImage;                 // 페이드 인/아웃 이미지

    [Header("Spawn")]   // 스폰 시스템
    public Animator[] woods_spawn_areas;
    public Transform CampSpawnArea;

    [HideInInspector] public bool isUIInteract;
    private bool isActiveSubMenu;
    private bool isActiveHelpMenu;

    private void Start()
    {
        isActiveHelpMenu = true;        // 하이어라키에서 도움말 액티브된 상태로 시작하기


        _inventoryManager = gameObject.GetComponent<InventoryManager>();
        _playerInteract = player.GetComponent<PlayerInteract>();

    }


    private void Update()
    {
        HandleUIInput();
    }

    //public void talkAction(GameObject scanObj)
    //{
    //    scanObject = scanObj;
    //    ObjData objData = scanObject.GetComponent<ObjData>();
    //    Talk(objData.id, objData.isQuestNpc, objData.isWeaponTraderNpc, objData.isPosionTraderNpc);
    //    talkPanel.SetActive(isAction);
    //}

    //void Talk(int id, bool isQuestNpc, bool isWeaponTradeNpc, bool isPosionTradeNpc)
    //{
    //    if (questManager_lagacy.eliminateHenchmanNum_Quest3 == 6)
    //        enemyGroup_Quest3.SetActive(false);

    //    if (questManager_lagacy.eliminateBossNum_Quest4 == 2 && questManager_lagacy.eliminateHenchmanNum_Quest4 == 6)
    //        enemyGroup_Quest4.SetActive(false);

    //    inventoryManager.DestroyQuestItemAndTradeEtcItem();

    //    // 대화 데이터 세팅, 각 npc id마다 작성 ★
    //    if(id == 2000)
    //    {
    //        int questTalkIndex = questManager_lagacy.GetQuestTalkIndex(id);
    //        string questTalkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

    //        if (questTalkData == null && questManager_lagacy.questId == 50)
    //        {
    //            //isEnding = true;
    //        }
    //        if (questTalkData == null && id == 2000 && !isWeaponTradeNpc && !isPosionTradeNpc)
    //        {
    //            isAction = false;
    //            talkIndex = 0;
    //            questManager_lagacy.checkQuest(id);
    //            return;
    //        }


    //        if (isQuestNpc && id == 2000)
    //        {
    //            talkText.text = questTalkData;
    //        }

    //    }
    //    else if(id == 1000)
    //    {
    //        int weaponTradeTalkIndex = getWeaponTradeTalkIndex;
    //        string weaponTradeTalkData = talkManager.GetTalk(id + weaponTradeTalkIndex, talkIndex);

    //         if (isWeaponTradeNpc && id == 1000)
    //        {
    //            talkText.text = weaponTradeTalkData;
    //        }


    //        if (weaponTradeTalkData == null && id == 1000)
    //        {
    //            isAction = false;
    //            talkIndex = 0;
    //            return;
    //        }

    //    }
    //    else if(id == 3000)
    //    {
    //        int posionTradeTalkIndex = getPosionTradeTalkIndex;
    //        string posionTradeTalkData = talkManager.GetTalk(id + posionTradeTalkIndex, talkIndex);
    //        isGetAlreadyPosionNum++;

    //        if (posionTradeTalkData == null && id == 3000)
    //        {
    //            isAction = false;
    //            talkIndex = 0;
    //            return;
    //        }

    //        else if (isPosionTradeNpc && id == 3000)
    //        {
    //            talkText.text = posionTradeTalkData;
    //        }


    //    }


    // 캐릭터의 각 대화가 끝났을 때

    // 대화 분기점(npc, 아이템 마다 설정 가능)
    //else
    //{
    //    talkText.text = questTalkData;
    //}


    //    isAction = true;
    //    talkIndex++;

    //    Debug.Log(isGetAlreadyPosionNum);
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        activeInventory = !activeInventory;
    //        inventoryPanel.SetActive(activeInventory);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Escape) && !activeHelpMenu)        // helpmenu 꺼져있을 때만(예외처리)
    //        OnOffSubMenuPanel();

    //    if (activeHelpMenu)     // 예외처리
    //    {
    //        if(Input.GetKeyDown(KeyCode.Escape))
    //            OnOffHelpMenuPanel();
    //    }


    //    if(questManager_lagacy.locateAtQuestAreaNum_Quest1 == 2)   // 퀘스트 1 처리
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

    //    if (questManager_lagacy.getItemNum_Quest2 == 0)        // 퀘스트2 처리
    //    {
    //        Invoke("setActiveQuestClearText", 1f);
    //        questManager_lagacy.NextQuest();
    //        questManager_lagacy.required_ItemGroup_Quest2.SetActive(false);
    //        questManager_lagacy.Direction_Up.SetActive(true);
    //        questManager_lagacy.Direction_Right.SetActive(false);

    //        questManager_lagacy.getItemNum_Quest2 += 100;      // 조건문 한 번만 호출
    //    }

    //    if(questManager_lagacy.eliminateHenchmanNum_Quest3 == 5)      // 퀘스트3 처리
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

    //        questManager_lagacy.eliminateHenchmanNum_Quest3++;        // 조건문 한 번만 호출하기 위함
    //    }

    //    // 퀘스트 4 처리
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

    //    if (questManager_lagacy.eliminateHenchmanNum_Quest4 == 6 && questManager_lagacy.eliminateBossNum_Quest4 == 1)      // 퀘스트 4 처리
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


    //        questManager_lagacy.eliminateBossNum_Quest4++;        // 조건문 한 번만 호출하기 위함
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


    public void setActiveQuestClearText()       // 각 퀘스트 조건 달성 및 퀘스트 클리어 시 나오는 텍스트, 각 questcondition에서 등록하여 최신화할 듯.
    {
            Instantiate(questClearText, questClearText.transform.position, Quaternion.identity, GameObject.Find("MainUICanvas").transform);
    }

    public void SpawnPlayerOnWoods()
    {
        StartCoroutine(coSpawnPlayerOnWoods());

    }

    IEnumerator coSpawnPlayerOnWoods()
    {
        _playerInteract.isPlayerInteracting = true;     // 이동 제한
        ScreenFadeOut();                                // 화면 페이드아웃
        yield return new WaitForSeconds(1.5f);

        LocatePlayerOnWoods();                           // 플레이어 위치 지정
        MainCamera.transform.position = player.transform.position;
        MainCamera.GetComponent<CameraController>().center = new Vector2(62.5f, 9);
        MainCamera.GetComponent<CameraController>().size = new Vector2(54, 28);
        yield return new WaitForSeconds(0.2f);

        ScreenFadeIn();                                 // 화면 페이드인
        yield return new WaitForSeconds(0.5f);

        SetTrueSpawnAreasAnimState();                  // 문 애니메이션 초기화
        _playerInteract.isPlayerInteracting = false;    // 이동 제한 해제
        yield return new WaitForSeconds(0.8f);

        fadeImage.gameObject.SetActive(false);
    }

    public void LocatePlayerOnWoods()    // 캠프 > 숲 플레이어 이동 처리
    {
        playerLight.falloffIntensity = 0.9f;

        int randomCount = Random.Range(0, 3);

        switch (randomCount)
        {
            case 0:
                player.transform.position = woods_spawn_areas[0].transform.position; break;
            case 1:
                player.transform.position = woods_spawn_areas[1].transform.position; break;
            case 2:
                player.transform.position = woods_spawn_areas[2].transform.position; break;
        }
    }

    public void SpawnPlayerOnCamp()
    {
        StartCoroutine(coSpawnPlayerOnCamp());
    }

    IEnumerator coSpawnPlayerOnCamp()
    {
        _playerInteract.isPlayerInteracting = true;     // 이동 제한
        ScreenFadeOut();                                // 화면 페이드아웃

        yield return new WaitForSeconds(1.5f);

        LocatePlayerOnCamp();                           // 플레이어 위치 지정
        ScreenFadeIn();                                 // 화면 페이드인

        yield return new WaitForSeconds(0.5f);
        SetFalseSpawnAreasAnimState();                  // 문 애니메이션 초기화
        _playerInteract.isPlayerInteracting = false;    // 이동 제한 해제

        yield return new WaitForSeconds(0.8f);
        fadeImage.gameObject.SetActive(false);
    }

    public void LocatePlayerOnCamp()     // 숲 > 캠프 플레이어 이동 처리
    {
        player.transform.position = CampSpawnArea.transform.position;
        playerLight.falloffIntensity = 0.6f;
    }

    private void ScreenFadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 1.3f);
    }

    private void ScreenFadeIn()
    {
        fadeImage.DOFade(0, 1.3f);
    }

    private void SetTrueSpawnAreasAnimState()       // 플레이어가 이동한 문 애니메이션 동작
    {
        foreach (Animator spawnArea in woods_spawn_areas)
        {
            spawnArea.SetBool("isPlayerInWoods", true);
        }
    }
    private void SetFalseSpawnAreasAnimState()      // 문 애니메이션 초기화
    {
        foreach (Animator spawnArea in woods_spawn_areas)
        {
            spawnArea.SetBool("isPlayerInWoods", false);
        }
    }

    public void CloseEnterWoodsUIPanelandResume()
    {
        enterWoodsUI.SetActive(false);
        isUIInteract = false;
    }

    public void CloseEnterCampUIPanelandResume()
    {
        enterCampUI.SetActive(false);
        isUIInteract = false;
    }

    public void CloseEnterFightUIAndResume()
    {
        enterFightUI.SetActive(false);
        isUIInteract = false;
    }

    public void ResumeGame()        // 게임 일시정지 해제
    {
        isActiveSubMenu = false;
        subMenuUI.SetActive(false);
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void LoadIntroScene()
    {
        SceneManager.LoadScene("Intro");
    }

    public void LoadCampScene()
    {
        SceneManager.LoadScene("Camp");
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnOffSubMenu();

        if (Input.GetKeyDown(KeyCode.I))
        {
            // 인벤토리
        }
    }

    public void OnOffSubMenu()
    {
        isActiveSubMenu = !isActiveSubMenu;
        subMenuUI.SetActive(isActiveSubMenu);
        Time.timeScale = isActiveSubMenu ? 0 : 1;
    }

    public void OnOffHelpMenu()
    {
        isActiveHelpMenu = !isActiveHelpMenu;
        helpMenuUI.SetActive(isActiveHelpMenu);
        Time.timeScale = isActiveHelpMenu ? 0 : 1;
    }

    public void OpenSoundControlPanel()
    {
        SoundManager.instance.OpenSoundControlPanel();
    }

    public void ClosdSoundControlPanel()
    {
        SoundManager.instance.CloseSoundControllPanel();
    }

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
        player.transform.position = enterFightPos.position;
        //SoundManager.instance.EnterFight();
        isEnterFight = true;
    }

}
