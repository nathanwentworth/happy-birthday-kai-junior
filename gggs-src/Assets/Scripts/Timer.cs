using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

  private HUDManager hudManager;

  private void Start() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;

    StartTimer(30f);
  }

  public void StartTimer(float totalTime) {
    StartCoroutine(TimerCoroutine(totalTime));
  }

  private IEnumerator TimerCoroutine(float totalTime) {
    float time = totalTime;
    while (time > 0) {
      time -= Time.deltaTime;
      hudManager.TimerChange(time);
      yield return null;
    }

    if (time <= 0) time = 0;
  }

}
