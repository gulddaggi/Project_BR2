using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_Boss : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    Text text;

    public void HPUpdate(float _fullHp, float _curHP)
    {
        text.text = _curHP.ToString() + " / " + _fullHp.ToString();
        image.fillAmount = _curHP / _fullHp;
        if(image.fillAmount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
