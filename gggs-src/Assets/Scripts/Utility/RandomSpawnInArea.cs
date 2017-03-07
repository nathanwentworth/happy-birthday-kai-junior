using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnInArea : MonoBehaviour {

  [SerializeField]
  private float checkRadius;
  [SerializeField]
  private float numberOfCitizensToSpawn;
  [SerializeField]
  private GameObject[] citizens;

  private System.Random rnd;
  int checks = 0;

  private void Start() {
    rnd = new System.Random();
    Pool();
  }

  private void Pool() {
    for (int i = 0; i < numberOfCitizensToSpawn; i++) {
      Spawn();
    }
  }

  private void Spawn() {
    Vector3 rndPosWithin;
    rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);

    // RaycastHit hit;
    // if (Physics.Raycast(rndPosWithin, -Vector3.up, out hit, 100f)) {
    //   rndPosWithin = hit.point;
    // } else {
    //   Debug.Log("nothing is within 100 meters i guess???????");
    // }

    Vector3 rndRotation = new Vector3(transform.rotation.x, Random.Range(0, 360), transform.rotation.z);
    if (!Physics.CheckSphere(rndPosWithin, checkRadius)) {
      Instantiate(citizens[rnd.Next(citizens.Length)], rndPosWithin, Quaternion.Euler(rndRotation));
      checks = 0;
    } else if (checks < 10) {
      checks++;
      Debug.Log("Object overlapping, checking again: " + checks);
      Spawn();
    } else {
      Debug.Log("Maxed out checks, not running function anymore");
    }
  }


}
