using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJumpCameraSwitch : MonoBehaviour {

  private Camera jumpCam;
  private Camera carCam;
  private Timer timer;

  [Header("Should the Game Over panel be displayed when entered?")]
  [SerializeField]
  private bool gameOver;
  
  private void Awake() {
    carCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    jumpCam = transform.GetChild(0).GetComponent<Camera>();
    timer = FindObjectOfType (typeof (Timer)) as Timer;

    carCam.enabled = true;
    jumpCam.enabled = false;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Player") {
      carCam.enabled = false;
      jumpCam.enabled = true;
      if (gameOver) {
        StartCoroutine(timer.GameOverDelay(3));
      }
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.gameObject.tag == "Player") {
      carCam.enabled = true;
      jumpCam.enabled = false;
    }
  }

}
