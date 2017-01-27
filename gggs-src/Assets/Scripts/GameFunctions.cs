using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFunctions : MonoBehaviour {

  private Controls controls;
  private static HUDManager hudManager;

  // [SerializeField]
  private Weapon selectedWeapon;

  [SerializeField]
  private GameObject[] weapons;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    if (hudManager == null) {
      // @REFACTOR
      // potentially slow. FindObjectOfType is slower than GameObject.Find()
      hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
    }

    Debug.Log("timescale is now " + Time.timeScale);
    if (DataManager.Paused) {
      Pause();
      Debug.Log("was paused, now it's not!");
    } else {
      Debug.Log("not paused on start!");
    }
    DataManager.GameOver = false;
    DataManager.Score = 0;
    selectedWeapon = DataManager.SelectedWeapon;

    if (SceneManager.GetActiveScene().name.StartsWith("main-test")) {
      GameObject.Instantiate(weapons[(int)selectedWeapon]);
      
    }


  }

	private void Update () {

    // @DEBUG
    if (controls.Interact.WasPressed) {
      DataManager.ResetHighScore();
    }
    if (controls.Pause.WasPressed) {
      Pause();
      
    }
    // @DEBUG
    if (controls.Confirm.WasPressed && DataManager.GameOver) {
      DataManager.GameOver = false;
      Debug.Log("Reloading scene: " + SceneManager.GetActiveScene().name);
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
	}

  public void Pause() {
    DataManager.Paused = !DataManager.Paused;
    bool paused = DataManager.Paused;
    Time.timeScale = (paused) ? 0.000001f : 1f;
    hudManager.PausePanelDisplay(paused);
  }


}
