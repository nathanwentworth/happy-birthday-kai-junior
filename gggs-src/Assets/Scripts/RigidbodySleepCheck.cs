using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleepCheck : MonoBehaviour {

  private Rigidbody rb;
  private bool knockedOver;
  private int points;
  private HUDManager hudManager;

	private void Start () {
    points = GetComponent<ObjectDataContainer>().ObjectPoints;
    knockedOver = false;
		rb = GetComponent<Rigidbody>();
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
	}
	
	private void OnCollisionStay (Collision other) {
    if (other.gameObject.GetComponent<Rigidbody>() != null) {
  		if (!rb.IsSleeping() && !knockedOver && rb.velocity.magnitude > 2) {
        knockedOver = true;
        DataManager.Score += points;
        hudManager.ScoreChange();

        if (DataManager.Score >= DataManager.HighScore) {
          print ("new high score!");
          DataManager.UpdateHighScore();
          hudManager.HighScoreChange();
        }

        this.enabled = false;
      }
    }
	}

}
