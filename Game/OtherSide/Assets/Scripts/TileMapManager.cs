using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    public GameObject lightsource;
    public List<Vector3> availablePlaces;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
                //Vector3Int torchpos = map.WorldToCell(transform.position);
                //GameObject light = Instantiate(lightsource, torchpos, transform.rotation) as GameObject;
            }
        }


        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)map.transform.position.y));
                Vector3 place = map.GetCellCenterWorld(localPlace);
                if (map.HasTile(localPlace))
                {
                    //Tile at "place"
                    GameObject light = Instantiate(lightsource, place, transform.rotation) as GameObject;
                }
                else
                {
                    //No tile at "place"
                    
                }
            }
        }
        
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

            float intensity = dataFromTiles[clickedTile].intensity;
            float radius = dataFromTiles[clickedTile].radius;

            print("intensity,radius for " + clickedTile + " is " + intensity + " " + radius);
        }
        */

    }
}
