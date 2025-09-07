using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSoldOut : MonoBehaviour
{
    [SerializeField]
    Text soldOutText;

    [SerializeField]
    Button shopItem;
    
    public void SoldOutTextOn()
    {
        soldOutText.gameObject.SetActive(true);
        this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
        shopItem.GetComponent<Button>().enabled = false;
    }

    public void SoldOutTextOff()
    {
        soldOutText.gameObject.SetActive(false);
        this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.25f);
        shopItem.GetComponent<Button>().enabled = true;
    }
}
