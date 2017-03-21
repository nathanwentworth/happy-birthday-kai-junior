using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectSwap : MonoBehaviour {

  [SerializeField]
  private GameObject objectToSwapPrefab;
  private GameObject objectToSwapScene;

  private void Awake() {

    objectToSwapScene = Instantiate(objectToSwapPrefab, transform.position, transform.rotation);
    objectToSwapScene.SetActive(false);

  }

  private void OnTriggerEnter(Collider other) {

    if (other.gameObject.GetComponent<Rigidbody>() != null) {
      if (other.gameObject.tag == "Player" || other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 2) {
        SwapModel();
      }
    }


  }

  private void SwapModel() {
    gameObject.SetActive(false);
    objectToSwapScene.SetActive(true);
  }

}
