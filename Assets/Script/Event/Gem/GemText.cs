using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemText : MonoBehaviour
{
    void Start()
    {
        GameManager_JS.Instance.gemText = this.gameObject;
        GemTextUpdate(GameManager_JS.Instance.Gem);
    }

    public void GemTextUpdate(int _gem)
    {
        this.GetComponent<Text>().text = "Gem : " + _gem.ToString();
    }
}
