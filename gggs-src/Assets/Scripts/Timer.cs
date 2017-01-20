using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

  private HUDManager hudManager;

  [SerializeField]
  private float defaultTime;

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;

    StartTimer(defaultTime);
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

    if (time <= 0) {
      time = 0;
      hudManager.TimerChange(time);
      DataManager.AllowControl = false;
      DataManager.GameOver = true;

      StartCoroutine(GameOverDelay());
    }
  }

  private IEnumerator GameOverDelay() {
    yield return new WaitForSeconds(3);

    string gameOverText = (DataManager.NewHighScore) ? "GAME OVER\n" + "Score: " + DataManager.Score + "\n" + "NEW HIGH SCORE!" : "GAME OVER\n" + "Score: " + DataManager.Score;

    hudManager.OverlayText(gameOverText);
  }

}
