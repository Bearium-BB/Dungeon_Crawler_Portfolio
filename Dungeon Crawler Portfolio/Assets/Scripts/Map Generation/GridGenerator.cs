 using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject squarePrefab;     // Prefab for the 2D square
    public GameObject wallPrefab;       // Prefab for the wall
    public float squareSize = 1f;       // Size of each square
    public int numRooms = 2;
    public int hallwayLength = 15;


    private HashSet<Vector3Int> squarePositions;  // Hash to store the square positions
    private HashSet<Vector3Int> wallPositions;    // Hash to store the wall positions

    private void Start()
    {
        squarePositions = new HashSet<Vector3Int>();
        wallPositions = new HashSet<Vector3Int>();
        GenerateGrid();
        AddWallsToEmptyAdjacentPositions();
        InstantiateGrid();
    }

    private void GenerateGrid()
    {
        Vector3Int currentPosition = Vector3Int.zero; // Start at (0, 0)

        int roomCount = 0;
        while (roomCount < numRooms)
        {
            // Generate a room
            for (int i = 0; i < 250; i++)
            {
                currentPosition += GetRandomDirection();
                if (!squarePositions.Contains(currentPosition))
                {
                    squarePositions.Add(currentPosition);
                }
                else
                {
                    i--;
                }
            }

            roomCount++;

            if (roomCount < numRooms)
            {
                Vector3Int hallwayDirection = GetRandomDirection();
                for (int i = 0; i < hallwayLength; i++)
                {
                    currentPosition += hallwayDirection;
                    squarePositions.Add(currentPosition);
                }
            }
        }
    }

    private void AddWallsToEmptyAdjacentPositions()
    {
        foreach (Vector3Int position in squarePositions)
        {
            // Check adjacent positions
            Vector3Int[] adjacentPositions = GetAdjacentPositions(position);
            foreach (Vector3Int adjPosition in adjacentPositions)
            {
                // If adjacent position is not in the square positions hash set, add a wall
                if (!squarePositions.Contains(adjPosition))
                {
                    wallPositions.Add(adjPosition);
                }
            }
        }
    }

    private Vector3Int[] GetAdjacentPositions(Vector3Int position)
    {
        Vector3Int[] adjacentPositions = new Vector3Int[4];

        adjacentPositions[0] = position + Vector3Int.forward;
        adjacentPositions[1] = position + Vector3Int.right;
        adjacentPositions[2] = position + Vector3Int.back;
        adjacentPositions[3] = position + Vector3Int.left;

        return adjacentPositions;
    }

    private void InstantiateGrid()
    {
        // Instantiate squares at the stored positions
        foreach (Vector3Int position in squarePositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, position.z * squareSize);
            GameObject square = Instantiate(squarePrefab, spawnPosition, Quaternion.identity);
            square.transform.localScale = new Vector3(squareSize, squareSize, 1f);
        }

        // Instantiate walls at the stored positions
        foreach (Vector3Int position in wallPositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, position.z * squareSize);
            GameObject wall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
            wall.transform.localScale = new Vector3(squareSize, squareSize, 1f);

        }

        //foreach (Vector3Int position in squarePositions)
        //{
        //    if (Random.Range(0, 10) == 5)
        //    {
        //        Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, 0f);
        //        Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        //    }
        //}
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

}
