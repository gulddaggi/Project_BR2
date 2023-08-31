using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SHOP_EVENT_TYPE
{
    sHPReinforce,
    sWeaponReinforce,
    sHPPotion
};

public interface IListener
{
    void EventOn(SHOP_EVENT_TYPE sEventType, Component from, object _param = null);
}

public class EventListener : MonoBehaviour, IListener
{
    public void EventOn(SHOP_EVENT_TYPE sEventType, Component from, object _param = null)
    {

    }
}
