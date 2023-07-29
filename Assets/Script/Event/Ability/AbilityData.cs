using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : EventData
{
    [SerializeField]
    string NPCName;

    // typeIndex 프로퍼티
    public string NPCNAME
    {
        get { return NPCName; }
        set { NPCName = value; }
    }
}
