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
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.black;
        DataManager.Score += points;
        DataManager.CumulativeScore += points;
        hudManager.ScoreChange();
        hudManager.CumulativeScoreChange();

        if (DataManager.Score >= DataManager.HighScore) {
          DataManager.NewHighScore = true;
          hudManager.HighScoreChange();
        }

        this.enabled = false;
      }
    }
	}

}
