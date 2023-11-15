using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChoice : MonoBehaviour
{
    // 능력의 타입 인덱스. 보상 정령에 따라 다름.
    // 0 : 물
    [SerializeField]
    int typeIndex;

    // 적용되는 능력.
    // 0 : 약공격, 1 : 강공격, 2 : 장판 & 돌진공격 , 3 : 이동속도
    [SerializeField]
    int abilityID;

    // 가산 수치.
    [SerializeField]
    int plusValue;

    int[] tmpArray;

    [SerializeField]
    Player player;

    [SerializeField]
    AbilityListManager aLManager;

    // 해당 오브젝트에 존재하는 버튼 컴포넌트 변수
    Button button;

    private void Start()
    {
        // 활성화 시, 오브젝트에 부착된 버튼 컴포넌트 저장.
        button = this.gameObject.GetComponent<Button>();
        Debug.Log(button.name);
        // 버튼 onClick() 이벤트에 리스너 추가. 클릭 시 해당 능력 정보를 전달.
        button.onClick.AddListener(() => aLManager.GetSelected(this.gameObject));
    }

    public void SetChoiceValue(int[] indexArray)
    {
        tmpArray = new int[3] { indexArray[0], indexArray[1], indexArray[2] };
        typeIndex = tmpArray[0];
        abilityID = tmpArray[1];
        plusValue = tmpArray[2];


    }

    public void ChoiceClicked()
    {
        player.AbilityOnStat(tmpArray);
    }


}
