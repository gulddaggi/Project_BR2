using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 리스트 저장을 위한 능력 클래스. DB 클래스에서 사용하는 능력 클래스와 변수가 다소 다름.
public class Ability_ListComponent
{
    // 능력명
    public string ability_name;

    // 능력 설명
    public string description;

    // 능력치 적용 옵션
    public string option;

    // 부가 설명. 부여되는 상태이상 효과 설명
    public string sub_Description;

    // 선행 능력
    public string[] pre_abilities;

    // 레벨
    public int level;

    public float plus_Value;

    public string rank;

    // 능력 인덱스. 0 : 능력 종류, 1 : 해당 능력의 DB 상 ID
    public int[] indexArr = new int[2];
};

// 현재 플레이어가 획득한 능력에 대한 정보를 보유하고, 이를 UI로 출력하는 클래스.
public class AbilityListManager : MonoBehaviour
{
    // 현재 플레이어 보유 능력 리스트
    List<Ability_ListComponent> playerAbilityList = new List<Ability_ListComponent>();

    // 출력 UI 양식 프리팹
    [SerializeField]
    GameObject abilityUIPref;

    // 리스트 선언을 위한 변수
    public int totalAbilityNum = 0;
    public int totalIDNum;

    // 능력 레벨 확인을 위한 리스트
    List<List<int>> levelCheckList = null;

    // 임시 저장 변수들
    int abilityIndex;
    int id;
    int value;

    // 플레이어 상태 클래스 변수
    [SerializeField]
    Player player;

    // 선택된 능력 처리 클래스 변수
    [SerializeField]
    SelectedAbilityProcessor selectedAbilityProcessor;

    // 능력 레벨 확인을 위한 리스트
    void CreateArray()
    {
        Debug.Log("새 리스트 생성");
        levelCheckList = new List<List<int>>();
        totalAbilityNum = EventDBManager.instance.GetTotalDicNum();

        for (int i = 0; i < totalAbilityNum; i++)
        {
            totalIDNum = EventDBManager.instance.GetTotalIDNum(i);
            List<int> tmpList = new List<int>();

            for (int j = 0; j < totalIDNum; j++)
            {
                tmpList.Add(0);
            }
            levelCheckList.Add(tmpList);
        }

        /*Debug.Log("리스트 구성 완료. 능력 종류 개수 : " + levelCheckList.Count);
        for (int i = 0; i < levelCheckList.Count; i++)
        {
            Debug.Log((i + 1) + "번째 능력의 개수 : " + levelCheckList[i].Count);
        }*/
    }
    private void OnEnable()
    {
        ALOn();
    }

    private void OnDisable()
    {
        ALOff();
    }

    // 현재 보유 리스트에 선택된 능력을 추가하는 함수.
    // 능력 선택 이벤트에서 버튼 클릭으로 인한 OnClick() 이벤트 발생 시 실행되는 이벤트 핸들러.
    public void GetSelected(GameObject _selected)
    {
        // 리스트 초기화 진행
        if (levelCheckList == null)
        {
            CreateArray();
        }

        // 레벨 지정에 필요한 인덱스 저장
        abilityIndex = _selected.GetComponent<AbilityChoice>().typeIndex;
        id = _selected.GetComponent<AbilityChoice>().abilityID;
        value = _selected.GetComponent<AbilityChoice>().plusValue;

        // 리스트 추가를 위한 능력 데이터 생성
        Ability_ListComponent ability = new Ability_ListComponent();
        ability.ability_name = _selected.transform.GetChild(0).GetComponent<Text>().text;
        ability.description = _selected.transform.GetChild(1).GetComponent<Text>().text;
        ability.option = _selected.transform.GetChild(2).GetComponent<Text>().text;
        ability.plus_Value = value;
        ability.rank = _selected.transform.GetChild(4).GetComponent<Text>().text;
        ability.level = 1;

        ability.indexArr[0] = abilityIndex;
        ability.indexArr[1] = id;

        // 레벨 지정
        // 능력이 처음 선택되었을 경우
        if (levelCheckList[abilityIndex][id] == 0)
        {
            // 레벨 증가
            ability.level = ++levelCheckList[abilityIndex][id];
            
            // 현재 선택 리스트에 삽입
            playerAbilityList.Add(ability);
            Debug.Log("새로운 능력 삽입 완료");
        }
        else // 이미 선택되었을 경우. 랭크간 비교가 필요
        {
            if (playerAbilityList.Exists(x => x.ability_name == ability.ability_name))
            {
                int index = playerAbilityList.FindIndex(x => x.ability_name == ability.ability_name);
                
                // 기존 랭크가 선택된 랭크보다 작을 경우. 데이터 교체
                if (RankToInt(playerAbilityList[index].rank) < RankToInt(ability.rank))
                {
                    playerAbilityList[index].rank = ability.rank;
                    playerAbilityList[index].plus_Value = ability.plus_Value;
                    playerAbilityList[index].level = ability.level;
                    
                }
                // 같은 랭크일 경우. 레벨 가산
                else if(RankToInt(playerAbilityList[index].rank) == RankToInt(ability.rank))
                {
                    ++playerAbilityList[index].level;
                    playerAbilityList[index].plus_Value = (float)(value + (2f*playerAbilityList[index].level));
                }
            }
        }

        // 선택된 능력 적용
        Debug.Log(ability.indexArr[0] + ", " + ability.indexArr[1] + " 의 능력 선택 전달");
        selectedAbilityProcessor.AbilitySelected(ability);
    }

    int RankToInt(string rank)
    {
        string[] ranks = { "일반", "희귀", "영웅" };
        int index = 0;

        for (int i = 0; i < ranks.Length; i++)
        {
            if (rank == ranks[i])
            {
                index = i;
            }
        }

        return index;
    }

    // 보유 능력 UI 활성화
    void ALOn()
    {
        for (int i = 0; i < playerAbilityList.Count; i++)
        {
            Debug.Log(playerAbilityList[i].ability_name);
            GameObject obj = Instantiate(abilityUIPref);
            obj.transform.SetParent(this.gameObject.transform);
            obj.transform.GetChild(0).GetComponent<Text>().text = playerAbilityList[i].ability_name;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerAbilityList[i].description;
            obj.transform.GetChild(2).GetComponent<Text>().text = playerAbilityList[i].option;
            obj.transform.GetChild(3).GetComponent<Text>().text = "+" + playerAbilityList[i].plus_Value + "%";
        }
    }

    public void ALOff()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject.DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
        Transform _parent = transform.parent;
        _parent.parent.gameObject.SetActive(false);
    }

    public bool AbilityCheck(int _typeIndex, int _id)
    {
        // 리스트 초기화 진행
        if (levelCheckList == null)
        {
            CreateArray();
        }

        // 능력 이미 선택
        if (levelCheckList[_typeIndex][_id] != 0)
        {
            return true;
        }
        // 능력 미선택
        else
        {
            return false;
        }
    }

    // 기존 선택 능력의 등급 확인
    public int AbilityRankCheck(int _typeIndex, int _id)
    {
        // 이미 선택된 능력일 경우
        if (levelCheckList[_typeIndex][_id] != 0)
        {
            // 해당 능력을 찾아 등급 확인.
            for (int i = 0; i < playerAbilityList.Count; i++)
            {
                int[] tmpArr = playerAbilityList[i].indexArr;

                if (tmpArr[0] == _typeIndex && tmpArr[1] == _id)
                {
                    Debug.Log("능력 : " + playerAbilityList[i].ability_name);
                    Debug.Log("이미 선택된 능력. 등급은 : " + playerAbilityList[i].rank);

                    // 기존 선택 능력 등급을 정수로 변환
                    int curRank = RankToInt(playerAbilityList[i].rank);
                    return curRank;
                }
            }
        }
        return -1;
    }
}
