using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor{
    private void OnSceneGUI()
    {
        var spawner = target as Spawner;
        Color color;
        ColorUtility.TryParseHtmlString("#2196F3", out color);
        color.a = 0.4f;
        Handles.color = color;

        Handles.DrawSolidArc(spawner.transform.position, Vector3.up, Vector3.right * spawner.Radius, 360, spawner.Radius);
    }
}
