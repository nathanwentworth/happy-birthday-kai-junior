using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatPersonLookAt : MonoBehaviour {

  private Transform transform;
  private Transform camTransform;

	private void Start () {
    transform = GetComponent<Transform>();
    camTransform = GameObject.Find("Cam").GetComponent<Transform>();
	}

	private void Update () {
    transform.LookAt(camTransform.position);
	}
}
