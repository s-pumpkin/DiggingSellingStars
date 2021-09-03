using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ShopManager>();
            return instance;
        }
    }

    public GameObject SellObjectPool;

    public ItemUI itemUI;
    public GameObject SellObject;
    public Text SellWeightText;
    public Dictionary<string, ItemData> SelinglItems = new Dictionary<string, ItemData>();
    public bool isShop;

    private void OnEnable()
    {
        isShop = true;
        if (SelinglItems.Count != 0)
            return;
        newSellItems();
    }

    public void newSellItems(bool removeOther = false)
    {
        if (removeOther)
        {
            SelinglItems.Clear();
        }

        ItemData itemDataDefault = BackPackManager.Instance.ItemsDefault.GetRandomItem();
        ItemData newItemData = new ItemData(itemDataDefault, Random.Range(5.00f, 50.00f));
        SelinglItems.Add(newItemData.ItemName, newItemData);
        SetSellUI(newItemData);
    }

    public void SetSellUI(ItemData itemData)
    {
        itemUI.MyItemData = itemData;
        itemUI.SetIamge();
        SellWeightText.text = "需求量: " + itemUI.MyItemData.weight.ToString("f2") + " kg";
    }

    private void OnDisable()
    {
        isShop = false;
        for (int i = SellObjectPool.transform.childCount; i > 0;)
        {
            i--;
            ItemUI itemUI = BackPackManager.Instance.GameObjectItemUIDictionary[SellObjectPool.transform.GetChild(i).gameObject];
            itemUI.BackToTheBackpack();
        }
    }

    public void CheckMerchandise()
    {
        bool OtherItems = false;
        float weight = 0;
        if (SellObjectPool.transform.childCount == 0)
            return;

        List<GameObject> Childs = new List<GameObject>();
        for (int i = 0; i < SellObjectPool.transform.childCount; i++)
        {
            ItemUI itemUIChild = BackPackManager.Instance.GameObjectItemUIDictionary[SellObjectPool.transform.GetChild(i).gameObject];
            Childs.Add(itemUIChild.gameObject);
            if (itemUIChild.MyItemData.ItemName != itemUI.MyItemData.ItemName)
            {
                OtherItems = true;
                continue;
            }
            weight += itemUIChild.MyItemData.weight;
        }

        CheckValue(weight, OtherItems);

        foreach (GameObject go in Childs)
            BackPackManager.Instance.DeleItem(go);

        newSellItems(true);
    }

    public void CheckValue(float weight, bool OtherItems)
    {
        float count = weight.TakeDecimalPoint(2) - itemUI.MyItemData.weight.TakeDecimalPoint(2);
        if (count < 0)
        {
            if (OtherItems)
            {
                ScoreManager.Instance.AddScore(-15);
                ShakeUI.Instance.SetText("給錯礦石了!!");
            }
            else
            {
                Debug.Log("太少了");
                ScoreManager.Instance.AddScore(-5);
                ShakeUI.Instance.SetText("太少了");
            }
        }
        else if (count >= 0 && (OtherItems || count >= itemUI.MyItemData.weight.TakeDecimalPoint(2) * .25f))
        {
            Debug.Log("虧阿!太多了!");
            ScoreManager.Instance.AddScore(-5);
            ShakeUI.Instance.SetText("虧阿!太多了!");
        }
        else
        {
            Debug.Log("交易完成");
            ScoreManager.Instance.AddScore(10);
            ShakeUI.Instance.SetText("交易完成");
        }
    }
}
