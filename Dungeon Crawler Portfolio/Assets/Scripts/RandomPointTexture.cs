using UnityEngine;

public class RandomPointTexture : MonoBehaviour
{
    public Texture2D texture; // Assign this in the Unity Inspector
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();

        // Define a simple quad mesh
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), // Bottom-left
            new Vector3(1, 0, 0), // Bottom-right
            new Vector3(0, 1, 0), // Top-left
            new Vector3(1, 1, 0)  // Top-right
        };

        int[] triangles = new int[]
        {
            0, 2, 1, // First triangle
            2, 3, 1  // Second triangle
        };

        // Initial UVs (full texture on the mesh)
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0), // Bottom-left
            new Vector2(1, 0), // Bottom-right
            new Vector2(0, 1), // Top-left
            new Vector2(1, 1)  // Top-right
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Apply the texture
        meshRenderer.material.mainTexture = texture;

        // Select a random point and update the UV mapping
        AssignTextureToRandomPoint();
    }

    void AssignTextureToRandomPoint()
    {
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;

        // Get a random vertex on the mesh
        int randomIndex = Random.Range(0, vertices.Length);
        Vector3 randomPoint = vertices[randomIndex];

        Debug.Log("Random Point: " + randomPoint);

        // Set the UVs to highlight the random point with the texture
        for (int i = 0; i < uvs.Length; i++)
        {
            if (i == randomIndex)
            {
                uvs[i] = new Vector2(0.5f, 0.5f); // Center of the texture
            }
            else
            {
                uvs[i] = new Vector2(0, 0); // Rest of the mesh to the bottom-left of the texture
            }
        }

        mesh.uv = uvs;
    }
}
