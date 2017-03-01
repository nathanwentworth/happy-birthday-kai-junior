using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {
	
  private void OnTriggerEnter(Collider other) {

    if (other.gameObject.tag == "Player") {

      Vector3 respawnPoint = other.GetComponent<WaypointRespawningController>().defaultRespawnPoint;
      Quaternion respawnDirection = other.GetComponent<WaypointRespawningController>().defaultRespawnDirection;
      other.transform.position = respawnPoint;
      other.transform.rotation = respawnDirection;
      other.GetComponent<Rigidbody>().velocity = Vector3.zero;

    } else {
      other.gameObject.SetActive(false);
    }

  }

}
