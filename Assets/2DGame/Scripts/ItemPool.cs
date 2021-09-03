using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPool", menuName = "Invrntory/ItemPool")]
public class ItemPool : ScriptableObject
{
    public List<ItemDefault> DefaultPool = new List<ItemDefault>();

    public ItemData GetRandomItem()
    {
        return DefaultPool[Random.Range(0, DefaultPool.Count)].itemData;
    }
}
