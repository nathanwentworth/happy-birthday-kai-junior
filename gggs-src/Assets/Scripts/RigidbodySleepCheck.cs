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
	
	private void Update () {
		if (!rb.IsSleeping() && !knockedOver && rb.velocity.magnitude > 2) {
      print(rb.transform.name + " isn't sleeping!");
      knockedOver = true;
      DataManager.Points += points;
      hudManager.ScoreChange();
    }
	}
}
