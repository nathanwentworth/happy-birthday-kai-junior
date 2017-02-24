using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour {

  [MenuItem ("Kaiju/Make Destructible")]
  static void MakeObjectDestructible () {
    for (int i = 0; i < Selection.gameObjects.Length; i++) {
      Selection.gameObjects[i].AddComponent<BoxCollider>();
      Selection.gameObjects[i].AddComponent<Rigidbody>();
      Selection.gameObjects[i].AddComponent<RigidbodySleepCheck>();
      Selection.gameObjects[i].AddComponent<ObjectDataContainer>();
    }
  }

  
  [MenuItem ("Kaiju/Initialize Scene")]
  static void InitializeScene () {
    if (GameObject.Find("Main Camera") != null) {
      Object.DestroyImmediate(GameObject.Find("Main Camera"));
    }



    if (GameObject.Find("CameraFollow") == null) {
      Object cameraFollow = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/CameraFollow.prefab", typeof(GameObject));
      GameObject _cameraFollow = PrefabUtility.InstantiatePrefab(cameraFollow) as GameObject;
      _cameraFollow.name = "CameraFollow";
    }

    if (GameObject.Find("MainHUD") == null) {
      Object hud = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/MainHUD.prefab", typeof(GameObject));
      GameObject _hud = PrefabUtility.InstantiatePrefab(hud) as GameObject;
      _hud.name = "MainHUD";
    }

    if (GameObject.Find("Ball") == null) {
      Object ball = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Ball.prefab", typeof(GameObject));
      GameObject _ball = PrefabUtility.InstantiatePrefab(ball) as GameObject;
      _ball.name = "Ball";
    }

    if (GameObject.Find("GameManagerSingleton") == null) {
      Object gameManager = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/GameManagerSingleton.prefab", typeof(GameObject));
      GameObject _gameManager = PrefabUtility.InstantiatePrefab(gameManager) as GameObject;
      _gameManager.name = "GameManagerSingleton";
    }

    if (GameObject.Find("InControl") == null) {
      Object inControl = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Utility/InControl.prefab", typeof(GameObject));
      GameObject _inControl = PrefabUtility.InstantiatePrefab(inControl) as GameObject;
      _inControl.name = "InControl";
    }


  }



}
