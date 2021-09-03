using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tile
{
    public Vector3 position { get; protected set; }

    public float X { get { return position.x; } }
    public float Y { get { return position.y; } }
    public float Z { get { return position.z; } }

    public Dictionary<string, TileData> LevelTileDataDictionary = new Dictionary<string, TileData>();

    public TileData NowTileData;

    public Action<TileData> ChangeTile;

    public struct TileData
    {
        public string Level;
        public bool CanWalk;
        public SpriteRenderer spriteRenderer;
        public bool CanDig
        {
            get
            {
                return !CanWalk;
            }
        }

        public TileData(string Level, SpriteRenderer spriteRenderer, bool CanWalk = false)
        {
            this.Level = Level;
            this.CanWalk = CanWalk;
            this.spriteRenderer = spriteRenderer;
        }
    }
    public Tile(Vector3 position, Action<TileData> func)
    {
        this.position = position;
        ChangeTile += func;
    }

    public void SetNewData(string Level, SpriteRenderer spriteRenderer, bool CanWalk = false)
    {
        if (LevelTileDataDictionary.ContainsKey(Level))
        {
            Debug.LogErrorFormat("位置:{0}在{1}已被註冊過", position, Level);
            return;
        }

        TileData data = new TileData(Level, spriteRenderer, CanWalk);
        LevelTileDataDictionary.Add(Level, data);
        NowTileData = data;

        if (!CanWalk)
            GameWorldManager.ReSetObject += ReSetTile;
    }

    public void OnDigTile()
    {
        NowTileData.CanWalk = true;
        ChangeTile?.Invoke(NowTileData);

        string name = BackPackManager.Instance.ItemsDefault.GetRandomItem().ItemName;
        BackPackManager.Instance.NewItem(name, UnityEngine.Random.Range(0.01f, 25.00f));
    }

    public void ReSetTile()
    {
        NowTileData.CanWalk = false;
        ChangeTile?.Invoke(NowTileData);
    }
}

