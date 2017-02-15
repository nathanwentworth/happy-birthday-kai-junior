using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {
	
  private void OnTriggerEnter(Collider other) {

    if (other.gameObject.tag == "Player") {
      Vector3 respawnPoint = other.GetComponent<CarControl>().defaultRespawnPoint;
      Quaternion respawnDirection = other.GetComponent<CarControl>().defaultRespawnDirection;
      other.transform.position = respawnPoint;
      other.transform.rotation = respawnDirection;
      other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

  }

}
