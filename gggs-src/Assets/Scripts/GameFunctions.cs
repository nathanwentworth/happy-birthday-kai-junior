using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFunctions : MonoBehaviour {

  private Controls controls;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    DataManager.GameOver = false;
    DataManager.Score = 0;
  }

	private void Update () {

    Debug.Log("is the return key being pressed: " + controls.Confirm.WasPressed);

    if (controls.Interact.WasPressed) {
      DataManager.ResetHighScore();
    }
    if (controls.Confirm.WasPressed && DataManager.GameOver) {
      DataManager.GameOver = false;
      Debug.Log("Reloading scene: " + SceneManager.GetActiveScene().name);
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
	}
}
