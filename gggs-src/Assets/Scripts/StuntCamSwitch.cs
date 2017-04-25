using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntCamSwitch : MonoBehaviour {

  private Camera jumpCam;
  private Camera mainCamera;
  private LevelDataContainer timer;

  [Header("Should the Game Over panel be displayed when entered?")]
  [SerializeField]
  private bool gameOver;

  private void Awake() {
    mainCamera = GameObject.Find("Cam").GetComponent<Camera>();
    jumpCam = transform.GetChild(0).GetComponent<Camera>();
    timer = FindObjectOfType (typeof (LevelDataContainer)) as LevelDataContainer;

    mainCamera.enabled = true;
    jumpCam.enabled = false;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Player") {
      mainCamera.enabled = false;
      jumpCam.enabled = true;
      if (gameOver) {
        StartCoroutine(timer.GameOverDelay(3));
        timer.runTimer = false;
      }
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.gameObject.tag == "Player") {
      mainCamera.enabled = true;
      jumpCam.enabled = false;
    }
  }

}
