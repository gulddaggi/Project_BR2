using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFormGetter : MonoBehaviour
{
    public List<Transform> objs = new List<Transform>();
    public List<ShopItem> shopItemList = new List<ShopItem>();

    private void Awake()
    {
        // 대화 내용 세팅에 필요한 자식 오브젝트를 리스트에 저장. 순서대로 이름 - 대사.
        for (int i = 0; i < transform.childCount; i++)
        {
            objs.Add(this.transform.GetChild(i));
        }
    }

    public void AddShopItem(ShopItem shopItem)
    {
        shopItemList.Add(shopItem);
    }

    public void ClearList()
    {
        shopItemList.Clear();
    }

    public void SelectShopItem(int _index)
    {
        int price = shopItemList[_index].price;

        if (GameManager_JS.Instance.Coin >= price)
        {
            GameManager_JS.Instance.Coin = -price;
            shopItemList[_index].EventOccur();
        }
    }
}
