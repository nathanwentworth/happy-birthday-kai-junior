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
    
  }



}
