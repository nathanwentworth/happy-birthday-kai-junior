using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJumpCameraFollow : MonoBehaviour {

  private Transform car;

  private void Awake() {
    car = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  private void Update() {
    transform.LookAt(car);
  }

}
