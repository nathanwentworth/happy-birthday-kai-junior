using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarJumpCameraFollow : MonoBehaviour {

  private Transform car;
  [Header("Should the camera follow the player?")]
  [SerializeField]
  private bool followPlayer;

  private void Awake() {
    car = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  private void Update() {
    if (!followPlayer) { return; }

    transform.LookAt(car);
  }

}
