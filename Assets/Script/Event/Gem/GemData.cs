using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemData : EventData
{
    private void Awake()
    {
        SetValue();
    }

    void SetValue()
    {
        int tmp = GameManager_JS.Instance.GetDungeonCount() / 7;
        TypeIndex = (tmp > 1) ? tmp * 10 + 70 : 70;
    }
}
