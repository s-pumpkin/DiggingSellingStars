using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackManager : MonoBehaviour
{
    private static BackPackManager instance;
    public static BackPackManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BackPackManager>();
            return instance;
        }
    }

    public GameObject DragTemp;
    public GameObject ItemsParent;
    public GameObject DefaultItemUI;
    public GameObject[] InventorySolotPool;
    public int BagMax { get { return InventorySolotPool.Length; } }
    public Text InfoText;

    public Dictionary<GameObject, ItemUI> BackPackDictionary = new Dictionary<GameObject, ItemUI>();

    public ItemPool ItemsDefault;
    public Dictionary<GameObject, ItemUI> GameObjectItemUIDictionary = new Dictionary<GameObject, ItemUI>();

    private void Awake()
    {
        instance = this;
        Info();
    }

    private void Info()
    {
        foreach (GameObject go in InventorySolotPool)
        {
            BackPackDictionary.Add(go, null);
        }
    }

    private GameObject AddToBackPack(ItemUI itemUI)
    {
        foreach (KeyValuePair<GameObject, ItemUI> backPack in BackPackDictionary)
        {
            if (backPack.Value == null)
            {
                BackPackDictionary[backPack.Key] = itemUI;
                return backPack.Key;
            }
        }
        return null;
    }

    private void RemoveBackPack(ItemUI itemUI)
    {
        foreach (KeyValuePair<GameObject, ItemUI> backPack in BackPackDictionary)
        {
            if (backPack.Value == itemUI)
            {
                BackPackDictionary[backPack.Key] = null;
                return;
            }
        }
    }

    public void NewItem(string name, float weight)
    {
        //Data
        ItemDefault defaultItem = ItemsDefault.DefaultPool.Find(x => x.itemData.ItemName == name);
        ItemData newItem = new ItemData(defaultItem.itemData, weight);

        GameObject go = Instantiate(DefaultItemUI);
        go.transform.SetParent(ItemsParent.transform);
        go.transform.localScale = Vector3.one;
        go.name = name + "  " + weight;

        ItemUI newItemUI = GameObjectItemUIDictionary[go];
        newItemUI.MyItemData = newItem;
        newItemUI.SetIamge();
        newItemUI.originalParent = ItemsParent.transform;

        GameObject goParent = AddToBackPack(newItemUI);

        go.transform.position = goParent.transform.position;
    }

    public void DeleItem(GameObject go)
    {
        RemoveBackPack(GameObjectItemUIDictionary[go]);
        Destroy(go);
    }

    public void RegisterGameObjectItemUIDictionary(GameObject go, ItemUI ui)
    {
        if (!GameObjectItemUIDictionary.ContainsKey(go))
            GameObjectItemUIDictionary.Add(go, ui);
    }

    public void UnRegisterGameObjectItemUIDictionary(GameObject go)
    {
        if (GameObjectItemUIDictionary.ContainsKey(go))
            GameObjectItemUIDictionary.Remove(go);
    }
}
