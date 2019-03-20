﻿// Property drawer showing uneditable hex coordinates of a tile
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HexCoordinates coords = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue);

        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coords.ToString());
    }
}
