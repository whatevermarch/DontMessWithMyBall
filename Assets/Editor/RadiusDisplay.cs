using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MineralController))]
public class RadiusDisplay : Editor {

    void OnSceneGUI() {
        MineralController fov = (MineralController)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc (fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets) {
            Handles.DrawLine (fov.transform.position, visibleTarget.position);
        }
    }
}
