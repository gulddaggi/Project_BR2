using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField]
    GameObject choiceEvent;

    [SerializeField]
    GameObject choiceDialogue;

    [SerializeField]
    GameObject choiceMain;

    [SerializeField]
    LayerMask layerMask;

    RaycastHit hit;

    [SerializeField]
    float range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EventCheck();
    }

    void EventCheck()
    {
        // event trigger detected
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            //event type : choice
            if (Input.GetKeyDown(KeyCode.E) && hit.transform.tag == "EventTrigger")
            {
                //Debug.Log("Event trigger on");
                ChoiceEventStart();
            }
        }
    }

    void ChoiceEventStart()
    {
        choiceEvent.SetActive(true);
        Time.timeScale = 0f;
        // add function about stopping all objects
    }

    public void SwitchToChoice()
    {
        choiceDialogue.SetActive(false);
        choiceMain.SetActive(true);
    }

    public void ChoiceEventEnd()
    {
        ChoiceEventInit();
        choiceEvent.SetActive(false);
    }

    void ChoiceEventInit()
    {
        choiceDialogue.SetActive(true);
        choiceMain.SetActive(false);
        Time.timeScale = 1f;
        // add function about restarting move of all objects
    }
}
