using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntCamFollow : MonoBehaviour {

  private Transform player;
  [Header("Should the camera follow the player?")]
  [SerializeField]
  private bool followPlayer;

  private void Awake() {
    player = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  private void Update() {
    if (!followPlayer) { return; }

    transform.LookAt(player);
  }

}
