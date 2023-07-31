using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffChecker : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer objColor;

    float time = 0f;
    bool timerOn = false;
    Color originColor;
    int curDebuffIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        originColor = objColor.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
        }

        if (time > 5f)
        {
            DebuffOff();
        }
    }

    public void DebuffCheck(int index)
    {
        timerOn = true;
        curDebuffIndex = index;
        switch (index)
        {
            case 0:
                time = 0f;
                objColor.material.color = Color.blue;
                break;
            default:
                break;
        }

    }

    public void DebuffOff()
    {
        time = 0f;
        timerOn = false;
        objColor.material.color = originColor;
        //GameObject.Find("Player").GetComponent<Player>().DebuffOff(curDebuffIndex);

    }
}
