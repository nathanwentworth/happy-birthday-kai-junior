#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JointAutoCreate))]
public class JointAutoCreateEditor : Editor {
  public override void OnInspectorGUI() {
    DrawDefaultInspector();
    
    JointAutoCreate script = (JointAutoCreate)target;
    if(GUILayout.Button("Create Joints")) {
      // script.CreateJoints();
    }
  }
}

#endif