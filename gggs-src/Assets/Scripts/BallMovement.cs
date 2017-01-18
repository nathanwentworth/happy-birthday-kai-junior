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

  private Controls controls;

  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }


	private void Start () {
    player = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
	}
	
	private void Update () {
		dir = controls.Move;

	}

  private void FixedUpdate() {
    if (dir != Vector3.zero) {
      rb.AddTorque(dir.x * 100 * speed * -cam.transform.forward);
      rb.AddTorque(dir.y * 100 * speed * cam.transform.right);    
    }
  }
}
