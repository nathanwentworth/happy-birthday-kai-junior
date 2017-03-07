using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierWallToggle : MonoBehaviour {

  [SerializeField]
  private GameObject[] walls;

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Player") {
      for (int i = 0; i < walls.Length; i++) {
        walls[i].SetActive(false);
      }
    }
  }

}
