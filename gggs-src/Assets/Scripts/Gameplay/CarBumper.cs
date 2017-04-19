using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBumper : MonoBehaviour {

  private CarMovement carMovement;

  private void Start() {
    carMovement = transform.parent.GetComponent<CarMovement>();
    carMovement.carInBumper = 1;
  }

  private void OnTriggerEnter(Collider other) {
    // there is a car in the bumper, so stop accelerating
    carMovement.carInBumper = 0;
  }

  private void OnTriggerExit(Collider other) {
    // there is NOT a car in the bumper, so accelerate
    carMovement.carInBumper = 1;
  }

}
