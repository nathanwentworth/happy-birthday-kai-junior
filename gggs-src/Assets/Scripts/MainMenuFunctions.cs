using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour {

  private void Start() {
    Screen.lockCursor = false;
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
