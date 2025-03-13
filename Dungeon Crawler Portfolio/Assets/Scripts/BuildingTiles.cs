using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "BuildingTiles")]
public class BuildingTiles : ScriptableObject
{
    [SerializeField]
    public List<BuildingTile> buildingTiles;
}

[System.Serializable]
public class BuildingTile
{
    public int x;
    public int z;
    public GameObject building;
}


[CustomPropertyDrawer(typeof(BuildingTile))]
public class BuildingTileDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4 + 10; // Adjust height for multiple fields
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        Rect widthRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect heightRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        Rect buildingRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + 2) * 2, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(widthRect, property.FindPropertyRelative("x"));
        EditorGUI.PropertyField(heightRect, property.FindPropertyRelative("z"));
        EditorGUI.PropertyField(buildingRect, property.FindPropertyRelative("building"));

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}