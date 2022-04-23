using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConnectionPrefabManager))]
public class ConnectionPrefabManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = target as ConnectionPrefabManager;
        if (t == null)
            return;

        t.From.position = Handles.DoPositionHandle(t.From.position, Quaternion.identity);
        t.To.position = Handles.DoPositionHandle(t.To.position, Quaternion.identity);
    }
}