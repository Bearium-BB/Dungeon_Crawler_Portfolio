using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class MapGrid : MonoBehaviour
{
    List<GridCellList> grid = new List<GridCellList>();
    List<GridCellList> chunkGrid = new List<GridCellList>();

    public GameObject gameObjectWall;
    public GameObject gameObjectChunk;

    void Start()
    {
        GenerateGrid(1200);

        MakeMapTexture();


        chunkGrid = ChunkGrid();

        InstantiateChunkGrid();
    }


    void InstantiateChunkGrid()
    {

        for (int x = 0; x < chunkGrid.Count; x++)
        {

            List<GameObject> gameObjectsGridCellList = new List<GameObject>();
            List<Vector3> vector3List = new List<Vector3>();

            for (int y = 0; y < chunkGrid[0].cells.Count; y++)
            {

                if (chunkGrid[x].cells[y].type == "Black")
                {
                    gameObjectsGridCellList.Add(Instantiate(gameObjectWall, new Vector3(chunkGrid[x].cells[y].pos.x, 0, chunkGrid[x].cells[y].pos.y), Quaternion.identity));
                    vector3List.Add(new Vector3(chunkGrid[x].cells[y].pos.x, 0, chunkGrid[x].cells[y].pos.y));
                }
            }

            GameObject chunk = Instantiate(gameObjectChunk, FindCenter(vector3List), Quaternion.identity);

            for (int i = 0; i < gameObjectsGridCellList.Count; i++)
            {
                gameObjectsGridCellList[i].transform.parent = chunk.transform;
            }

            gameObjectsGridCellList.Clear();
            vector3List.Clear();
            chunk.SetActive(false);
        }
    }

    public static Vector3 FindCenter(List<Vector3> positions)
    {
        if (positions == null || positions.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            sum += pos;
        }

        return sum / positions.Count;
    }

    public List<GridCellList> ChunkGrid()
    {

        List<GridCellList> gridCellLists = new List<GridCellList>();

        int chunkSize = 10;
        int gridWidth = grid.Count;
        int gridHeight = grid[0].cells.Count;

        for (int startX = 0; startX < gridWidth; startX += chunkSize)
        {
            for (int startY = 0; startY < gridHeight; startY += chunkSize)
            {
                GridCellList gridCells = new GridCellList();

                for (int x = startX; x < Math.Min(startX + chunkSize, gridWidth); x++)
                {
                    for (int y = startY; y < Math.Min(startY + chunkSize, gridHeight); y++)
                    {
                        gridCells.cells.Add(grid[x].cells[y]);
                    }
                }

                if (gridCells.cells.Count > 0)
                {
                    gridCellLists.Add(gridCells);
                }
            }
        }

        return gridCellLists;
    }

    public void MakeMapTexture()
    {
        for (int i = 0; i < 20; i++)
        {

            GenerateOval(Random.Range(50, 150), Random.Range(50, 150), new Vector2Int(Random.Range(200, 1000), Random.Range(200, 400)));

        }

        for (int i = 0; i < 20; i++)
        {

            GenerateOval(Random.Range(50, 200), Random.Range(50, 150), new Vector2Int(Random.Range(200, 1000), Random.Range(700, 1000)));

        }

        GenerateTexture();
    }

    public void ClearMap()
    {
        for (int x = 0; x < grid.Count; x++)
        {
            for (int y = 0; y < grid[0].cells.Count; y++)
            {
                grid[x].cells[y].type = "Empty";
            }
        }
    }


    void GenerateGrid(int size)
    {
        for (int x = 0; x < size; x++)
        {
            GridCellList cellsList = new GridCellList();
            for (int y = 0; y < size; y++)
            {
                cellsList.cells.Add(new GridCell(new Vector2Int(x, y), "Empty"));
            }
            grid.Add(cellsList);
        }
    }

    void GenerateTexture()
    {
        Texture2D texture = new Texture2D(grid.Count, grid[0].cells.Count);

        for (int x = 0; x < grid.Count; x++)
        {
            for (int y = 0; y < grid[0].cells.Count; y++)
            {

                if (grid[x].cells[y].type == "Black")
                {
                    texture.SetPixel(x, y, Color.black);
                }
                else
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
        }

        texture.Apply();

        string folderPath = Application.dataPath + "/Textures";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(folderPath + $"/{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png", bytes);
    }

    void GenerateOval(float inputX, float inputY, Vector2Int position)
    {
        float noiseOffsetX = Random.value * 1000;
        float noiseOffsetY = Random.value * 1000;

        for (int x = 0; x < grid.Count; x++)
        {
            for (int y = 0; y < grid[0].cells.Count; y++)
            {
                float nx = (x - position.x) / inputX;
                float ny = (y - position.y) / inputY;

                float xCoord = noiseOffsetX + (float)x / 256 * 5;
                float yCoord = noiseOffsetY + (float)y / 256 * 5;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                float noiseFactor = (sample - 0.5f) * 0.75f;
                float ovalEquation = (nx * nx) + (ny * ny) - noiseFactor;

                if (ovalEquation <= 1)
                    grid[x].cells[y].type = "Black";
            }
        }

    }
}

[Serializable]
public class GridCellList
{
    public List<GridCell> cells = new List<GridCell>();
}

[Serializable]
public class GridCell
{
    public Vector2Int pos;
    public string type; 

    public GridCell(Vector2Int _pos, string _type)
    {
        pos = _pos;
        type = _type;
    }
}

[CustomPropertyDrawer(typeof(GridCellList))]
public class GridCellListDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty listProperty = property.FindPropertyRelative("cells");
        return EditorGUI.GetPropertyHeight(listProperty);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty listProperty = property.FindPropertyRelative("cells");
        EditorGUI.PropertyField(position, listProperty, label, true);
    }
}
