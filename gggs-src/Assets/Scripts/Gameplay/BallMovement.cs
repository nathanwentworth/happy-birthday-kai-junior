using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using InControl;

public class BallMovement : MonoBehaviour {

  private Rigidbody rb;

  private Transform cam;
  [SerializeField]
  private float airSpeed;
  [SerializeField]
  private float groundedSpeed;
  private bool grounded;
  private float checkHeight;
  [SerializeField]
  private bool ai = false;
  private float x1, x2, y1, y2;

  public float currentSpeed { get; private set; }

  private Controls controls;

  private Vector3 dir;

  private void OnEnable() {
    if (!ai) {
      controls = Controls.DefaultBindings();
    }
  }

  private void Awake() {
    if (!ai) {
      cam = GameObject.Find("CameraFollow").transform.GetChild(0);
    } else {
      cam = Camera.current.gameObject.transform;
    }
  }

	private void Start() {
    if (ai) {
      x1 = y1 = 0.0f;
      x2 = y2 = 0.5f;
    }

		rb = GetComponent<Rigidbody>();

    checkHeight = (transform.localScale.y / 2) + 1;

    DataManager.StartingPosition = transform.position;
    DataManager.StartingRotation = transform.rotation;
	}

	private void Update() {
    if (!ai) {
		  dir = controls.Move;
    } else {
      dir = new Vector2(Mathf.PerlinNoise(x1 += 0.01f, y1 += 0.01f), Mathf.PerlinNoise(x2 += 0.01f, y2 += 0.01f));
    }
  }

  private void FixedUpdate() {
    grounded = Physics.Raycast(transform.position, -Vector3.up, checkHeight);

    Quaternion _cam = cam.transform.rotation;
    _cam.eulerAngles = new Vector3 ( 0, _cam.eulerAngles.y, _cam.eulerAngles.z );

    if (dir != Vector3.zero && DataManager.AllowControl) {

      float speed = (grounded) ? groundedSpeed : airSpeed;

      rb.AddForce(dir.x * speed * (_cam * Vector3.right));
      rb.AddForce(dir.y * speed * (_cam * Vector3.forward));

    }
  }
}
