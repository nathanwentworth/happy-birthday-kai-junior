using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBlockToggle : MonoBehaviour {

  [SerializeField]
  private float onTime;
  [SerializeField]
  private float offTime;
  [SerializeField]
  private float waitDelay;

  private MeshRenderer mesh;
  private Collider[] colliders;

  private void Start() {
    colliders = GetComponents<Collider>();
    mesh = GetComponent<MeshRenderer>();

    StartCoroutine(StartTimer());
  }

  private IEnumerator StartTimer() {
    yield return new WaitForSeconds(waitDelay);
    StartCoroutine(TimedToggle(true));
  }

  private IEnumerator TimedToggle(bool onT) {
    float t = (onT) ? onTime : offTime;

    mesh.enabled = onT;
    foreach (Collider c in GetComponents<Collider>()) {
      c.enabled = onT;
    }

    while (t > 0) {
      t -= Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    StartCoroutine(TimedToggle(!onT));
  }

}
