using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]

public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public float radius, intensity;
}
