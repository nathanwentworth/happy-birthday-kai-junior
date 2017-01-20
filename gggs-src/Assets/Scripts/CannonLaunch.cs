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

		if (controls.Jump.WasPressed) {
      cannonBallInst.GetComponent<Rigidbody>().velocity = Vector3.zero;
      cannonBallInst.transform.position = spawnPoint.transform.position;
      cannonBallInst.transform.rotation = spawnPoint.transform.rotation;
      cannonBallInst.SetActive(true);
      cannonBallInst.GetComponent<Rigidbody>().AddForce(-Vector3.forward * 1000);
    }
	}

  private void FixedUpdate() {
    gameObject.transform.Rotate(0, dir.x * Time.deltaTime * 10, dir.y * Time.deltaTime * 10);
  }
}
