using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // 싱글톤으로 구현
    public static EventManager Instance { get { return instance; } }

    private static EventManager instance = null;
    private Dictionary<SHOP_EVENT_TYPE, List<IListener>> ListenerDic = new Dictionary<SHOP_EVENT_TYPE, List<IListener>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    // 리스너 추가
    public void AddListener(SHOP_EVENT_TYPE sEventType, IListener listener)
    {
        List<IListener> listenerList = null;

        if (ListenerDic.TryGetValue(sEventType, out listenerList))
        {
            listenerList.Add(listener);
            return;
        }
        else
        {
            listenerList = new List<IListener>();
            listenerList.Add(listener);
            ListenerDic.Add(sEventType, listenerList);
        }
    }

    // 포스터 -> 매니저. 이벤트 발생을 매니저에 알림
    public void EventPostToManager(SHOP_EVENT_TYPE sEventType, Component from, object _param = null)
    {
        List<IListener> listenerList = null;

        // 해당 이벤트 발생을 전달받을 리스너 없음
        if (!ListenerDic.TryGetValue(sEventType, out listenerList))
        {
            return;
        }
        else
        {
            for (int i = 0; i < listenerList.Count; i++)
            {
                listenerList?[i].EventOn(sEventType, from, _param); // ?. : 연산자. null이 아니면 참조. null이면 null로 처리   
            }
        }
    }

    public void TurnBasedEventPost()
    {
        List<IListener> listenerList = null;

        for (int i = 0; i < System.Enum.GetValues(typeof(SHOP_EVENT_TYPE)).Length; i++)
        {
            switch (i)
            {
                case 0:
                    break;

                case 1:
                    break;

                case 2:
                    if (!ListenerDic.TryGetValue(SHOP_EVENT_TYPE.sHPPotion, out listenerList))
                    {
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < listenerList.Count; j++)
                        {
                            listenerList?[j].TurnBasedEventOn(i); // ?. : 연산자. null이 아니면 참조. null이면 null로 처리   
                        }
                    }
                    break;

                case 3:
                    if (!ListenerDic.TryGetValue(SHOP_EVENT_TYPE.sSpeedReinforce, out listenerList))
                    {
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < listenerList.Count; j++)
                        {
                            listenerList?[j].TurnBasedEventOn(i);
                        }
                    }
                    break;

                case 4:
                    break;

                case 5:
                    if (!ListenerDic.TryGetValue(SHOP_EVENT_TYPE.sAllReinforce, out listenerList))
                    {
                        return;
                    }
                    else
                    {
                        for (int j = 0; j < listenerList.Count; j++)
                        {
                            listenerList?[j].TurnBasedEventOn(i);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void RemoveEvent(SHOP_EVENT_TYPE sEventType)
    {
        ListenerDic.Remove(sEventType);
    }

    // 씬 변경 시 파괴된 오브젝트 참조 문제 해결 메서드
    public void RemoveDestroies()
    {
        Dictionary<SHOP_EVENT_TYPE, List<IListener>> NewListenerDic = new Dictionary<SHOP_EVENT_TYPE, List<IListener>>();

        // 기존 딕셔너리 내 데이터 검사
        foreach (KeyValuePair<SHOP_EVENT_TYPE, List<IListener>> Item in ListenerDic)
        {
            // 삭제
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            // 새로운 딕셔너리에 추가
            if (Item.Value.Count > 0)
            {
                NewListenerDic.Add(Item.Key, Item.Value);
            }
        }

        // 기존 딕셔너리 갱신
        ListenerDic = NewListenerDic;
    }

    // 씬 변경시 호출되는 이벤트 함수
    private void OnLevelWasLoaded(int level)
    {
        RemoveDestroies();
    }

    // AbilityController를 등록
    public void AssignAbManager(AbilityListManager _abManager)
    {
        GetComponentInChildren<AbilitySelector>().AssignAbManager(_abManager);
    }
}
