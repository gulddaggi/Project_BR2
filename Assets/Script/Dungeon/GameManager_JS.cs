using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager_JS : MonoBehaviour
{
    // 싱글톤 구현
    private static GameManager_JS instance = null;

    [SerializeField]
    private int dungeonCount = 0;

    GameObject canvas;

    [SerializeField]
    Image panelImage;

    GameObject curStage;

    //플레이어 Transform 전달을 위한 임시 변수
    Transform tmpPlayerPos;

    //보상 정보 전달을 위한 임시 변수
    GameObject tmpReward;

    [SerializeField]
    //스테이지 객체를 저장하는 큐
    private Queue<GameObject> stageQueue = new Queue<GameObject>();

    //스테이지 이동 가능 여부
    private bool isMoveOn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager_JS Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        Debug.Log("Start Dungeon count : " + dungeonCount);
    }

    public Image GetPanel()
    {
        if (panelImage == null)
        {
            canvas = GameObject.Find("Canvas");
            panelImage = canvas.GetComponentInChildren<Image>();
        }

        return panelImage;
    }

    public void StageInput(GameObject stage)
    {
        stageQueue.Enqueue(stage);
    }

    public void InitStage()
    {
        stageQueue.Clear();
        dungeonCount = 0;
    }

    // 다음 스테이지 활성화
    public void NextStage(Transform transform)
    {
        if (dungeonCount == 0)
        {
            SceneManager.LoadScene("DungeonScene");
        }
        else
        {
            curStage = transform.parent.gameObject;
            tmpPlayerPos = curStage.GetComponent<Stage>().GetPlayerPos();

            curStage.SetActive(false);
            curStage = null;

            if (stageQueue.Count == 0)
            {
                InitStage();
                return;
            }
            else
            {
                curStage = stageQueue.Dequeue();
                curStage.SetActive(true);
                curStage.GetComponent<Stage>().SetPlayerPos(tmpPlayerPos);
            }
        }

        isMoveOn = false;
        ++dungeonCount;
        Debug.Log("Dungeon count : " + dungeonCount);
    }

    public int GetDungeonCount()
    {
        return dungeonCount;
    }

    public void SetIsMoveOn(bool isClear)
    {
        if (isClear)
        {
            isMoveOn = true;
        }
    }

    public bool GetIsMoveOn()
    {
        return isMoveOn;
    }
}
