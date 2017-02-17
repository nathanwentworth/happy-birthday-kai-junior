using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotate : SmoothFollow {

  public override sealed void Rotate() {
    base.Rotate();

    float wantedRotationAngleY = playerTarget.eulerAngles.y;
    float wantedRotationAngleX = playerTarget.eulerAngles.x;

    float currentRotationAngleY = transform.eulerAngles.y;
    float currentRotationAngleX = transform.eulerAngles.x;

    transform.position = new Vector3(
      Mathf.Lerp(transform.position.x, playerTarget.transform.position.x, Time.deltaTime * speed * 100),
      Mathf.Lerp(transform.position.y, playerTarget.transform.position.y, Time.deltaTime * speed * 100),
      Mathf.Lerp(transform.position.z, playerTarget.transform.position.z, Time.deltaTime * speed * 100)
    );

    if (playerTargetName.StartsWith("Ball")) {

      wantedRotationAngleY = transform.eulerAngles.y;
      wantedRotationAngleY += controls.Look * 100;
      currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);
      var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0);
      transform.rotation = currentRotation;
      
    } else if (playerTargetName.StartsWith("Cannon")) {

      wantedRotationAngleY = playerTarget.eulerAngles.y - 180;
      wantedRotationAngleX = -(playerTarget.eulerAngles.x - 15);
      currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);
      currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationSpeed * Time.deltaTime);
      var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0);
      transform.rotation = currentRotation;

    } else if (playerTargetName.StartsWith("Goblin") && DataManager.Grounded) {

      wantedRotationAngleY = playerTarget.eulerAngles.y;
      // wantedRotationAngleX = playerTarget.eulerAngles.x;
      currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);
      // currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationSpeed * Time.deltaTime);
      var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0);
      transform.rotation = currentRotation;
      
    }
  }
}
