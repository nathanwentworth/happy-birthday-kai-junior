using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using System;

public class CarControl : MonoBehaviour {

  public List<AxleInfo> axleInfos; // the information about each individual axle
  public float maxMotorTorque; // maximum torque the motor can apply to wheel
  public float maxSteeringAngle; // maximum steer angle the wheel can have
  public float maxBrakingTorque; // how fast should you brake
  public float controllerDeadzone = 0.15f;

  private float accelerationForce = 0;
  private float brakingForce = 0;
  private Rigidbody rigid;
  private int mph;

  private Vector2 dir;
  private Controls controls;

  private Vector3 groundedVelocity;

  public float MPH { get { return mph; } }

  public bool grounded { get; private set; }

  private void OnEnable() {
    controls = Controls.DefaultBindings();
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
    accelerationForce = dir.y;
    brakingForce = (controls.Interact.IsPressed) ? 1 : 0;

    if (!grounded) {
      Vector2 rotationalInput = new Vector2 (dir.y, dir.x); 
      rigid.AddRelativeTorque(rotationalInput * 5000);
    }
  }

  private void CarMotor() {
    mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
    float motor = maxMotorTorque * (accelerationForce * 3f);
    float steering = maxSteeringAngle * dir.x / ((150f - (mph * 0.75f)) / 150f);

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
    if (right.isGrounded && left.isGrounded) {
      grounded = true;
    } else {
      grounded = false;
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


}

[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor; // is this wheel attached to motor?
  public bool steering; // does this wheel apply steer angle?
}