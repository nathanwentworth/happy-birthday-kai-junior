using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLaunch : MonoBehaviour {

  [SerializeField]
  private GameObject cannonBall;
  [SerializeField]
  private GameObject spawnPoint;

  private GameObject cannonBallInst;

  private Controls controls;


  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

	private void Start () {
		cannonBallInst = GameObject.Instantiate(cannonBall);
    cannonBallInst.SetActive(false);
	}
	
  private	void Update () {

    dir = controls.Move;

    if (controls.Jump.WasPressed && DataManager.AllowControl) {
      FireCannon();
    }

		
	}

  private void FixedUpdate() {
    if (DataManager.AllowControl) {
      gameObject.transform.Rotate(dir.y * Time.deltaTime * 10, dir.x * Time.deltaTime * 10, 0);
    }
  }

  private void FireCannon() {
    cannonBallInst.GetComponent<Rigidbody>().velocity = Vector3.zero;
    cannonBallInst.transform.position = spawnPoint.transform.position;
    cannonBallInst.transform.rotation = spawnPoint.transform.rotation;
    cannonBallInst.SetActive(true);
    cannonBallInst.GetComponent<Rigidbody>().AddForce(-cannonBallInst.transform.forward * 1000);
  }
}
