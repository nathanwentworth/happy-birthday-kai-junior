using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
    
  [HideInInspector]
  public Transform playerTarget;
  [HideInInspector]
  public Transform followTarget;
  private Rigidbody playerTargetRb;

  private Transform camera;

  public float speed = 3f;

  public float xOffset = 0f;
  public float yOffset = 0f;
  public float zOffset = -3f;
  public float rotationSpeed = 3.0f;
  private Vector3 playerTargetVector;

  [HideInInspector]
  public string playerTargetName;
  [HideInInspector]
  public Controls controls;
  private SmoothRotate smoothRotate;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    camera = transform.GetChild(0).GetComponent<Transform>();
    followTarget = transform.root;

    if (playerTarget == null) {
      StartCoroutine(FindPlayer());
    }

  }

  private void FixedUpdate() {
    if (!followTarget) return;
    else if (!playerTarget) return;

    Rotate();
    Follow();
  }

  private void Follow() {

    transform.position = new Vector3(
      Mathf.Lerp(transform.position.x, followTarget.transform.position.x + xOffset, Time.deltaTime * speed),
      Mathf.Lerp(transform.position.y, followTarget.transform.position.y + (yOffset + 4f), Time.deltaTime * speed),
      Mathf.Lerp(transform.position.z, followTarget.transform.position.z + zOffset, Time.deltaTime * speed)
    );

    transform.position += transform.rotation * Vector3.forward * zOffset;

  }

  public virtual void Rotate() {
    // overridden by SmoothRotate
  }

  private IEnumerator FindPlayer() {
    var checks = 0;
    while (playerTarget == null && checks < 10) {
      if (GameObject.FindWithTag("Player") != null) {
        playerTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerTargetRb = playerTarget.GetComponent<Rigidbody>();
        playerTargetName = playerTarget.name;    
      }

      yield return null;
    }

  }
}
