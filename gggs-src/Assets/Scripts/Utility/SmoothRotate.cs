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

    wantedRotationAngleY = transform.eulerAngles.y;
    wantedRotationAngleY += controls.Look.X * 100 * rotationSpeed * Time.deltaTime;
    currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationSpeed * Time.deltaTime);

    wantedRotationAngleX = transform.eulerAngles.x;
    wantedRotationAngleX += controls.Look.Y * 100 * -rotationSpeed * Time.deltaTime;

    if (wantedRotationAngleX > 180) {
      wantedRotationAngleX = wantedRotationAngleX - 360;
    }

    if (wantedRotationAngleX >= maxLookX) {
      wantedRotationAngleX = maxLookX;
    } else if (wantedRotationAngleX <= minLookX) {
      wantedRotationAngleX = minLookX;
    }


    // wantedRotationAngleX = Mathf.Clamp(wantedRotationAngleX, minLookX, maxLookX);
    currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationSpeed * Time.deltaTime);


    var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0);
    transform.rotation = currentRotation;
      
  }
}
