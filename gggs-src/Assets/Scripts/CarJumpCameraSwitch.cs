﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJumpCameraSwitch : MonoBehaviour {

  [SerializeField]
  private Camera jumpCam;
  private Camera carCam;
  private Timer timer;

  private void Awake() {
    carCam = GameObject.Find("CarCamera").GetComponent<Camera>();
    timer = FindObjectOfType (typeof (Timer)) as Timer;
  }

  private void OnTriggerEnter(Collider other) {
    carCam.enabled = false;
    jumpCam.enabled = true;
    StartCoroutine(timer.GameOverDelay(6));
  }

}