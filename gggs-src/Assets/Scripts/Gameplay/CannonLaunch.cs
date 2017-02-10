using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLaunch : MonoBehaviour {

  [SerializeField]
  private GameObject cannonBall;
  [SerializeField]
  private GameObject spawnPoint;
  [SerializeField]
  private int numBalls;
  [SerializeField]
  private int launchPower;

  private GameObject cannonBallInst;
  private List<GameObject> cannonBalls;
  private bool allowFire = true;

  private Controls controls;

  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {

    cannonBalls = new List<GameObject>();

    for (int i = 0; i < numBalls; i++) {
      cannonBallInst = GameObject.Instantiate(cannonBall);
      cannonBallInst.SetActive(false);
      cannonBalls.Add(cannonBallInst);
    }
  }

  private	void Update() {

    dir = controls.Move;

    if (controls.Jump.WasPressed && DataManager.AllowControl && allowFire) {
      FireCannon();
    }

		
	}

  private void FixedUpdate() {
    if (DataManager.AllowControl) {
      gameObject.transform.Rotate(dir.y * Time.deltaTime * 10, dir.x * Time.deltaTime * 10, 0);
    }
  }

  private void FireCannon() {
    for (int i = 0; i < cannonBalls.Count; i++) {
      if (!cannonBalls[i].activeInHierarchy) {
        cannonBalls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        cannonBalls[i].transform.position = spawnPoint.transform.position;
        cannonBalls[i].transform.rotation = spawnPoint.transform.rotation;
        cannonBalls[i].SetActive(true);
        cannonBalls[i].GetComponent<Rigidbody>().AddForce(-cannonBalls[i].transform.forward * launchPower, ForceMode.Impulse);

        allowFire = false;
        StartCoroutine(AllowFireTimer());

        return;
      }
    }

  }

  private IEnumerator AllowFireTimer() {
    yield return new WaitForSeconds(0.5f);
    allowFire = true;
  }
  
}
