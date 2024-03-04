using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // 이름
    public string item_Name;

    // 설명
    public string description;

    // 적용 옵션
    public string option_Name;

    // 이벤트 타입. 0 : sHPPotion, 1 : sHPReinforce, 2 : sWeaponReinforce
    public int eventType;

    // 적용 값
    public string value;

    public int price = 10;

    // 적용 턴 수
    public string turn;

    public void EventOccur()
    {
        EventManager.Instance.EventPostToManager((SHOP_EVENT_TYPE)this.eventType, this, price); // 일단 매개변수 param은 안씀.
    }

    SHOP_EVENT_TYPE TypeReturn(int _eventType)
    {
        switch (_eventType)
        {
            case 0:
                return SHOP_EVENT_TYPE.sHPPotion;

            case 1:
                return SHOP_EVENT_TYPE.sHPReinforce;

            case 2:
                return SHOP_EVENT_TYPE.sWeaponReinforce;

            default:
                return 0;
        }
    }
}
