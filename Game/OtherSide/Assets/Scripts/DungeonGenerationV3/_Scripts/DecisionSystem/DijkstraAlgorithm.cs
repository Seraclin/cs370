using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm
{
    public static Dictionary<Vector2Int, int> Dijkstra(Graph graph, Vector2Int startposition)
    {
        Queue<Vector2Int> unfinishedVertices = new Queue<Vector2Int>();

        Dictionary<Vector2Int, int> distanceDictionary = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, Vector2Int> parentDictionary = new Dictionary<Vector2Int, Vector2Int>();

        distanceDictionary[startposition] = 0;
        parentDictionary[startposition] = startposition;

        foreach (Vector2Int vertex in graph.GetNeighbours4Directions(startposition))
        {
            unfinishedVertices.Enqueue(vertex);
            parentDictionary[vertex] = startposition;
        }

        while (unfinishedVertices.Count > 0)
        {
            Vector2Int vertex = unfinishedVertices.Dequeue();
            int newDistance = distanceDictionary[parentDictionary[vertex]]+1;
            if (distanceDictionary.ContainsKey(vertex) && distanceDictionary[vertex] <= newDistance)
                continue;
            distanceDictionary[vertex] = newDistance;

            foreach (Vector2Int neighbour in graph.GetNeighbours4Directions(vertex))
            {
                if (distanceDictionary.ContainsKey(neighbour))
                    continue;
                unfinishedVertices.Enqueue(neighbour);
                parentDictionary[neighbour] = vertex;
            }
        }

        return distanceDictionary;
    }
}
