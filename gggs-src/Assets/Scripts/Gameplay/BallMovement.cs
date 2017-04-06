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

  public float currentSpeed { get; private set; }

  private Controls controls;
  private HUDManager hudManager;

  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
    cam = GameObject.Find("CameraFollow").transform.GetChild(0);
  }


	private void Start() {
		rb = GetComponent<Rigidbody>();

    DataManager.StartingPosition = transform.position;
    DataManager.StartingRotation = transform.rotation;

	}
	
	private void Update() {
		dir = controls.Move;

    grounded = Physics.Raycast(transform.position, -Vector3.up, 6);

    currentSpeed = rb.velocity.magnitude;
    hudManager.SpeedometerDisplay(currentSpeed);
	}

  public void Restart() {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  private void FixedUpdate() {

    Quaternion _cam = cam.transform.rotation;

    _cam.eulerAngles = new Vector3 ( 0, _cam.eulerAngles.y, _cam.eulerAngles.z );


    if (dir != Vector3.zero && DataManager.AllowControl) {

      float speed = (grounded) ? groundedSpeed : airSpeed;

      // rb.AddForce(dir.x * speed * _cam.transform.right);
      // rb.AddForce(dir.y * speed * _cam.transform.forward);

      rb.AddForce(dir.x * speed * (_cam * Vector3.right));
      rb.AddForce(dir.y * speed * (_cam * Vector3.forward));

      // forward needs a flat thing! child an object under the camera that is always 0,0,0 for rotation
    }
  }
}
