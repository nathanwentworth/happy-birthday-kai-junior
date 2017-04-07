using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {

  public List<AxleInfo> axleInfos;
  public float maxMotorTorque;
  public float maxSteeringAngle;

  private float x1, x2, y1, y2;

  private void Start() {
    x1 = x2 = (Random.value);
    y1 = y2 = (Random.value / 2);
  }

  public void FixedUpdate() {
    float motor = maxMotorTorque * -Mathf.PerlinNoise(x1 += 0.01f, y1 += 0.01f);
    float steering = maxSteeringAngle * ((Mathf.PerlinNoise(x2 += 0.01f, y2 += 0.01f) * 2) - 1);

    foreach (AxleInfo axleInfo in axleInfos) {
      if (axleInfo.steering) {
        axleInfo.leftWheel.steerAngle = steering;
        axleInfo.rightWheel.steerAngle = steering;
      }
      if (axleInfo.motor) {
        axleInfo.leftWheel.motorTorque = motor;
        axleInfo.rightWheel.motorTorque = motor;
      }
    }
  }
}

[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor;
  public bool steering;
}