using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> corridorPositions;

    public HashSet<Vector2Int> GetRoomFloorWithoutCorridors(Vector2Int dictionaryKey)
    {
        HashSet<Vector2Int> roomFloorNoCorridors = new HashSet<Vector2Int>(roomsDictionary[dictionaryKey]);
        roomFloorNoCorridors.ExceptWith(corridorPositions);
        return roomFloorNoCorridors;
    }
}