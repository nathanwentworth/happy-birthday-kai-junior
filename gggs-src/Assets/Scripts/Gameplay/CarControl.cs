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

  private int rotations; // how many 180 rotations the car has done
  private float lastRotation; // the degree of the last y rotation
  private float totalRotation; // total y rotations since being not grounded

  private float comboTimer; // how much time is left to continue the combo timer
  [SerializeField]
  private float comboTimerDefault;
  private int comboCount; // current combo counter
  private bool runComboCountdown;
  private bool comboTrickCounted;

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
    if (!grounded) {
      CheckGroundAngle();
      RotationCount();
      if (comboCount > 1) {
        comboTimer = 6f;
      }
    } else {
      StartCoroutine(ClearTrickDisplay(rotations));
      ComboCountdown();
      rotations = 0;
      totalRotation = 0;
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

    // @DEBUG: hopefully won't need this when a real respawn thing is implemented
    if (controls.Interact.WasPressed) {
      transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
      transform.rotation = Quaternion.Euler(0, 0, 0);
      Debug.Log("car reset!");
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

  private void CheckGroundAngle() {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, Vector3.down, out hit, 2)) {
      Vector3 ground = Vector3.RotateTowards(-transform.forward, hit.normal, Time.deltaTime * 10, 0f);
      // transform.rotation = Quaternion.LookRotation(ground);
    }
  }

  private void RotationCount() {
    // counts rotations
    float rotDiff = Mathf.Abs(transform.rotation.y - lastRotation);
    totalRotation += rotDiff;

    int _rotations = (int)(totalRotation % 180);

    if (_rotations != rotations) {
      rotations = _rotations;

      if (rotations > 0) {
        hudManager.CarTrickTextChange("SICK " + (rotations * 180));
        if (!comboTrickCounted) {
          comboCount++;
          DataManager.Combo = comboCount;
          Debug.Log("Combo: " + comboCount);
          hudManager.ComboCounterTextChange(comboCount + "x");
          comboTrickCounted = true;
        }
      }
    }



    lastRotation = transform.rotation.y;
  }

  private void ComboCountdown() {
    string comboText = "";

    if (runComboCountdown && comboCount > 0) {
      comboTimer -= Time.deltaTime;
      hudManager.ComboCounterImageChange(comboTimer);
    }

    if (comboTimer <= 0) {
      if (comboCount > 1) {
        comboCount--;
        DataManager.Combo = comboCount;
        comboTimer = 6f;
        comboText = comboCount + "x";
        hudManager.ComboCounterTextChange(comboText);        
      } else {
        runComboCountdown = false;
        comboText = "";
        hudManager.ComboCounterTextChange(comboText);  
      }
    }
  }

  private void IsGrounded(WheelCollider right, WheelCollider left) {
    bool _grounded = (right.isGrounded && left.isGrounded);

    if (_grounded != grounded) {
      grounded = _grounded;
      DataManager.Grounded = _grounded;
      groundedChange = true;
      if (grounded) {
        comboTrickCounted = false;
        runComboCountdown = true;
      }
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

    if (lastRotCount == rotations) {
      hudManager.CarTrickTextChange("");
    }
  }


}

[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor; // is this wheel attached to motor?
  public bool steering; // does this wheel apply steer angle?
}

