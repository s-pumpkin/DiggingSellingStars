using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Invrntory/ItemData")]
public class ItemDefault : ScriptableObject
{
    public ItemData itemData;
}

[System.Serializable]
public class ItemData
{
    private int ItemID;
    public string ItemName;
    public Sprite ItemImage;
    public float weight { get; protected set; }

    public ItemData(ItemData data, float weight, int ItemID = -1)
    {
        this.ItemID = ItemID == -1 ? this.ItemID.GetHashCode() : ItemID;
        this.ItemName = data.ItemName;
        this.ItemImage = data.ItemImage;

        this.weight = weight;
    }

    public string ItemInfo
    {
        get
        {
            return string.Format("{0}\n­«¶q:{1} kg", ItemName, weight.ToString("f2"));
        }
    }
}
