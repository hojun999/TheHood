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
    PlayerAttack _playerAttack;


    private InventoryManager _inventoryManager;
    private PlayerInteract _playerInteract;

    [Header("Player")]
    public GameObject player;
    public GameObject magicSquareHolder;
    public Light2D playerLight;

    //public Camera FightCamera;

    [Header("UI")]
    public GameObject inventoryUI;          //인벤토리 UI
    public GameObject enterWoodsUI;         // 숲 진입 UI
    public GameObject enterCampUI;          // 캠프 진입 UI
    public GameObject subMenuUI;            // 옵션 메뉴 UI
    public GameObject helpMenuUI;           // 도움말 UI
    public GameObject questClearText;
    public GameObject enterFightUI;         // 전투 시작 UI (퀘스트3)
    public GameObject playerDieUI;
    public Image fadeImage;                 // 페이드 인/아웃 이미지

    [Header("Spawn")]   // 스폰 시스템
    public Animator[] woods_spawn_areas;
    public Transform CampSpawnArea;

    [Header("Quest4")]
    public EnemyBoss enemy_boss;
    public EnemyNormal[] enemys_normal;

    private Camera mainCamera;
    [HideInInspector] public bool activeInventory = false;
    [HideInInspector] public bool isUIInteract;
    private bool isActiveSubMenu;
    private bool isActiveHelpMenu;
    private bool isActiveInventory;
    [HideInInspector] public bool isEnding;
    private void Start()
    {
        SetOnStart();
    }

    private void SetOnStart()
    {
        isActiveHelpMenu = true;        // 하이어라키에서 도움말 액티브된 상태로 시작하기

        _inventoryManager = gameObject.GetComponent<InventoryManager>();
        _playerInteract = player.GetComponent<PlayerInteract>();
        _playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        HandleUIInput();
    }

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
        _playerAttack.isPlayerInWoods = true;
        _playerInteract.isPlayerInteracting = true;     // 이동 제한
        isUIInteract = false;
        ScreenFadeOut();                                // 화면 페이드아웃
        yield return new WaitForSeconds(1.5f);

        LocatePlayerOnWoods();                           // 플레이어 위치 지정
        mainCamera.transform.position = player.transform.position;
        mainCamera.GetComponent<CameraController>().center = new Vector2(62.5f, 9);
        mainCamera.GetComponent<CameraController>().size = new Vector2(54, 28);
        yield return new WaitForSeconds(0.2f);

        ScreenFadeIn();                                 // 화면 페이드인
        SoundManager.instance.EnterWoods();
        yield return new WaitForSeconds(0.5f);

        SetTrueSpawnAreasAnimState();                   // 문 애니메이션 초기화
        _playerInteract.isPlayerInteracting = false;    // 이동 제한 해제
        _playerAttack.enabled = true;                   // 공격 가능
        magicSquareHolder.SetActive(true);
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
        _playerAttack.isPlayerInWoods = false;
        _playerInteract.isPlayerInteracting = true;     // 이동 제한
        isUIInteract = false;
        ScreenFadeOut();                                // 화면 페이드아웃
        yield return new WaitForSeconds(1.5f);

        LocatePlayerOnCamp();                           // 플레이어 위치 지정

        mainCamera.transform.position = player.transform.position;
        mainCamera.GetComponent<CameraController>().center = new Vector2(0, 0);
        mainCamera.GetComponent<CameraController>().size = new Vector2(18, 10);
        yield return new WaitForSeconds(0.2f);

        ScreenFadeIn();                                 // 화면 페이드인
        SoundManager.instance.EnterCamp();
        yield return new WaitForSeconds(0.5f);

        SetFalseSpawnAreasAnimState();                  // 문 애니메이션 초기화
        _playerInteract.isPlayerInteracting = false;    // 이동 제한 해제
        _playerAttack.enabled = false;
        magicSquareHolder.SetActive(false);
        yield return new WaitForSeconds(0.8f);

        fadeImage.gameObject.SetActive(false);
    }

    public void LocatePlayerOnCamp()     // 숲 > 캠프 플레이어 이동 처리
    {
        player.transform.position = CampSpawnArea.transform.position;
        playerLight.falloffIntensity = 0.6f;
    }

    public void SetPlayerInteractFalseOnCloseSpawnUI()
    {
        _playerInteract.isPlayerInteracting = false;
        isUIInteract = false;
        Time.timeScale = 1;
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
        _playerAttack.enabled = true;
        isActiveSubMenu = false;
        subMenuUI.SetActive(false);
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        SoundManager.instance.BgSoundPlay(SoundManager.instance.bglist[2]);
        Time.timeScale = 1;
    }
    public void LoadIntroScene()
    {
        SceneManager.LoadScene("Intro");
        SoundManager.instance.BgSoundPlay(SoundManager.instance.bglist[4]);
    }

    public void LoadCampScene()
    {
        SceneManager.LoadScene("Camp");
        SoundManager.instance.BgSoundPlay(SoundManager.instance.bglist[0]);
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
            OnOffInventory();
    }

    public void OnOffSubMenu()
    {
        isActiveSubMenu = !isActiveSubMenu;
        subMenuUI.SetActive(isActiveSubMenu);
        _playerAttack.enabled = isActiveSubMenu ? false : true;
        Time.timeScale = isActiveSubMenu ? 0 : 1;
    }

    public void OnOffHelpMenu()
    {
        isActiveHelpMenu = !isActiveHelpMenu;
        helpMenuUI.SetActive(isActiveHelpMenu);
        _playerAttack.enabled = isActiveHelpMenu ? false : true;
        Time.timeScale = isActiveHelpMenu ? 0 : 1;
    }

    public void OnOffInventory()
    {
        isActiveInventory = !isActiveInventory;
        inventoryUI.SetActive(isActiveInventory);
        _playerAttack.enabled = isActiveInventory ? false : true;
        Time.timeScale = isActiveInventory ? 0 : 1;
    }

    public void OpenSoundControlPanel()
    {
        SoundManager.instance.OpenSoundControlPanel();
    }

    public void ClosdSoundControlPanel()
    {
        SoundManager.instance.CloseSoundControllPanel();
    }

    public void EnterFightSetting_Quest4()
    {
        StartCoroutine(coEnterFightSetting());
    }

    IEnumerator coEnterFightSetting()
    {
        Time.timeScale = 1;

        _playerInteract.isPlayerInteracting = true;     // 이동 제한
        isUIInteract = false;
        ScreenFadeOut();                                // 화면 페이드아웃
        yield return new WaitForSeconds(1.5f);

        mainCamera.GetComponent<CameraController>().enabled = false;
        mainCamera.transform.position = new Vector3(46.5f, 11.5f, -22);
        player.transform.position = enterFightPos.position;     // 전투 지역으로 플레이어 이동
        yield return new WaitForSeconds(0.2f);

        ScreenFadeIn();                                 // 화면 페이드인
        SoundManager.instance.EnterFight();
        yield return new WaitForSeconds(1f);
        fadeImage.gameObject.SetActive(false);

        _playerInteract.isPlayerInteracting = false;     // 이동 제한 해제

        enemy_boss.enabled = true;                      // enemy 전투 상태로 변경
        enemy_boss.startFight = true;
        foreach (EnemyNormal enemy in enemys_normal)
        {
            enemy.enabled = true;
            enemy.startFight = true;
        }

        isUIInteract = false;
    }

    public void OpenDieUI()
    {
        playerDieUI.SetActive(true);
        Time.timeScale = 0;
    }


    public void EndingSequence()
    {
        StartCoroutine(coEndingSequence());
    }

    IEnumerator coEndingSequence()
    {
        isEnding = true;

        _playerInteract.isPlayerInteracting = true;
        yield return new WaitForSeconds(1.5f);

        ScreenFadeOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Ending");
    }
}
