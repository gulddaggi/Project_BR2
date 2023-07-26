using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : MonoBehaviour
{
    // 이벤트 타입.
    // 0 : 능력, 1 : 코인, 2 : 잼, 3 : 업그레이드
    [SerializeField]
    private int eventType;

    // 이벤트 타입에서 세분화되는 인덱스.
    // 각 종류의 이벤트에서 다시 종류별 구분이 필요한 경우 사용. 우선 능력에서만.
    // 0 : 물, 1 : 불, 2 : 바람, 3 : 대지, 4 : 해, 5 : 달
    [SerializeField]
    private int typeIndex;

    // eventType 프로퍼티
    public int EventType
    {
        get {  return eventType; }
    }

    // typeIndex 프로퍼티
    public int TypeIndex
    {
        get { return typeIndex; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
