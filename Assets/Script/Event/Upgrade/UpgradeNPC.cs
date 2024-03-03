using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNPC : MonoBehaviour
{
    [SerializeField]
    GameObject NPCText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NPCText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NPCText.SetActive(false);
        }
    }
}
