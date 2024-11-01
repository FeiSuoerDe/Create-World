using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldTileMap))]
public class WorldTileMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("生成地图"))
        {
            ((WorldTileMap)target).MakeMap();
        }
        if (GUILayout.Button("清除地图"))
        {
            ((WorldTileMap)target).ClearMap();
        }
    }
}