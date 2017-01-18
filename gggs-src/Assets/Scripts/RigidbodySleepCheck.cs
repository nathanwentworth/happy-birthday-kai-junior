using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleepCheck : MonoBehaviour {

  private Rigidbody rb;
  private bool knockedOver;
  [SerializeField]
  private int points;
  private HUDManager hudManager;

	private void Start () {
    knockedOver = false;
		rb = GetComponent<Rigidbody>();
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
	}
	
	private void OnCollisionStay (Collision other) {
    if (other.gameObject.GetComponent<Rigidbody>() != null) {
  		if (!rb.IsSleeping() && !knockedOver && rb.velocity.magnitude > 2) {
        Debug.Log(other.gameObject.name);
        knockedOver = true;
        DataManager.Points += points;
        hudManager.ScoreChange();
      }      
    }
	}

}
