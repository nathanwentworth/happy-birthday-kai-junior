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

  private Rigidbody rb;

  public float speed { get; set; }

  private Transform kaiju;

  private void Start() {

    rb = GetComponent<Rigidbody>();

    speed = defaultSpeed;
    kaiju = GameObject.FindWithTag("Player").GetComponent<Transform>();
    
  }
  
  private void Update() {

    float distance = Vector3.Distance(transform.position, kaiju.position);

    if (distance < speedUpDistance) {
      speed = (((1 - (distance / speedUpDistance)) * maxSpeedMultiplier) + defaultSpeed);
    } else {
      speed = defaultSpeed;
    }
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

}
