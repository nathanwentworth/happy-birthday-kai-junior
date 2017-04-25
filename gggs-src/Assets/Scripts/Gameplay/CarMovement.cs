using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {


  [SerializeField]
  private List<AxleInfo> axleInfos;
  [SerializeField]
  private float maxMotorTorque;
  [SerializeField]
  private float maxBrakeTorque;
  [SerializeField]
  private float maxSteeringAngle;
  [SerializeField]
  private Transform path;
  public int carInBumper { private get; set; }
  private Transform frontGroundCheck;
  private Transform rightGroundCheck;
  private Transform leftGroundCheck;
  private List<Transform> nodes;
  private int currentNode = 0;
  private bool pathing = true;
  [SerializeField]
  private bool loop = true;

  //Connors Vars Refactor Pls
  private bool isReversing = false;
  private float reverseTime;
  public float maxReverseTime = 1.5f;

  private float x1, x2, y1, y2;

  private void Start() {
    x1 = x2 = (Random.value);
    y1 = y2 = (Random.value / 2);
    GetNodes();
    currentNode = FindClosestWaypoint();
    frontGroundCheck = transform.Find("front-ground-check").GetComponent<Transform>();
    rightGroundCheck = transform.Find("right-ground-check").GetComponent<Transform>();
    leftGroundCheck = transform.Find("left-ground-check").GetComponent<Transform>();
  }

  public void FixedUpdate() {
    Drive();
  }

  private void GetNodes() {
    List<Transform> _nodes = new List<Transform>();

    foreach (Transform child in path) {
      _nodes.Add(child);
      Debug.Log("Added: " + child.gameObject.name);
    }

    nodes = _nodes;
  }

  private void Drive() {
    float steering = 0;
    float motor = 0;

    Vector3 relVector = Vector3.zero;

    Debug.DrawRay(leftGroundCheck.position, Vector3.down * 10, Color.red, 3, false);
    Debug.DrawRay(rightGroundCheck.position, Vector3.down * 10, Color.red, 3, false);

    if (!Physics.Raycast(leftGroundCheck.position, Vector3.down, 10)) {
      steering = maxSteeringAngle * 1;
      Debug.Log("Something is to the left!");
    } else if (!Physics.Raycast(rightGroundCheck.position, Vector3.down, 10)) {
      steering = maxSteeringAngle * -1;
      Debug.Log("Something is to the right!");
    } else {
      if (nodes.Count > 1 && pathing) {
        GetNextWaypoint();
        relVector = transform.InverseTransformPoint(nodes[currentNode].position);
        relVector = Quaternion.Euler(new Vector3(0, 0, 180)) * relVector;
        steering = (relVector.x / relVector.magnitude) * maxSteeringAngle;
      } else {
        steering = maxSteeringAngle * ((Mathf.PerlinNoise(x2 += 0.01f, y2 += 0.01f) * 2) - 1);
      }
    }

    if (Physics.Raycast(frontGroundCheck.position, Vector3.down, 10) && !isReversing) {
      motor = maxMotorTorque * carInBumper * Mathf.PerlinNoise(x1 += 0.01f, y1 += 0.01f);
    } else {
      isReversing = true;
      reverseTime += Time.fixedDeltaTime;
      Debug.Log("Reverse!");
      steering *= -1;
      motor = -maxMotorTorque * carInBumper * Mathf.PerlinNoise(x1 += 0.01f, y1 += 0.01f);
      if(reverseTime >= maxReverseTime){
        isReversing = false;
        reverseTime = 0;
      }
    }

    foreach (AxleInfo axleInfo in axleInfos) {
      if (axleInfo.steering) {
        axleInfo.leftWheel.steerAngle = steering;
        axleInfo.rightWheel.steerAngle = steering;
      }
      if (axleInfo.motor) {
        axleInfo.leftWheel.motorTorque = motor;
        axleInfo.rightWheel.motorTorque = motor;
      }

      axleInfo.leftWheel.brakeTorque = (carInBumper == 1) ? 0 * maxBrakeTorque : 1 * maxBrakeTorque;
      axleInfo.rightWheel.brakeTorque = (carInBumper == 1) ? 0 * maxBrakeTorque : 1 * maxBrakeTorque;
    }
  }

  private void GetNextWaypoint() {
    if (Vector3.Distance(transform.position, nodes[currentNode].position) < 7.5f) {
      if (currentNode == nodes.Count - 1) {
        if (loop) {
          currentNode = 0;
        } else {
          pathing = false;
        }
      } else {
        currentNode++;
      }

      Debug.Log("The current node for " + gameObject.name + " is " + nodes[currentNode].gameObject.name + ", and is " + Vector3.Distance(transform.position, nodes[currentNode].position) + "m away");
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