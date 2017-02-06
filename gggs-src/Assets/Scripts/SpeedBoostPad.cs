using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPad : MonoBehaviour {

  [SerializeField]
  private float boostForce;

  private void OnTriggerEnter(Collider other) {

    if (other.gameObject.GetComponent<Rigidbody>() != null) {
      Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

      rb.AddForce(transform.up * boostForce, ForceMode.VelocityChange);

      Debug.Log("boosted!");
    }

  }

}
