using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform PlayerTransform;
    [SerializeField] Vector3 offset;
    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerTransform.position + offset;
    }
}
