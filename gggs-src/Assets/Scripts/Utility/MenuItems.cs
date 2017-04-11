#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MenuItems : MonoBehaviour {

  [MenuItem ("Kaiju/Make Destructible")]
  static void MakeObjectDestructible () {
    for (int i = 0; i < Selection.gameObjects.Length; i++) {
      GameObject go = Selection.gameObjects[i];

      if (go.GetComponent<BoxCollider>() == null && go.GetComponent<MeshCollider>() == null && go.GetComponent<SphereCollider>() == null && go.GetComponent<CapsuleCollider>() == null) {
        go.AddComponent<BoxCollider>();
      }
      if (go.GetComponent<Rigidbody>() == null) {
        go.AddComponent<Rigidbody>();
      }
      if (go.GetComponent<RigidbodySleepCheck>() == null) {
        go.AddComponent<RigidbodySleepCheck>();
      }
      if (go.GetComponent<ObjectDataContainer>() == null) {
        go.AddComponent<ObjectDataContainer>();
      }
      if (go.GetComponent<ObjectBehaviors>() == null) {
        go.AddComponent<ObjectBehaviors>();
      }

    }
  }

  [MenuItem ("Kaiju/Add ObjectBehaviors")]
  static void AddObjectBehaviors() {
    for (int i = 0; i < Selection.gameObjects.Length; i++) {
      GameObject go = Selection.gameObjects[i];

      if (go.GetComponent<ObjectBehaviors>() == null) {
        go.AddComponent<ObjectBehaviors>();
      }

    }
  }

  
  [MenuItem ("Kaiju/Initialize Scene")]
  static void InitializeScene () {
    // delete default main camera
    GameObject go;
    Object o;
    if ((go = GameObject.Find("Main Camera")) != null && go.transform.parent == null) {
      Object.DestroyImmediate(go);
    }

    // add the smooth camera follow
    if (GameObject.Find("CameraFollow") == null) {
      o = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/CameraFollow.prefab", typeof(GameObject));
      GameObject _go = PrefabUtility.InstantiatePrefab(o) as GameObject;
      _go.name = "CameraFollow";
    }

    // add the main hud
    if (GameObject.Find("MainHUD") == null) {
      o = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/MainHUD.prefab", typeof(GameObject));
      GameObject _go = PrefabUtility.InstantiatePrefab(o) as GameObject;
      _go.name = "MainHUD";
    }

    // add an eventsystem
    if (GameObject.Find("EventSystem") == null) {
      go = new GameObject();
      go.AddComponent<UnityEngine.EventSystems.EventSystem>();
      go.AddComponent<InControl.InControlInputModule>();
      go.name = "EventSystem";
    }

    // add the level data object
    if (GameObject.Find("LevelData") == null) {
      go = new GameObject();
      go.AddComponent<LevelDataContainer>();
      go.name = "LevelData";
    }

    // add the ball
    if (GameObject.Find("Ball") == null) {
      o = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ball.prefab", typeof(GameObject));
      GameObject _go = PrefabUtility.InstantiatePrefab(o) as GameObject;
      _go.name = "Ball";
    }

    // add the game manager
    if (GameObject.Find("GameManagerSingleton") == null) {
      o = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/GameManagerSingleton.prefab", typeof(GameObject));
      GameObject _go = PrefabUtility.InstantiatePrefab(o) as GameObject;
      _go.name = "GameManagerSingleton";
    }

    // add incontrol
    if (GameObject.Find("InControl") == null) {
      o = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/InControl.prefab", typeof(GameObject));
      GameObject _go = PrefabUtility.InstantiatePrefab(o) as GameObject;
      _go.name = "InControl";
    }

    // add a floor
    if (GameObject.Find("Floor") == null) {
      go = GameObject.CreatePrimitive(PrimitiveType.Cube);
      go.name = "Floor";
      go.transform.localScale = new Vector3(200f, 1f, 200f);
      go.transform.position = new Vector3(0f, -0.5f, 0f);
    }


  }

}


#endif