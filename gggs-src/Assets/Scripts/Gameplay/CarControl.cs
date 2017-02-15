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

  private int rotations; // how many 180 rotations the car has done
  private float lastRotation; // the degree of the last y rotation
  private float totalRotation; // total y rotations since being not grounded

  private float autoRotationCountdown;

  [SerializeField]
  private float comboTimerDefault;
  private float comboTimer; // how much time is left to continue the combo timer
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
    autoRotationCountdown = 0;
  }

  private void Update() {
    dir = controls.Move;
    if (DataManager.AllowControl) {
      CarInput();
      if (dir != Vector2.zero) {
        autoRotationCountdown = autoRotationTimerDefault;
      }
    }
    if (!grounded) {
      RotationCount();

      if (comboCount > 1) {
        comboTimer = 6f;
      }

      if (autoRotationCountdown > 0) {
        autoRotationCountdown -= Time.deltaTime;
      } else {
        CheckGroundAngle();
      }

    } else {
      // grounded
      StartCoroutine(ClearTrickDisplay(rotations));
      ComboCountdown();
      rotations = 0;
      totalRotation = 0;
      autoRotationCountdown = autoRotationTimerDefault;
    }

    Debug.Log("autoRotationCountdown " + autoRotationCountdown);
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
      rigid.AddTorque(transform.up * dir.x * 0.2f, ForceMode.VelocityChange);
    }

    // @DEBUG: hopefully won't need this when a real respawn thing is implemented
    if (controls.Reset.WasPressed) {
      transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
      transform.rotation = Quaternion.Euler(0, 0, 0);
      Debug.Log("car reset!");
    }
  }

  private void CarMotor() {
    mph = (int)((rigid.velocity.magnitude * 10) / 2.5);
    // Debug.Log("VRROM VROOOOOOM BITCH");
    Debug.Log("mph: " + mph);
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

  private void CheckGroundAngle() {
    RaycastHit hit;

    if (Physics.Raycast(transform.position, Vector3.down, out hit, autoRotationCheckHeight)) {

      Debug.DrawLine(transform.position, hit.point, Color.red, 3f, false);
      Debug.DrawRay(hit.point, hit.normal * 10, Color.green, 3f, false);
      // checks normal of surface below, slerps to match the same direction outwards
      Quaternion currentRotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0), autoRotationSpeed * Time.deltaTime);
      Vector3 currentRotationVector = new Vector3(currentRotation.eulerAngles.x, transform.eulerAngles.y, currentRotation.eulerAngles.z);
      transform.rotation = Quaternion.Euler(currentRotationVector);

    } else {

      Quaternion currentRotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(Vector3.zero), autoRotationSpeed * Time.deltaTime);
      Vector3 currentRotationVector = new Vector3(currentRotation.eulerAngles.x, transform.eulerAngles.y, currentRotation.eulerAngles.z);
      transform.rotation = Quaternion.Euler(currentRotationVector);

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

