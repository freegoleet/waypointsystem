
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Waypoints))]
[CanEditMultipleObjects]
public class WaypointsEditor : Editor
{
    SerializedProperty waypoints;
    SerializedProperty waypoint;

    public void OnEnable()
    {
        waypoints = serializedObject.FindProperty("waypoints");
        waypoint = serializedObject.FindProperty("waypoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(waypoints);
        Waypoints waypointsRef = (Waypoints)target;
        EditorGUILayout.PropertyField(waypoint);

        waypointsRef.speed = EditorGUILayout.Slider("Speed", waypointsRef.speed, 0, 100);

        if (GUILayout.Button("Add Waypoint"))
        {
            waypointsRef.AddNewWaypoint();
        }
        

        if (GUI.changed && !Application.isPlaying)
        {
            EditorUtility.SetDirty(waypointsRef);
            EditorSceneManager.MarkSceneDirty(waypointsRef.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {

        for (int i = 0; i < waypoints.arraySize; i++)
        {
            var t = (target as Waypoints);
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Handles.PositionHandle(t.waypoints[i].transform.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterFullObjectHierarchyUndo(t.waypoints[i], "Move point");
                if (pos != null)
                {
                    t.waypoints[i].transform.position = pos;
                    t.Update();
                }
            }
        }
    }
}
