using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyAddOnCollision : MonoBehaviour {

  private Rigidbody rb;

  [SerializeField]
  private float hitForce;

	private void Start () {
    if (GetComponent<Rigidbody>() == null) {
      rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
    } else {
		  rb = GetComponent<Rigidbody>();
    }
    rb.isKinematic = true;
	}

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.tag == "Player") {
      if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > hitForce) {
        if (rb.isKinematic) {
          rb.isKinematic = false;
        }        
      }
    }
  }

}
