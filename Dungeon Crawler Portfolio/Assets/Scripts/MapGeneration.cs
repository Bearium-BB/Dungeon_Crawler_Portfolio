using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGeneration : MonoBehaviour
{
    private List<HashSet<Vector3Int>> rooms = new List<HashSet<Vector3Int>>();
    private List<HashSet<Vector3Int>> hallwayPositions = new List<HashSet<Vector3Int>>();
    private HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public float squareSize = 1f;


    // Start is called before the first frame update
    void Start()
    {
        rooms.Add(GenerateRoom(20,10, Vector3Int.zero));
        rooms.Add(GenerateRoom(200,50, new Vector3Int(100,0, 100)));
        rooms.Add(GenerateRoom(200, 50, new Vector3Int(300,0 ,2)));
        var hallway1 = FindClosestVectors(rooms[0], rooms[1]);
        var hallway2 = FindClosestVectors(rooms[1], rooms[2]);
        var hallway3 = FindClosestVectors(rooms[0], rooms[2]);

        hallwayPositions.Add(GenerateHallways(hallway1.Item1, hallway1.Item2,2));
        hallwayPositions.Add(GenerateHallways(hallway2.Item1, hallway2.Item2, 2));
        hallwayPositions.Add(GenerateHallways(hallway3.Item1, hallway3.Item2, 2));


        MakeFloorPositions();
        AddWallsToEmptyAdjacentPositions();

        InstantiateGrid();
    }

    private void MakeFloorPositions()
    {
        foreach (var i in hallwayPositions)
        {
            floorPositions.UnionWith(i);

        }
        foreach (var i in rooms)
        {
            floorPositions.UnionWith(i);

        }
    }

    private HashSet<Vector3Int> GenerateRoom(int iteration,int longToGo, Vector3Int startingPosition)
    {
        HashSet<Vector3Int> FloorPositions = new HashSet<Vector3Int>();
        Vector3Int currentPosition = startingPosition;

        for (int i = 0; i <= iteration; i++)
        {
            currentPosition = startingPosition;
            for (int j = 0; j <= longToGo; j++)
            {
                currentPosition += GetRandomDirection();
                if (!FloorPositions.Contains(currentPosition))
                {
                    FloorPositions.Add(currentPosition);
                }
                else
                {
                    j--;
                }
            }
        }
        return FloorPositions;
    }

    public static HashSet<Vector3Int> GenerateHallways(Vector3Int startPoint, Vector3Int endPoint, int hallwayWidth)
    {
        HashSet<Vector3Int> hallways = new HashSet<Vector3Int>();

        int deltaX = Mathf.Abs(endPoint.x - startPoint.x);
        int deltaZ = Mathf.Abs(endPoint.z - startPoint.z);
        int stepX = startPoint.x < endPoint.x ? 1 : -1;
        int stepY = startPoint.z < endPoint.z ? 1 : -1;
        int error = deltaX - deltaZ;

        int halfWidth = hallwayWidth / 2;
        int minX = -halfWidth;
        int maxX = halfWidth;
        int minY = -halfWidth;
        int maxY = halfWidth;

        // Generate hallways along the line
        while (true)
        {
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    Vector3Int hallwayPoint = new Vector3Int(startPoint.x + i,0, startPoint.z + j);
                    hallways.Add(hallwayPoint);
                }
            }

            if (startPoint.x == endPoint.x && startPoint.z == endPoint.z)
            {
                break;
            }

            int error2 = error * 2;

            if (error2 > -deltaZ)
            {
                error -= deltaZ;
                startPoint.x += stepX;
            }

            if (error2 < deltaX)
            {
                error += deltaX;
                startPoint.z += stepY;
            }
        }

        return hallways;
    }

    private void InstantiateGrid()
    {
        foreach (Vector3Int position in floorPositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, position.z * squareSize);
            GameObject square = Instantiate(floorPrefab, spawnPosition, Quaternion.identity);
            square.transform.localScale = new Vector3(squareSize, squareSize, 1f);
        }

        foreach (Vector3Int position in wallPositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, position.z * squareSize);
            GameObject wall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
            wall.transform.localScale = new Vector3(squareSize, squareSize, 1f);

        }

        //foreach (Vector3Int position in floorPositions)
        //{
        //    if (Random.Range(0, 50) == 2)
        //    {
        //        Vector3 spawnPosition = new Vector3(position.x * 1, position.y * 1, 0f);
        //        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        //    }
        //}
    }

    private Vector3Int[] GetAdjacentPositions(Vector3Int position)
    {
        Vector3Int[] adjacentPositions = new Vector3Int[4];

        adjacentPositions[0] = position + Vector3Int.back;
        adjacentPositions[1] = position + Vector3Int.right;
        adjacentPositions[2] = position + Vector3Int.forward;
        adjacentPositions[3] = position + Vector3Int.left;

        return adjacentPositions;
    }


    private void AddWallsToEmptyAdjacentPositions()
    {
        foreach (Vector3Int position in floorPositions)
        {
            // Check adjacent positions
            Vector3Int[] adjacentPositions = GetAdjacentPositions(position);
            foreach (Vector3Int adjPosition in adjacentPositions)
            {
                // If adjacent position is not in the square positions hash set, add a wall
                if (!floorPositions.Contains(adjPosition))
                {
                    wallPositions.Add(adjPosition);
                }
            }
        }
    }


    private Vector3Int GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4); // 0: Up, 1: Right, 2: Down, 3: Left

        switch (randomIndex)
        {
            case 0:
                return Vector3Int.forward;
            case 1:
                return Vector3Int.right;
            case 2:
                return Vector3Int.back;
            case 3:
                return Vector3Int.left;
            default:
                return Vector3Int.zero;
        }
    }

    public (Vector3Int, Vector3Int) FindClosestVectors(HashSet<Vector3Int> list1, HashSet<Vector3Int> list2)
    {
        Vector3Int closestVector1 = default;
        Vector3Int closestVector2 = default;
        int closestDistance = int.MaxValue;

        foreach (var vector1 in list1)
        {
            foreach (var vector2 in list2)
            {
                int distance = CalculateDistance(vector1, vector2);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVector1 = vector1;
                    closestVector2 = vector2;
                }
            }
        }

        return (closestVector1, closestVector2);
    }

    public int CalculateDistance(Vector3Int vector1, Vector3Int vector2)
    {
        int deltaX = vector2.x - vector1.x;
        int deltaz = vector2.z - vector1.z;
        return deltaX * deltaX + deltaz * deltaz; // Euclidean distance squared
    }
}




