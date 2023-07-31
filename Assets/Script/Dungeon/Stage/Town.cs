using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Stage
{
    protected override void Start()
    {
        base.Start();
        GameManager_JS.Instance.SetIsMoveOn(true);
    }

    void Update()
    {
        
    }
}
