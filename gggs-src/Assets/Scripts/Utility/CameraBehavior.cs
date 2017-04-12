using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

  private Transform target;
  private Transform root;

  [SerializeField]
  private float rotateSpeed = 3f;
  [SerializeField]
  private float minDistance = 10f;
  [SerializeField]
  private float maxDistance = 25f;
  [SerializeField]
  private float zoomDistance = 50f;
  private float activeDistance;
  private float countDownTime;
  private bool doneZooming;

  [SerializeField]
  private float minLookX;
  [SerializeField]
  private float maxLookX;

  [SerializeField]
  private float zRotation = 10f;

  private float distanceNoise = 0.25f;
  [SerializeField]
  private float distanceNoiseRate;

  private Controls controls;
  private BallMovement ballMovement;
  private LevelDataContainer levelData;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    levelData = GameObject.Find("LevelData").GetComponent<LevelDataContainer>();
    countDownTime = levelData.CountDownTime;
    ballMovement = target.GetComponent<BallMovement>();
    root = transform.root;
    root.rotation = Quaternion.Euler(new Vector3(30, root.rotation.y, 0));
  }

  private void Start() {
    if (countDownTime > 0) {
      doneZooming = false;
      StartCoroutine(ZoomIn());
    } else {
      doneZooming = true;
    }
  }

  private void FixedUpdate() {
    if (!target) return;

    root.position = target.position;
    Rotate();
    Follow();
  }

  private void Follow() {
    float _distanceNoise = Mathf.PerlinNoise(distanceNoise += 0.01f, distanceNoise += 0.01f);
    _distanceNoise *= Time.deltaTime * distanceNoiseRate;
    // activeDistance = (distance * (ballMovement.currentSpeed / 110)) + (10 + _distanceNoise);
    if (doneZooming) {
      activeDistance = (minDistance + _distanceNoise) + ((ballMovement.currentSpeed / 110) * (maxDistance - minDistance));
    }

    Vector3 pos = root.rotation * Vector3.forward + root.position;
    pos += root.transform.forward * -activeDistance;

    transform.position = pos;
  }

  private void Rotate() {
    float currentRotationAngleY = root.eulerAngles.y;
    float currentRotationAngleX = root.eulerAngles.x;
    float currentRotationAngleZ = root.eulerAngles.z;

    float wantedRotationAngleY = root.eulerAngles.y
    + controls.Look.X * 100 * rotateSpeed * Time.deltaTime;
    float wantedRotationAngleX = root.eulerAngles.x
    + controls.Look.Y * 100 * -rotateSpeed * Time.deltaTime;
    float wantedRotationAngleZ = 0
    + controls.Look.X * zRotation * -(rotateSpeed * 0.8f) * Time.deltaTime;

    if (wantedRotationAngleX > 180) {
      wantedRotationAngleX = wantedRotationAngleX - 360;
    }

    if (wantedRotationAngleX >= maxLookX) {
      wantedRotationAngleX = maxLookX;
    } else if (wantedRotationAngleX <= minLookX) {
      wantedRotationAngleX = minLookX;
    }

    wantedRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotateSpeed * Time.deltaTime);
    wantedRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotateSpeed * Time.deltaTime);
    wantedRotationAngleZ = Mathf.LerpAngle(currentRotationAngleZ, wantedRotationAngleZ, rotateSpeed * Time.deltaTime);

    var currentRotation = Quaternion.Euler(wantedRotationAngleX, wantedRotationAngleY, wantedRotationAngleZ);
    root.rotation = currentRotation;
  }

  private IEnumerator ZoomIn() {
    float startTime = countDownTime;
    float lerpTime = 0;
    float t = 0;
    while (lerpTime < startTime) {
      t = (lerpTime / startTime);
      t = Mathf.Sin(t * Mathf.PI * 0.5f);
      activeDistance = Mathf.Lerp(zoomDistance, minDistance, t);

      lerpTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    doneZooming = true;
    yield return null;
  }

}
