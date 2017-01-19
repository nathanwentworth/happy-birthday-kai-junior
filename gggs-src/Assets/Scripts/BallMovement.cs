using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

  private Transform player;
  private Rigidbody rb;

  [SerializeField]
  private Transform cam;
  [SerializeField]
  private float speed;
  private bool allowControl;

  private Controls controls;

  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }


	private void Start () {
    player = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();

    allowControl = true;
	}
	
	private void Update () {
		dir = controls.Move;

	}

  private void FixedUpdate() {
    if (dir != Vector3.zero && allowControl) {
      rb.AddForce(dir.x * 1 * speed * cam.transform.right);
      rb.AddForce(dir.y * 1 * speed * cam.transform.forward);
    }
  }
}
