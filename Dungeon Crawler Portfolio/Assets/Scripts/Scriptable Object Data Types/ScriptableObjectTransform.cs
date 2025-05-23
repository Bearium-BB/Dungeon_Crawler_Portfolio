using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Transform", menuName = "Data Types/Transform ", order = 3)]
public class ScriptableObjectTransform : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;

    private void OnEnable()
    {
        position = Vector3.zero;
    }
    private void OnDisable()
    {
        rotation = Quaternion.identity;
    }
}
