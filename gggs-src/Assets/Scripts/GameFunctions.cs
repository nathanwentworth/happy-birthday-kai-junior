using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFunctions : MonoBehaviour {

  private Controls controls;
  private HUDManager hudManager;

  [SerializeField]
  private Weapon selectedWeapon;

  [SerializeField]
  private GameObject[] weapons;

  private enum Weapon {
    Ball,
    Cannon
  }

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
    DataManager.GameOver = false;
    DataManager.Score = 0;


    GameObject.Instantiate(weapons[(int)selectedWeapon]);
  }

	private void Update () {

    if (controls.Interact.WasPressed) {
      DataManager.ResetHighScore();
    }
    if (controls.Pause.WasPressed) {
      Pause();
    }
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
