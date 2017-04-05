using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

  private Transform target;
  private Transform root;

  [SerializeField]
  private float followSpeed = 3f;
  [SerializeField]
  private float rotateSpeed = 3f;
  [SerializeField]
  private float distance = 3f;

  [SerializeField]
  private float minLookX;
  [SerializeField]
  private float maxLookX;

  private Quaternion startingRotation;

  private Controls controls;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    root = transform.root;
    startingRotation = target.rotation;
    transform.rotation = startingRotation;
  }

  private void FixedUpdate() {
    if (!target) return;

    RootTransform();
    Follow();
  }

  private void RootTransform() {
    root.position = target.position;
  }

  private void Follow() {

    float wantedRotationAngleY = root.eulerAngles.y;
    float wantedRotationAngleX = root.eulerAngles.x;

    float currentRotationAngleY = root.eulerAngles.y;
    float currentRotationAngleX = root.eulerAngles.x;


    wantedRotationAngleY = root.eulerAngles.y;
    wantedRotationAngleY += controls.Look.X * 100 * rotateSpeed * Time.deltaTime;

    wantedRotationAngleX = root.eulerAngles.x;
    wantedRotationAngleX += controls.Look.Y * 100 * -rotateSpeed * Time.deltaTime;

    if (wantedRotationAngleX > 180) {
      wantedRotationAngleX = wantedRotationAngleX - 360;
    }

    if (wantedRotationAngleX >= maxLookX) {
      wantedRotationAngleX = maxLookX;
    } else if (wantedRotationAngleX <= minLookX) {
      wantedRotationAngleX = minLookX;
    }

    var currentRotation = Quaternion.Euler(wantedRotationAngleX, wantedRotationAngleY, 0);
    root.rotation = currentRotation;


    // transform.rotation = new Vector3(
    //   // target.trans
    // );
    transform.position = root.rotation * Vector3.forward + root.position;

    transform.position += root.transform.forward * -distance;

  }

}
