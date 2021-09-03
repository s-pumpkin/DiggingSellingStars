using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileControl
{
    Dictionary<Tile, GameObject> TileGameObjectDictionary = new Dictionary<Tile, GameObject>();


    public void GenerateLevelTileInfo(Vector2Int Start, Vector2Int End, Transform parent)
    {
        int Xmin = Mathf.Min(Start.x, End.x);
        int Xmax = Mathf.Max(Start.x, End.x);
        int Ymin = Mathf.Min(Start.y, End.y);
        int Ymax = Mathf.Max(Start.y, End.y);

        GameWorldManager.Instance.tiles = new Tile[Xmax - Xmin + 1, Ymax - Ymin + 1];
        for (int x = 0; x <= Xmax - Xmin; x++)
        {
            for (int y = 0; y <= Ymax - Ymin; y++)
            {
                Tile tile_Data = new Tile(new Vector3(Xmin + x, Ymin + y, 0), ChangeSprite);
                GameObject tile_GO = new GameObject();
                TileGameObjectDictionary.Add(tile_Data, tile_GO);

                tile_GO.transform.position = new Vector3(tile_Data.X, tile_Data.Y, tile_Data.Z);
                tile_GO.transform.SetParent(parent, true);

                SpriteRenderer sr = tile_GO.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = "Rock";

                //³]©wtileData
                bool cnaWalk = GameWorldManager.Instance.IgnorePos.Any(x => x == tile_Data.position);
                tile_Data.SetNewData("Level" + GameWorldManager.LevelCount, sr, cnaWalk);

                GameWorldManager.Instance.tiles[Xmin + x, Ymin + y] = tile_Data;
            }
        }
    }

    public void ChangeSprite(Tile.TileData tileData)
    {
        if (tileData.CanDig)
            tileData.spriteRenderer.sprite = null;
        else
            tileData.spriteRenderer.sprite = Resources.Load<Sprite>("Sprite/Diamond");
    }

}
