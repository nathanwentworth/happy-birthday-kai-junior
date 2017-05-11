using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizensAutoRun : MonoBehaviour {

  [SerializeField]
  private float speedUpDistance = 50f;
  [SerializeField]
  private float defaultSpeed = 1f;
  [SerializeField]
  private float maxSpeedMultiplier = 3f;
  private ParticleSystem heart;
  private ParticleSystem sweat;

  private Animator anim;

  public float speed { get; set; }

  private Transform kaiju;

  private void Start() {

    anim = transform.root.GetComponent<Animator>();

    speed = defaultSpeed;
    kaiju = null;
    if ((kaiju = GameObject.FindWithTag("Player").GetComponent<Transform>()) == null) {
      kaiju = GameObject.Find("Ball").GetComponent<Transform>();
    }

    heart = transform.Find("particle-meeple-hearts").GetComponent<ParticleSystem>();
    sweat = transform.Find("particle-meeple-sweat").GetComponent<ParticleSystem>();

    sweat.Stop();
    heart.Play();
  }

  private void Update() {

    if (kaiju == null) {
      return;
    }

    if (Physics.Raycast(transform.position, -transform.up, 2f)) {

      if (anim != null) {
        anim.SetBool("rolling", false);
      }

      float distance = Vector3.Distance(transform.position, kaiju.position);

      if (distance < speedUpDistance) {
        speed = (((1 - (distance / speedUpDistance)) * maxSpeedMultiplier) + defaultSpeed);
        if (heart.isPlaying) {
          heart.Stop();
        }
        if (!sweat.isPlaying) {
          sweat.Play();
        }

      } else {
        if (!heart.isPlaying) {
          heart.Play();
        }
        if (sweat.isPlaying) {
          sweat.Stop();
        }
        speed = defaultSpeed;
      }

      Vector3 rotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);
      Quaternion q = Quaternion.Euler(rotation);
      Vector3 forward = Vector3.zero;
      forward = q * Vector3.forward;

      if (anim != null) {

        anim.SetFloat("speed", speed);

        if (speed < 3f){
          anim.speed = speed / 2 + 0.5f;
        } else {
          anim.speed = (speed - 1) / 3;
        }
      }

      transform.Translate(forward * Time.deltaTime * speed);

    } else {
      if (anim != null) {
        anim.SetBool("rolling", true);
      }

      heart.Play();
    }


	}

}
