using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 플레이어가 획득한 능력에 대한 정보를 보유하고, 이를 UI로 출력하는 클래스.
public class AbilityListManager : MonoBehaviour
{
    // 현재 플레이어 보유 능력 리스트
    List<Ability> playerAbilityList = new List<Ability>();

    // 출력 UI 양식 프리팹
    [SerializeField]
    GameObject abilityUIPref;

    // 임시 저장 변수들
    int abilityIndex;
    int id;
    int value;

    void Start()
    {
        ALOn();
    }

    void Update()
    {
        
    }

    // 현재 보유 리스트에 선택된 능력을 추가하는 함수.
    // 능력 선택 이벤트에서 버튼 클릭으로 인한 OnClick() 이벤트 발생 시 실행되는 이벤트 핸들러.
    public void GetSelected(GameObject _selected)
    {
        Debug.Log(_selected.name + "전달");

        Ability ability = new Ability();


        ability.ability_name = _selected.transform.GetChild(0).GetComponent<Text>().text;
        ability.description = _selected.transform.GetChild(1).GetComponent<Text>().text;
        ability.option = _selected.transform.GetChild(2).GetComponent<Text>().text;
        ability.plus_Value_Done = _selected.transform.GetChild(3).GetComponent<Text>().text;
        ability.isSelected = true;

        playerAbilityList.Add(ability);

        // 레벨 관련 시스템은 추후에 적용
        // ability.level
    }

    // 보유 능력 UI 활성화
    void ALOn()
    {
        for (int i = 0; i < playerAbilityList.Count; i++)
        {
            GameObject obj = Instantiate(abilityUIPref);
            obj.transform.SetParent(this.gameObject.transform);
            obj.transform.GetChild(0).GetComponent<Text>().text = playerAbilityList[i].ability_name;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerAbilityList[i].description;
            obj.transform.GetChild(2).GetComponent<Text>().text = playerAbilityList[i].option;
            obj.transform.GetChild(3).GetComponent<Text>().text = playerAbilityList[i].plus_Value_Done;
        }
    }
}
