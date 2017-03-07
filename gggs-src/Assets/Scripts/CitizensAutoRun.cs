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


  private Animator anim;
  private Rigidbody[] rb;

  public float speed { get; set; }

  private Transform kaiju;

  private void Start() {

    rb = GetComponentsInChildren<Rigidbody>();
    anim = transform.Find("MeepleJeffu").GetComponent<Animator>();

    speed = defaultSpeed;
    kaiju = GameObject.FindWithTag("Player").GetComponent<Transform>();
    
  }
  
  private void Update() {

    if (Physics.Raycast(transform.position, -transform.up, 2f)) {
      anim.SetBool("Rolling", false);

      float distance = Vector3.Distance(transform.position, kaiju.position);

      if (distance < speedUpDistance) {
        speed = (((1 - (distance / speedUpDistance)) * maxSpeedMultiplier) + defaultSpeed);
      } else {
        speed = defaultSpeed;
      }

      Vector3 rotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);
      Quaternion q = Quaternion.Euler(rotation);
      Vector3 forward = Vector3.zero;
      forward = q * Vector3.forward;

      anim.SetFloat("Speed", speed);
  
      if (anim.GetFloat("Speed") < 3f){
        anim.speed = speed / 2 + 0.5f;
      
      } else if (anim.GetFloat("Speed") > 3f){
        anim.speed = (speed - 1) / 3;
        // Debug.Log("animation speed: " + anim.GetFloat("Speed"));
      }

      transform.Translate(forward * Time.deltaTime * speed);
      //rb[1].MovePosition(transform.position * (Time.deltaTime * speed));

    } else {
      anim.SetBool("Rolling", true);
    }


	}

}
