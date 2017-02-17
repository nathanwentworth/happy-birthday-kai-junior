using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class WaypointRespawningController : MonoBehaviour {

  public Vector3 defaultRespawnPoint { get; private set; }
  public Quaternion defaultRespawnDirection { get; private set; }

  private Vector3 respawnPoint;
  private Quaternion respawnDirection;

  private Controls controls;

  private Rigidbody rigid;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

	private void Start() {
    rigid = GetComponent<Rigidbody>();
    defaultRespawnPoint = transform.position;
		
	}
	
	private void Update() {

    if (controls.SetRespawn.WasPressed) {
      SetRespawnPoint();
    }

    if (controls.GoToRespawn.WasPressed) {
      RespawnAtSetPoint();
    }

	}

  private void SetRespawnPoint() {
    respawnPoint = transform.position;
    respawnDirection = transform.rotation;
  }

  private void RespawnAtSetPoint() {
    transform.position = respawnPoint;
    transform.rotation = respawnDirection;
    rigid.velocity = Vector3.zero;
  }

}
