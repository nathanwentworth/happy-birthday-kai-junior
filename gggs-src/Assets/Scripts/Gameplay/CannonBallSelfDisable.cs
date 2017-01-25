using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallSelfDisable : MonoBehaviour {

  private void OnCollisionEnter(Collision other) {
    StartCoroutine(Disable());
  }

  private IEnumerator Disable() {
    yield return new WaitForSeconds(3);
    gameObject.SetActive(false);
  }

}
