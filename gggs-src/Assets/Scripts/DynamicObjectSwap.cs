using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectSwap : MonoBehaviour {

  [SerializeField]
  private GameObject objectToSwap;

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.tag == "Player") {
      Instantiate(objectToSwap, transform.position, transform.rotation);
      gameObject.SetActive(false);
    }
  }

}
