using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {


  [SerializeField]
  private List<AxleInfo> axleInfos;
  [SerializeField]
  private float maxMotorTorque;
  [SerializeField]
  private float maxSteeringAngle;
  [SerializeField]
  private List<Transform> nodes;
  private int currentNode = 0;
  private bool pathing = true;
  [SerializeField]
  private bool loop = true;

  private float x1, x2, y1, y2;

  private void Start() {
    x1 = x2 = (Random.value);
    y1 = y2 = (Random.value / 2);
    currentNode = FindClosestWaypoint();
  }

  public void FixedUpdate() {
    Drive();
  }

  private void Drive() {

    float steering = 0;
    float motor = 0;

    Vector3 relVector = Vector3.zero;
    if (nodes.Count > 1 && pathing) {
      GetNextWaypoint();
      relVector = transform.InverseTransformPoint(nodes[currentNode].position);
      relVector = Quaternion.Euler(new Vector3(0, 0, 180)) * relVector;
      steering = (relVector.x / relVector.magnitude) * maxSteeringAngle;
    } else {
      steering = maxSteeringAngle * ((Mathf.PerlinNoise(x2 += 0.01f, y2 += 0.01f) * 2) - 1);
    }

    motor = maxMotorTorque * Mathf.PerlinNoise(x1 += 0.01f, y1 += 0.01f);

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

  private void GetNextWaypoint() {
    if (Vector3.Distance(transform.position, nodes[currentNode].position) < 3f) {
      if (currentNode == nodes.Count - 1) {
        if (loop) {
          currentNode = 0;
        } else {
          pathing = false;
        }
      } else {
        currentNode++;
      }

      Debug.Log("The current node for " + gameObject.name + " is " + currentNode);
    }
  }

  private int FindClosestWaypoint() {
    float dist = Mathf.Infinity;
    int closestNode = 0;

    for (int i = 0; i < nodes.Count; i++) {
      float _dist;
      if ((_dist = Vector3.Distance(transform.position, nodes[i].position)) < dist && transform.InverseTransformPoint(nodes[i].position).z > 0) {
        closestNode = i;
        dist = _dist;
      }
    }

    Debug.Log("The closest node to " + gameObject.name + " is " + closestNode);
    return closestNode;
  }
}

[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor;
  public bool steering;
}