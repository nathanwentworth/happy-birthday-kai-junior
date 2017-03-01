using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleepCheck : MonoBehaviour {

  private Rigidbody rb;
  private bool knockedOver;
  private int points;
  private HUDManager hudManager;
  private float threshold;

	private void Start () {
    if (DataManager.ObjectMovementThreshold == 0) {
      DataManager.ObjectMovementThreshold = 1;
    }
    threshold = DataManager.ObjectMovementThreshold;
    points = GetComponent<ObjectDataContainer>().ObjectPoints;
    knockedOver = false;
		rb = GetComponent<Rigidbody>();
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
	}
	
	private void OnCollisionStay (Collision other) {
    if (other.gameObject.GetComponent<Rigidbody>() != null) {
      if (!knockedOver) {
    		if (rb.velocity.magnitude > 2) {
          knockedOver = true;
          Renderer rend = GetComponent<Renderer>();
          rend.material.color = Color.black;

          int _points = points;

          // _points *= DataManager.Combo;

          DataManager.Score += _points;
          DataManager.CumulativeScore += _points;

          hudManager.ScoreChange();
          // hudManager.CumulativeScoreChange();

          if (DataManager.Score > DataManager.HighScore) {

            DataManager.HighScore = DataManager.Score;

            DataManager.NewHighScore = true;
            hudManager.HighScoreChange();
          }

          StartCoroutine(CheckMoveState());

        }
      }
    }
	}

  // private void OnCollisionEnter(Collision other) {
  //   if (rb.velocity.magnitude > 15) {
  //     gameObject.SetActive(false);
  //   }
  // }

  // @REFACTOR: this whole script can be done betttttttttter

  private IEnumerator CheckMoveState() {
    while (rb.velocity.magnitude > threshold) {
      DataManager.ObjectIsStillMoving = true;
      Debug.Log(gameObject.name + " is still moving, speed is: " + rb.velocity.magnitude);
      yield return null;
    }

    DataManager.ObjectIsStillMoving = false;

    this.enabled = false;
  }

}
