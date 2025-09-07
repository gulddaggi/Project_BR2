using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    GameObject UpgradeContent;

    int totalUpgradeContentCount = 0;

    bool isSet = false;

    [SerializeField]
    Scrollbar scrollbar;

    [SerializeField]
    Text gemText;

    private void Awake()
    {
        isSet = false;
    }

    private void OnEnable()
    {
        // UI 세팅
        if (!isSet)
        {
            UISetting();
        }
        else
        {
            for (int i = 0; i < totalUpgradeContentCount; i++)
            {
                UIUpdate(i);
            }
        }
        GemTextSetting();
    }

    private void OnDisable()
    {
        // 스크롤 위치 초기화
        scrollbar.value = 1f;
    }

    // 업그레이드 UI 세팅.
    void UISetting()
    {
        totalUpgradeContentCount = EventDBManager.instance.GetUpgradeDicCount();

        // 업그레이드 오브젝트 생성 후 텍스트 세팅
        for (int i = 0; i < totalUpgradeContentCount; i++)
        {
            Transform target = this.transform.GetChild(i);

            UpgradeItem curItem = EventDBManager.instance.GetUpgradeItem(i);

            target.GetChild(0).GetComponent<Text>().text = curItem.name;
            target.GetChild(1).GetComponent<Text>().text = curItem.description;
            target.GetChild(2).GetComponent<Text>().text = "Lv." + curItem.level.ToString();
            
            if (curItem.level == 0)
            {
                target.GetChild(3).GetComponent<Text>().text = "-";
            }
            else
            {
                target.GetChild(3).GetComponent<Text>().text = "+ " + curItem.value[curItem.level-1];
            }

            if (curItem.level == curItem.value.Length)
            {
                target.GetChild(4).GetComponent<Text>().text = "최대";
            }
            else
            {
                target.GetChild(4).GetComponent<Text>().text = curItem.price[curItem.level];
            }
        }
        isSet = true;
    }

    
    // 레벨업
    public void Levelup(int _index)
    {
        int price = int.Parse(this.transform.GetChild(_index).GetChild(4).GetComponent<Text>().text);

        if (GameManager_JS.Instance.Gem >= price)
        {
            GameManager_JS.Instance.Gem = GameManager_JS.Instance.Gem - price;
            EventDBManager.instance.UpgradeItemLevelup(_index);
            UIUpdate(_index);
            GemTextSetting();
        }

    }

    // 레벨업 이후 해당 업그레이드 업데이트
    void UIUpdate(int _index)
    {
        Transform updateTarget = this.transform.GetChild(_index);

        UpgradeItem curItem = EventDBManager.instance.GetUpgradeItem(_index);

        updateTarget.GetChild(2).GetComponent<Text>().text = "Lv." + curItem.level.ToString();
        if (curItem.level == 0)
        {
            updateTarget.GetChild(3).GetComponent<Text>().text = "-";
        }
        else
        {
            updateTarget.GetChild(3).GetComponent<Text>().text = "+ " + curItem.value[curItem.level - 1];
        }

        if (curItem.level == curItem.value.Length)
        {
            updateTarget.GetChild(4).GetComponent<Text>().text = "최대";
        }
        else
        {
            updateTarget.GetChild(4).GetComponent<Text>().text = curItem.price[curItem.level];
        }
    }

    // 업그레이드 정보를 게임매니저에 전달
    public void SendToGameManager()
    {
        for (int i = 0; i < totalUpgradeContentCount; i++)
        {
            UpgradeItem curItem = EventDBManager.instance.GetUpgradeItem(i);
            GameManager_JS.Instance.UpgradeInfoUpdate(i, curItem.level);
        }
    }

    // 잼 수량 텍스트 세팅
    void GemTextSetting()
    {
        gemText.text = "Gem : " + GameManager_JS.Instance.Gem.ToString();
    }
}
