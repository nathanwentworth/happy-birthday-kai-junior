using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlSkate : MonoBehaviour {

  public List<AxleInfoSkate> axleInfos;
  private Rigidbody rb;
  private Vector2 dir;
  private Controls controls;
  private float mph = 0;

  private bool grounded;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }


	private void Start () {
		if (rb == null) {
      rb = GetComponent<Rigidbody>();
    }
	}
	
	private void Update () {
    dir = controls.Move;
    CarMovement();
  		
	}

  private void CarMovement() {
    rb.AddForce(dir.y * transform.forward * 200, ForceMode.Acceleration);

    Debug.Log(dir.y);

    float steering = 20 * dir.x / ((150f - (mph * 0.75f)) / 150f);


    foreach (AxleInfoSkate axleInfo in axleInfos) {
      
      if (axleInfo.steering) {
        axleInfo.leftWheel.steerAngle = steering;
        axleInfo.rightWheel.steerAngle = steering;
      }

      ApplyLocalPositionToVisuals(axleInfo.leftWheel);
      ApplyLocalPositionToVisuals(axleInfo.rightWheel);
      IsGrounded(axleInfo.rightWheel, axleInfo.leftWheel);
    }

  }

  private void IsGrounded(WheelCollider right, WheelCollider left) {
    grounded = (right.isGrounded && left.isGrounded);
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
public class AxleInfoSkate {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor; // is this wheel attached to motor?
  public bool steering; // does this wheel apply steer angle?
}



