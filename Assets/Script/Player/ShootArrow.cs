using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public GameObject player;
    public GameObject Arrow;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (player != null) { var instance = Instantiate(Arrow, gameObject.transform.position, player.transform.rotation); }
        else { player = GameObject.FindGameObjectWithTag("Player"); var instance = Instantiate(Arrow, gameObject.transform.position, player.transform.rotation); }
    }
}
