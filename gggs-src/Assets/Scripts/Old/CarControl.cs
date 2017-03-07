using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using System;

public class CarControl : MonoBehaviour {

  [Header("Wheel/Motor Variables")]
  public List<AxleInfo> axleInfos; // the information about each individual axle
  [SerializeField]
  [Range(500, 10000)]
  private float maxMotorTorque; // maximum torque the motor can apply to wheel
  [SerializeField]
  [Range(1, 30)]
  private float maxSteeringAngle; // maximum steer angle the wheel can have
  [SerializeField]
  [Range(500, 10000)]
  private float maxBrakingTorque; // how fast should you brake
  [SerializeField]
  [Range(0.01f, 1f)]
  private float controllerDeadzone = 0.15f;

  [Header("Air Control Variables")]
  [SerializeField]
  [Range(0.1f, 1f)]
  private float pitchMulti;
  [SerializeField]
  [Range(0.1f, 1f)]
  private float yawMulti;
  [SerializeField]
  [Range(0.1f, 1f)]
  private float rollMulti;
  [SerializeField]
  private float airControlForce;
  [SerializeField]
  [Range(1, 10)]
  private float autoRotationSpeed;
  [SerializeField]
  [Range(1, 100)]
  private float autoRotationCheckHeight;
  [SerializeField]
  [Range(0, 6)]
  private float autoRotationTimerDefault;

  // private variables

  private float accelerationForce = 0;
  private float brakingForce = 0;
  private Rigidbody rigid;
  private int mph;

  private Vector2 dir;
  private Controls controls;

  private Vector3 groundedVelocity;

  private HUDManager hudManager;

  public float MPH { get { return mph; } }

  private bool grounded;
  private bool groundedChange;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    // @REFACTOR
    // potentially slow
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
  }

  private void Start() {
    rigid = GetComponent<Rigidbody>();
  }

  private void Update() {
    dir = controls.Move;
    if (DataManager.AllowControl) {
      CarInput();
    }
  }

  private void FixedUpdate() {
    CarMotor();
  }

  private void CarInput() {
    // car controls
    // accelerationForce = dir.y;
    accelerationForce = (controls.Push.IsPressed) ? 1 : 0;
    // float pushForce = (controls.Push.WasPressed) ? 1 : 0;
    brakingForce = (controls.Brake.IsPressed) ? 1 : 0;

    if (!grounded) {
      Vector3 rotationalInput = new Vector3 (dir.y * pitchMulti, dir.x * yawMulti, -controls.Roll * rollMulti);
      rigid.AddRelativeTorque(rotationalInput * airControlForce);
    } else {
      // rigid.AddForce(pushForce * transform.forward * 10, ForceMode.VelocityChange);
    }

    if (mph < 8) {
      transform.Rotate(transform.up * dir.x * 1f);
    }

    // @DEBUG: hopefully won't need this when a real respawn thing is implemented
    if (controls.Reset.WasPressed) {
      transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
      transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 180, 0);
      Debug.Log("car reset!");
    }
  }

  private void CarMotor() {
    mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
    // Debug.Log("VRROM VROOOOOOM BITCH");
    float motor = maxMotorTorque * (accelerationForce * 3f);
    float steering = maxSteeringAngle * dir.x / ((200f - (mph * 0.75f)) / 200f);

    foreach (AxleInfo axleInfo in axleInfos) {

      if (axleInfo.steering) {
        axleInfo.leftWheel.steerAngle = steering;
        axleInfo.rightWheel.steerAngle = steering;
      }

      if (axleInfo.motor) {
        axleInfo.leftWheel.motorTorque = motor;
        axleInfo.rightWheel.motorTorque = motor;
      }

      axleInfo.leftWheel.brakeTorque = brakingForce * maxBrakingTorque;
      axleInfo.rightWheel.brakeTorque = brakingForce * maxBrakingTorque;
      ApplyLocalPositionToVisuals(axleInfo.leftWheel);
      ApplyLocalPositionToVisuals(axleInfo.rightWheel);
      IsGrounded(axleInfo.rightWheel, axleInfo.leftWheel);
    }

  }

  private void IsGrounded(WheelCollider right, WheelCollider left) {
    bool _grounded = (right.isGrounded && left.isGrounded);

    if (_grounded != grounded) {
      grounded = _grounded;
      DataManager.Grounded = _grounded;
      groundedChange = true;
    } else {
      groundedChange = false;
    }
  }

  public void ApplyLocalPositionToVisuals(WheelCollider collider) {
    if (collider.transform.childCount == 0) {
      return;
    }

    Transform visualWheel = collider.transform.GetChild(0);

    Vector3 position;
    Quaternion rotation;
    collider.GetWorldPose(out position, out rotation);

    visualWheel.transform.position = position;
    visualWheel.transform.rotation = rotation;
  }

  private IEnumerator ClearTrickDisplay(int lastRotCount) {
    yield return new WaitForSeconds(2);

  }


}

[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor; // is this wheel attached to motor?
  public bool steering; // does this wheel apply steer angle?
}

