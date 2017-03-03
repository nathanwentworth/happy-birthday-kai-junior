using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizensAutoRun : MonoBehaviour {

  [SerializeField]
  private float speedUpDistance;
  [SerializeField]
  private float defaultSpeed;
  [SerializeField]
  private float maxSpeedMultiplier;
  [SerializeField]
  private float centerOfMassYOffset;

  private Rigidbody[] rb;

  public float speed { get; set; }

  private Transform kaiju;

  private void Start() {

    rb = GetComponentsInChildren<Rigidbody>();

    speed = defaultSpeed;
    kaiju = GameObject.FindWithTag("Player").GetComponent<Transform>();
    
  }
  
  private void FixedUpdate() {

    if (Physics.Raycast(transform.position, -transform.up, 2f)) {

      float distance = Vector3.Distance(transform.position, kaiju.position);

      if (distance < speedUpDistance) {
        speed = (((1 - (distance / speedUpDistance)) * maxSpeedMultiplier) + defaultSpeed);
      } else {
        speed = defaultSpeed;
      }

      // get rotation of boy
      // convert to vector 3
      // set y rotation to 0
      // 

      Vector3 rotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);

      Quaternion q = Quaternion.Euler(rotation);

      Vector3 forward = Vector3.zero;

      forward = q * Vector3.forward;

      Debug.Log(forward);

  		transform.Translate(forward * Time.deltaTime * speed);
      //rb[1].MovePosition(transform.position * (Time.deltaTime * speed));

    }

	}

}
