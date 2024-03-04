using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemFormGetter : MonoBehaviour
{
    public List<Transform> objs = new List<Transform>();
    [SerializeField]
    private List<ShopItem> shopItemList = new List<ShopItem>();

    private void Awake()
    {
        // 대화 내용 세팅에 필요한 자식 오브젝트를 리스트에 저장. 순서대로 이름 - 대사.
        for (int i = 0; i < transform.childCount; i++)
        {
            objs.Add(this.transform.GetChild(i));
        }
    }

    public void AddShopItem(List<ShopItem> _shopItemList)
    {

        shopItemList = _shopItemList.ToList<ShopItem>();
    }

    public void ClearList()
    {
        shopItemList.Clear();
    }

    public void SelectShopItem(int _index)
    {
        int price = shopItemList[_index].price;
        Debug.Log(shopItemList[_index].item_Name);

        if (GameManager_JS.Instance.Coin >= price)
        {
            Debug.Log(shopItemList[_index].item_Name + "구매");
            GameManager_JS.Instance.Coin = -price;
            shopItemList[_index].EventOccur();
            objs[_index + 3].GetComponent<ItemSoldOut>().SoldOutTextOn();
        }
        else
        {
            Debug.Log("구매 불가");
            return;
        }
    }
}
