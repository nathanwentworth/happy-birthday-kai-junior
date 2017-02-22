using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour {

  [SerializeField]
  private Text otherButtonText;
  [SerializeField]
  private Button otherButton;
  [SerializeField]
  private InputField otherText;

  private void Update() {
    otherButtonText.text = "Load " + otherText.text;
  }

  public void LoadOther() {
    string scene = otherText.text;
    LoadScene(scene);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
