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

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Start searching event trigger
        Physics.Raycast(transform.position, Vector3.forward, out hit, 100.0f);

        Debug.Log(hit.transform.tag);

        // event trigger detected
        if (Input.GetKeyDown(KeyCode.E) && hit.transform.tag == "EventTrigger")
        {
            Debug.Log("Event trigger on");
            ChoiceEventStart();
        }
    }

    void ChoiceEventStart()
    {
        choiceEvent.SetActive(true);
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
    }
}
