using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }

    void Update()
    {
        
    }

    // 현재 보유 리스트에 선택된 능력을 추가하는 함수.
    // 능력 선택 이벤트에서 버튼 클릭으로 인한 OnClick() 이벤트 발생 시 실행되는 이벤트 핸들러.
    private void AddAbility() // 순서대로 능력 인덱스, 능력 DB 상 id, 가산 수치(후에 레벨로 변경가능)
    {
        Ability ability = new Ability();
        
    }

    public void GetSelected(GameObject _selected)
    {
        Debug.Log(_selected.name + "전달");
    }

    public void GetAbilityIndex(int _abilityIndex)
    {
        Debug.Log("능력 선택.");
        Debug.Log("능력 인덱스 : " + _abilityIndex);
        abilityIndex = _abilityIndex;
    }

    public void GetId(int _id)
    {
        Debug.Log("능력 ID : " + _id);
        id = _id;
    }

    public void GetValue(int _value)
    {
        Debug.Log("가산 수치 : " + _value);
        value = _value;

    }
}
