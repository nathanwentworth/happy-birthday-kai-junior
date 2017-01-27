using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

  private HUDManager hudManager;

  [SerializeField]
  private float gameTime;
  [SerializeField]
  private float countDownTime;

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;

    DataManager.AllowControl = false;

    StartCoroutine(CountDownTimer(countDownTime));
  }

  public void StartTimer(float totalTime) {
    StartCoroutine(TimerCoroutine(totalTime));
    DataManager.AllowControl = true;
  }

  private IEnumerator TimerCoroutine(float totalTime) {
    float time = totalTime;
    while (time > 0) {
      time -= Time.deltaTime;
      
      hudManager.TimerChange(Mathf.Round(time));
      yield return null;
    }

    if (time <= 0) {
      time = 0;
      hudManager.TimerChange(time);
      DataManager.AllowControl = false;
      DataManager.GameOver = true;

      StartCoroutine(GameOverDelay(3));
    }
  }

  public IEnumerator GameOverDelay(float wait) {
    yield return new WaitForSeconds(wait);


    // @REFACTOR: this is just bad lol
    while (DataManager.ObjectIsStillMoving) {
      Debug.Log("starting wait for srsats");
      yield return new WaitForSeconds(1);
      Debug.Log("after wait fors etcon");
      // yield return null;
    }

    string gameOverText = (DataManager.NewHighScore) ? "GAME OVER\n" + "Score: " + DataManager.Score + "\n" + "NEW HIGH SCORE!" : "GAME OVER\n" + "Score: " + DataManager.Score;

    hudManager.OverlayText(gameOverText);
  }

  private IEnumerator CountDownTimer(float countDownTime) {
    hudManager.OverlayText("");
    yield return new WaitForSeconds(1);
    float time = countDownTime;
    while (time > 0) {
      time -= Time.deltaTime;

      hudManager.OverlayText(Mathf.Round(time) + "");
      yield return null;
    }

    hudManager.HideOverlay();
    StartTimer(gameTime);
  }

}
