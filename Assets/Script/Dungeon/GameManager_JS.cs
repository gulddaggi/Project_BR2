using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 해당 보상 NPC와의 대화 출력 여부 확인 클래스
public class DialogueCheck
{
    // 만난 횟수
    int count = 0;
    // 이번 회차에서 만났는가
    bool isEncounter = false;

    public int Count
    {
        get { return count; }
        set { count = value; }
    }

    public bool IsEncounter
    {
        get { return isEncounter; }
        set { isEncounter = value; }
    }
}

public class GameManager_JS : MonoBehaviour
{
    // 싱글톤 구현
    private static GameManager_JS instance = null;

    [SerializeField]
    private int dungeonCount = 0;

    [SerializeField]
    private int tryCount = 0;

    [SerializeField]
    private int coin = 100; // 테스트를 위해 100 기본 제공

    GameObject canvas;

    [SerializeField]
    Image panelImage;

    [SerializeField]
    Text coinText_Play;

    [SerializeField]
    Text coinText_Event;

    GameObject curStage;
    GameObject nextStage;

    //플레이어 Transform 전달을 위한 임시 변수
    Transform tmpPlayerPos;

    //보상 정보 전달을 위한 임시 변수
    GameObject tmpReward;

    [SerializeField]
    //스테이지 객체를 저장하는 큐
    private Queue<GameObject> stageQueue = new Queue<GameObject>();

    //출구를 통한 스테이지 이동 가능 여부.
    private bool isMoveOn = false;

    public DialogueCheck[] dialogueChecks = new DialogueCheck[6];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            for (int i = 0; i < dialogueChecks.Length; i++)
            {
                dialogueChecks[i] = new DialogueCheck();
            }

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
        //디버깅용 출력
        Debug.Log("Start Dungeon count : " + dungeonCount);
        CoinOnOff(true);

    }

    // fadeout에 사용되는 panel 반환
    public Image GetPanel()
    {
        if (panelImage == null)
        {
            canvas = GameObject.Find("Canvas");
            panelImage = canvas.GetComponentInChildren<Image>();
        }

        return panelImage;
    }

    // stageQueue에 생성된 스테이지 삽입.
    public void StageInput(GameObject stage)
    {
        stageQueue.Enqueue(stage);
    }

    public void InitStage()
    {
        StartCoroutine(FadeInPanel());
        stageQueue.Clear();
        dungeonCount = 0;
        SceneManager.LoadScene("HomeScene");
        Coin = 0;
        coinText_Play.transform.gameObject.SetActive(false);
        isMoveOn = true;
        ResetEncounter();
    }

    void ResetEncounter()
    {
        for (int i = 0; i < dialogueChecks.Length; i++)
        {
            dialogueChecks[i].IsEncounter = false;
        }
    }

    // 다음 스테이지 활성화
    public void NextStage(Transform transform)
    {
        // 마을에서 던전 입장 or 하나의 던전 클리어 시 던전 씬으로 전환하여 던전 스테이지 생성
        if (dungeonCount == 0)
        {
            //마을에서 던전 입장 시 tryCount 증가. 추후 제일 처음 입장에 대한 시도 수 증가도 고려해야함.
            if (SceneManager.GetActiveScene().name == "HomeScene") ++tryCount;

            //SceneManager.LoadScene("DungeonScene");
            // 테스트용
            SceneManager.LoadScene("DungeonScene_JSTest");
        }
        // 던전 씬 내에서 스테이지 이동 시 스테이지 변경 수행
        else
        {
            curStage = transform.parent.gameObject;
            GameObject tmpReward = transform.GetComponent<Exit>().GetRewardObj();
            tmpPlayerPos = curStage.GetComponent<Stage>().GetPlayerPos();

            // 현재 스테이지 비활성화
            curStage.SetActive(false);
            //curStage = null;

            /*if (stageQueue.Count == 0)
            {
                InitStage();
                SceneManager.LoadScene("HomeScene");
                return;
            }*/

            // 다음 스테이지 큐로부터 전달 후 활성화.
            nextStage = stageQueue.Dequeue();
            nextStage.SetActive(true);
            // 플레이어를 스테이지 시작 위치로 이동
            nextStage.GetComponent<Stage>().PlayerPosToStart(tmpPlayerPos);
            // 출구에 표시된 보상을 스테이지 보상으로 지정
            nextStage.GetComponent<Dungeon>().SetReward(tmpReward);
            curStage = null;
            curStage = nextStage;
        }

        // 출구를 통한 스테이지 이동 불가로 설정
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

    public void PanelOff()
    {
        panelImage.gameObject.SetActive(false);
    }

    public void PanelOn()
    {
        panelImage.gameObject.SetActive(true);
    }

    public int GetTryCount()
    {
        return tryCount;
    }

    public int Coin
    {
        get { return coin; }
        set 
        { 
            coin += value;
            CoinUpdate();
        }
    }

    public void CoinUpdate()
    {
        coinText_Play.text = "Coin : " + coin;
        coinText_Event.text = "Coin : " + coin;
    }

    public void CoinOnOff(bool _bool)
    {
        if (SceneManager.GetActiveScene().name != "HomeScene")
        {
            if (coinText_Play == null)
            {
                coinText_Play = canvas.GetComponentInChildren<Text>();
            }
            CoinUpdate();
            //coinText_Play.gameObject.SetActive(_bool);

        }
    }

    private IEnumerator FadeInPanel()
    {
        Color startColor = panelImage.color;
        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            float currentAlpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / duration);

            // 현재 알파 값을 새로운 색상으로 설정
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            panelImage.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        panelImage.color = color;
        //GameManager_JS.Instance.CoinOnOff(true); // 코인 텍스트 on/off 기능. 이후에 활성화.
    }
}
