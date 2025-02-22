 using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject squarePrefab;     // Prefab for the 2D square
    public GameObject wallPrefab;       // Prefab for the wall
    public GameObject EnemyPrefab;
    public float squareSize = 1f;       // Size of each square
    public int numRooms = 2;
    public int hallwayLength = 15;


    private HashSet<Vector2Int> squarePositions;  // Hash to store the square positions
    private HashSet<Vector2Int> wallPositions;    // Hash to store the wall positions

    private void Start()
    {
        squarePositions = new HashSet<Vector2Int>();
        wallPositions = new HashSet<Vector2Int>();
        GenerateGrid();
        AddWallsToEmptyAdjacentPositions();
        InstantiateGrid();
    }

    private void GenerateGrid()
    {
        Vector2Int currentPosition = Vector2Int.zero; // Start at (0, 0)

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
                Vector2Int hallwayDirection = GetRandomDirection();
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
        foreach (Vector2Int position in squarePositions)
        {
            // Check adjacent positions
            Vector2Int[] adjacentPositions = GetAdjacentPositions(position);
            foreach (Vector2Int adjPosition in adjacentPositions)
            {
                // If adjacent position is not in the square positions hash set, add a wall
                if (!squarePositions.Contains(adjPosition))
                {
                    wallPositions.Add(adjPosition);
                }
            }
        }
    }

    private Vector2Int[] GetAdjacentPositions(Vector2Int position)
    {
        Vector2Int[] adjacentPositions = new Vector2Int[4];

        adjacentPositions[0] = position + Vector2Int.up;
        adjacentPositions[1] = position + Vector2Int.right;
        adjacentPositions[2] = position + Vector2Int.down;
        adjacentPositions[3] = position + Vector2Int.left;

        return adjacentPositions;
    }

    private void InstantiateGrid()
    {
        // Instantiate squares at the stored positions
        foreach (Vector2Int position in squarePositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, 0f);
            GameObject square = Instantiate(squarePrefab, spawnPosition, Quaternion.identity);
            square.transform.localScale = new Vector3(squareSize, squareSize, 1f);
        }

        // Instantiate walls at the stored positions
        foreach (Vector2Int position in wallPositions)
        {
            Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, 0f);
            GameObject wall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
            wall.transform.localScale = new Vector3(squareSize, squareSize, 1f);

        }

        foreach (Vector2Int position in squarePositions)
        {
            if (Random.Range(0, 10) == 5)
            {
                Vector3 spawnPosition = new Vector3(position.x * squareSize, position.y * squareSize, 0f);
                Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector2Int GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4); // 0: Up, 1: Right, 2: Down, 3: Left

        switch (randomIndex)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            case 3:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }

}
