using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataContainer : MonoBehaviour {

  private HUDManager hudManager;

  [SerializeField]
  private float defaultGameTime;
  [SerializeField]
  private float countDownTime;
  [SerializeField]
  private float scoreGoalInitial;
  [SerializeField]
  private float scoreGoalBonus;

  private float gameTime;
  public bool runTimer = false;
  private bool gameOverRun;

  private int _score;

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;

    DataManager.AllowControl = false;

    gameTime = defaultGameTime;
    hudManager.TimerChange(Mathf.Round(gameTime), defaultGameTime);

    StartGame();
  }

  public void StartGame() {
    StartCoroutine(CountDownTimer(countDownTime));
    // hudManager.HowToPanelHide();
    LockMouse.Lock(true);
  }

  public void StartTimer() {
    Debug.Log("timer starting");
    runTimer = true;
    gameOverRun = false;
    DataManager.AllowControl = true;
  }

  private void Update() {
    if (runTimer) {
      TimerLoop();
    }
  }

  private void TimerLoop() {

    if (gameTime > 0) {
      gameTime -= Time.deltaTime;
    } else if (!gameOverRun) {
      gameTime = 0;

      DataManager.AllowControl = false;
      DataManager.GameOver = true;

      StartCoroutine(GameOverDelay(3));

      gameOverRun = true;
    }

    hudManager.TimerChange(Mathf.Round(gameTime), defaultGameTime);
  }

  public IEnumerator GameOverDelay(float wait) {
    yield return new WaitForSeconds(wait);


    // @REFACTOR: this is just bad lol
    while (DataManager.ObjectIsStillMoving) {
      Debug.Log("starting wait for objects to stop moving");
      yield return new WaitForSeconds(1);
      // yield return null;
    }

    DataManager.Score += (int)(gameTime * 100);

    hudManager.GameOverDisplay();
    LockMouse.Lock(false);
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
    StartTimer();
  }

  private IEnumerator UnlockMouseDelay() {
    yield return new WaitForSeconds(0.1f);
    LockMouse.Lock(false);
  }

}
