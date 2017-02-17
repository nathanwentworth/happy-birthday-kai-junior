using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour {
	

  private float autoRotationCheckHeight = 10f;
  private float autoRotationSpeed = 10f;

	// Update is called once per frame
	void Update () {
    RaycastHit hit;

    if (Physics.Raycast(transform.position, Vector3.down, out hit, autoRotationCheckHeight)) {

      Debug.DrawLine(transform.position, hit.point, Color.red, 3f, false);
      Debug.DrawRay(hit.point, hit.normal * 10, Color.green, 3f, false);
      // checks normal of surface below, slerps to match the same direction outwards
      Quaternion look = Quaternion.LookRotation(hit.normal, -Vector3.up);
      Quaternion currentRotation = Quaternion.Slerp (transform.rotation, look, autoRotationSpeed * Time.deltaTime);

      Debug.Log("hit.normal " + hit.normal);
      Debug.Log("rot " + (new Vector3(look.eulerAngles.x, look.eulerAngles.y, look.eulerAngles.z)));

      Vector3 currentRotationVector = new Vector3(currentRotation.eulerAngles.x, transform.eulerAngles.y, currentRotation.eulerAngles.z);
      transform.rotation = Quaternion.Euler(currentRotationVector);

    }
	}
}
