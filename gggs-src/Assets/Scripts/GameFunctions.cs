using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFunctions : MonoBehaviour {

  private Controls controls;

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
    DataManager.GameOver = false;
    DataManager.Score = 0;
    DataManager.AllowControl = true;


    GameObject.Instantiate(weapons[(int)selectedWeapon]);
  }

	private void Update () {

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
