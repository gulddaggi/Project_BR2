using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debuff : MonoBehaviour
{
    public UnityEvent OnDebuffEnd;

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<ParticleSystem>().isStopped)
        {
            OnDebuffEnd.Invoke();
        }
    }
}
