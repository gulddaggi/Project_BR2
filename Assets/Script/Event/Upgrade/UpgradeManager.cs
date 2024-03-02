using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    GameObject UpgradeContent;

    int totalUpgradeContentCount = 0;

    [SerializeField]
    Scrollbar scrollbar;

    private void OnEnable()
    {
        // 스크롤 위치 초기화
        scrollbar.value = 1f;

        // UI 세팅
        if (this.transform.childCount == 0)
        {
            UISetting();
        }
    }

    // 업그레이드 UI 세팅.
    void UISetting()
    {
        totalUpgradeContentCount = EventDBManager.instance.GetUpgradeDicCount();

        // 업그레이드 오브젝트 생성 후 텍스트 세팅
        for (int i = 0; i < totalUpgradeContentCount; i++)
        {
            GameObject obj = Instantiate(UpgradeContent);
            obj.transform.SetParent(this.gameObject.transform);

            UpgradeItem curItem = EventDBManager.instance.GetUpgradeItem(i);
            obj.transform.GetChild(0).GetComponent<Text>().text = curItem.name;
            obj.transform.GetChild(1).GetComponent<Text>().text = curItem.description;
            obj.transform.GetChild(2).GetComponent<Text>().text = "Lv." + curItem.level.ToString();
            
            if (curItem.level == 0)
            {
                obj.transform.GetChild(3).GetComponent<Text>().text = "-";
            }
            else
            {
                obj.transform.GetChild(3).GetComponent<Text>().text = "+ " + curItem.value[curItem.level-1];
            }

            if (curItem.level == curItem.value.Length)
            {
                obj.transform.GetChild(4).GetComponent<Text>().text = "최대";
            }
            else
            {
                obj.transform.GetChild(4).GetComponent<Text>().text = curItem.price[curItem.level];
            }
        }
    }
}
