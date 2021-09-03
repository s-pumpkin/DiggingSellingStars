using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameWorldManager : MonoBehaviour
{
    private static GameWorldManager instance;
    public static GameWorldManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameWorldManager>();
            return instance;
        }
    }

    public Vector2Int StartPos;
    public Vector2Int EndPos = new Vector2Int(15, 7);
    public Vector3 PlayerStartPosititon = Vector3.zero;
    public Vector3[] IgnorePos;

    public Tile[,] tiles;
    private TileControl tileSpriteControl = new TileControl();

    public static int LevelCount = 0;

    public static Action ReSetObject;

    private void Awake()
    {
        tileSpriteControl.GenerateLevelTileInfo(StartPos, EndPos, this.gameObject.transform);

        PlayerControl.Instance.transform.position = PlayerStartPosititon;
    }

    public Tile GetTileAt(int x, int y)
    {
        if (!x.Scope(StartPos.x, EndPos.x) && !y.Scope(StartPos.y, EndPos.y))
        {
            Debug.LogError("Tile (" + x + "," + y + ") ¶W¥X½d³ò");
            return null;
        }
        return tiles[x, y];
    }
    public Tile GetTileAt(Vector2 position)
    {
        int x = Mathf.CeilToInt(position.x - 0.5f), y = Mathf.CeilToInt(position.y - 0.5f);
        if (!x.Scope(StartPos.x, EndPos.x) || !y.Scope(StartPos.y, EndPos.y))
        {
            Debug.LogError("Tile (" + x + "," + y + ") ¶W¥X½d³ò");
            return null;
        }
        // Debug.Log(x + "   " + y);
        return tiles[x, y];
    }

    public void LevelReset()
    {
        ReSetObject?.Invoke();
    }
}
