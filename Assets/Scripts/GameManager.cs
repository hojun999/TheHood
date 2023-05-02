using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;

    [Header("UI")]
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;
    public GameObject inventoryPanel;
    public GameObject moveWoodsUIPanel;
    public GameObject moveCampUIPanel;

    [Header("Manager")]
    public TalkManager talkManager;
    public QuestManager questManager;

    [Header("Camera")]
    public GameObject MainCamera;

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


    public bool isAction;
    public bool activeInventory = false;
    

    public void talkAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        // 대화 데이터 세팅
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        // 캐릭터의 각 대화가 끝났을 때
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questManager.checkQuest(2000);
            return;         // -void 함수에서 return은 함수의 종료를 의미-
        }

        if (isNpc)          // 대화 분기점(npc, 아이템 마다 설정 가능)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

        

        isAction = true;
        talkIndex++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }


    public void LocatePlayerAtCamp()      // 캠프로 이동
    {
        Player.transform.position = CampSpawnArea.transform.position;
        MainCamera.GetComponent<CameraController>().center = new Vector2(0, 0);
        MainCamera.GetComponent<CameraController>().size = new Vector2(18, 10);
        Player.transform.position = CampSpawnArea.transform.position;
        Player.GetComponent<PlayerController>().isPlayerInWoods = false;
        Player.GetComponent<SpriteRenderer>().material = litMaterial;

        moveCampUIPanel.SetActive(false);

        WestSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);
        EastSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);
        NorthSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", false);

        Time.timeScale = 1f;
    }

    public void LocatePlayerAtWoods()     // 숲으로 이동, 이후에 북쪽, 동쪽 스폰 랜덤하게 구현하기
    {
        Player.transform.position = WestSpawnArea.transform.position;
        MainCamera.GetComponent<CameraController>().center = new Vector2(43, 9);
        MainCamera.GetComponent<CameraController>().size = new Vector2(54, 28);
        moveWoodsUIPanel.SetActive(false);
        Player.GetComponent<PlayerController>().isPlayerInWoods = true;
        Player.GetComponent<SpriteRenderer>().material = unlitMaterial;

        WestSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);
        EastSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);
        NorthSpawnArea.GetComponent<Animator>().SetBool("isPlayerInWoods", true);

        Time.timeScale = 1f;
    }

    public void CloseMoveWoodsUIPanelandResume()
    {
        moveWoodsUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CloseMoveCampUIPanelandResume()
    {
        moveCampUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeGame()        // 게임 일시정지 해제
    {
        Time.timeScale = 1f;
    }

    public void LoadCamp()
    {
        SceneManager.LoadScene("Camp");
    }

    public void TurnOffGame()
    {
        Application.Quit();
    }


}
