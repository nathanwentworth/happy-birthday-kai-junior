using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {
    
  public Transform target;
  public float speed = 3f;

  public float xOffset = 0f;
  public float yOffset = 0f;
  public float zOffset = -3f;
  public float rotationSpeed = 3.0f;
  private Vector3 targetVector;

  private string targetName;

  private void Awake() {
    target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    targetName = target.name;
  }

  private void FixedUpdate () {
    if (!target) return;

    transform.position = new Vector3(
      Mathf.Lerp(transform.position.x, target.transform.position.x + xOffset, Time.deltaTime * speed),
      Mathf.Lerp(transform.position.y, target.transform.position.y + yOffset, Time.deltaTime * speed),
      Mathf.Lerp(transform.position.z, target.transform.position.z + zOffset, Time.deltaTime * speed)
    );


    float wantedRotationAngleY = target.eulerAngles.y;
    float wantedRotationAngleX = target.eulerAngles.x;
    // wantedRotationAngleY = target.eulerAngles.y
    float currentRotationAngleY = transform.eulerAngles.y;
    float currentRotationAngleX = transform.eulerAngles.x;
    // currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);

    if (targetName == "Ball") {
      transform.position = new Vector3(
        Mathf.Lerp(transform.position.x, target.transform.position.x + xOffset, Time.deltaTime * speed),
        Mathf.Lerp(transform.position.y, target.transform.position.y + yOffset, Time.deltaTime * speed),
        Mathf.Lerp(transform.position.z, target.transform.position.z + zOffset, Time.deltaTime * speed)
      );

    } else if (targetName == "Cannon(Clone)") {

      transform.position = new Vector3(
        Mathf.Lerp(transform.position.x, target.transform.position.x + xOffset, Time.deltaTime * speed),
        Mathf.Lerp(transform.position.y, target.transform.position.y + (yOffset + 3f), Time.deltaTime * speed),
        Mathf.Lerp(transform.position.z, target.transform.position.z + (zOffset - 0.5f), Time.deltaTime * speed)
      );


      wantedRotationAngleY = target.eulerAngles.y - 180;
      wantedRotationAngleX = -(target.eulerAngles.x - 15);
      currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);
      currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationSpeed * Time.deltaTime);
    }


    var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0);
    transform.rotation = currentRotation;
    transform.position += currentRotation * Vector3.forward * zOffset;
  }
}
