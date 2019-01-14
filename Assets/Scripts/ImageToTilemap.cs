using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[RequireComponent(typeof(Tilemap))]
public class ImageToTilemap : MonoBehaviour {

    //MAP COLOR CODES
    private Color TILE_EMPTY = new Color(0,0,0,0);
    private Color TILE_NORMAL = Color.black;
    private Color TILE_HALF = Color.red;
    private Color TILE_DARK = Color.white;

    private const int NORMALSIZE = 16;
    private const int HALFSIZE = 12;
    private const int DARKSIZE = 12;

    public Tile[] normalTiles = new Tile[NORMALSIZE];
    public Tile[] halfTiles = new Tile[HALFSIZE];
    public Tile[] darkTiles = new Tile[DARKSIZE];
    public Texture2D map;
    Color[,] mapArray;
    Tilemap tilemap;
    

    // Use this for initialization
    void Start ()
    {
        mapArray = new Color[map.width + 2, map.height + 2]; //Allocating space for an empty border around the map
    }

    void Awake()
    {
        mapArray = new Color[map.width + 2, map.height + 2]; //Allocating space for an empty border around the map
        tilemap = this.GetComponent<Tilemap>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void UpdateTilemap()
    {
        this.tilemap.ClearAllTiles();
        MapToArray();
        ProcessMap();
    }

    void OnValidate()
    {
        if (normalTiles.Length != NORMALSIZE)
        {
            Debug.LogWarning("Don't change the 'normalTiles' field's array size!");
            Array.Resize(ref normalTiles, NORMALSIZE);
        }
    }

    void MapToArray()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                mapArray[x+1, y+1] = map.GetPixel(x, y); //Creating the empty border around the map
            }
        }
    }

    void ProcessMap()
    {
        for(int x = 1; x <= map.width; x++)  //starts at 1 and ends at map.width to ignore the empty border
        {
            for (int y = 1; y <= map.height; y++)  //same as stated above for x (replace map.width for map.height)
            {
                if (!mapArray[x, y].Equals(TILE_EMPTY)) {
                    Color left = mapArray[x - 1, y];
                    Color top = mapArray[x, y + 1];
                    Color bottom = mapArray[x, y - 1];
                    Color right = mapArray[x + 1, y];

                    Tile t = ProcessPixel(mapArray[x, y], left, top, bottom, right);
                    PlaceTile(t, x, y);
                }
            }
        }
    }

    void PlaceTile(Tile t, int x, int y)
    {
        tilemap = this.GetComponent<Tilemap>();
        Vector3Int pos = new Vector3Int(x, y, 0);
        tilemap.SetTile(pos, t);
    }

    Tile ProcessPixel(Color self, Color left, Color top, Color bottom, Color right)
    {
        List<int> list = new List<int>();
        if (self.Equals(TILE_NORMAL))
        {
            if (!isTileEmpty(left))
            {
                list = new List<int>() { 2, 3, 5, 6, 8, 9, 11, 12 };
            }
            else
            {
                list = new List<int>() { 1, 4, 7, 10, 13, 14, 15, 16 };
            }

            if (!isTileEmpty(top))
            {
                list.Remove(2);
                list.Remove(3);
                list.Remove(11);
                list.Remove(12);
                list.Remove(1);
                list.Remove(10);
                list.Remove(13);
                list.Remove(16);
            }
            else
            {
                list.Remove(5);
                list.Remove(6);
                list.Remove(8);
                list.Remove(9);
                list.Remove(4);
                list.Remove(7);
                list.Remove(14);
                list.Remove(15);
            }

            if (!isTileEmpty(bottom))
            {
                list.Remove(7);
                list.Remove(10);
                list.Remove(11);
                list.Remove(12);
                list.Remove(8);
                list.Remove(9);
                list.Remove(15);
                list.Remove(16);
            }
            else
            {
                list.Remove(1);
                list.Remove(4);
                list.Remove(2);
                list.Remove(5);
                list.Remove(3);
                list.Remove(6);
                list.Remove(13);
                list.Remove(14);
            }

            if (!isTileEmpty(right))
            {
                list.Remove(3);
                list.Remove(6);
                list.Remove(9);
                list.Remove(12);
                list.Remove(13);
                list.Remove(14);
                list.Remove(15);
                list.Remove(16);
            }
            else
            {
                list.Remove(1);
                list.Remove(2);
                list.Remove(4);
                list.Remove(5);
                list.Remove(7);
                list.Remove(8);
                list.Remove(10);
                list.Remove(11);
            }

            print("normalTiles -> " + list.ToArray()[0]);
            return normalTiles[list.ToArray()[0] - 1];
        }
        else if (self.Equals(TILE_HALF))
        {
            if (!isTileEmpty(left))
            {
                list = new List<int>() { 2, 3, 6, 7, 10, 11 };
            }
            else
            {
                list = new List<int>() { 1, 4, 5, 8, 9, 12};
            }

            if (!isTileEmpty(top))
            {
                list.Remove(5);
                list.Remove(6);
                list.Remove(7);
                list.Remove(8);
                list.Remove(9);
                list.Remove(10);
                list.Remove(11);
                list.Remove(12);
            }
            else
            {
                list.Remove(1);
                list.Remove(2);
                list.Remove(3);
                list.Remove(4);
            }

            if (!isTileEmpty(bottom))
            {
                list.Remove(1);
                list.Remove(2);
                list.Remove(3);
                list.Remove(4);
                list.Remove(5);
                list.Remove(6);
                list.Remove(7);
                list.Remove(8);
            }
            else
            {
                list.Remove(9);
                list.Remove(10);
                list.Remove(11);
                list.Remove(12);
            }

            if (!isTileEmpty(right))
            {
                list.Remove(3);
                list.Remove(4);
                list.Remove(7);
                list.Remove(8);
                list.Remove(11);
                list.Remove(12);
            }
            else
            {
                list.Remove(1);
                list.Remove(2);
                list.Remove(5);
                list.Remove(6);
                list.Remove(9);
                list.Remove(10);
            }

            print("halfTiles -> " + list.ToArray());
            return halfTiles[list.ToArray()[0] - 1];
        }
        else if (self.Equals(TILE_DARK))
        {

        }
        print("null");
        return null;
    }

    //CHANGE
    bool isTileEmpty(Color c)
    {
        return c.a == 0;
    }
}
