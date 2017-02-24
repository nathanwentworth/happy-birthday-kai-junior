using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

  private HUDManager hudManager;
  private GameFunctions gameFunctions;

  [SerializeField]
  private float defaultGameTime;
  [SerializeField]
  private float countDownTime;

  private float gameTime;
  private bool runTimer;
  private bool gameOverRun;

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;
    gameFunctions = FindObjectOfType (typeof (GameFunctions)) as GameFunctions;

    DataManager.AllowControl = false;

    StartCoroutine(CountDownTimer(countDownTime));

    gameTime = defaultGameTime;
  }

  public void StartTimer() {
    Debug.Log("timer starting");
    runTimer = true;
    gameOverRun = false;
    DataManager.AllowControl = true;
  }

  private void Update() {
    TimerLoop();
  }

  private void TimerLoop() {

    if (gameTime > 0) {
      gameTime -= Time.deltaTime;
    } else if (!gameOverRun) {
      gameTime = 0;

      DataManager.AllowControl = false;
      DataManager.GameOver = true;

      List<int> highScoreList = new List<int>();

      highScoreList = DataManager.HighScoreList;

      highScoreList.Add(DataManager.Score);
      highScoreList.Sort();
      highScoreList.Reverse();

      DataManager.HighScoreList = highScoreList;


      StartCoroutine(GameOverDelay(3));

      gameOverRun = true;
    }

    hudManager.TimerChange(Mathf.Round(gameTime));
  }

  public IEnumerator GameOverDelay(float wait) {
    yield return new WaitForSeconds(wait);


    // @REFACTOR: this is just bad lol
    while (DataManager.ObjectIsStillMoving) {
      Debug.Log("starting wait for objects to stop moving");
      yield return new WaitForSeconds(1);
      // yield return null;
    }

    hudManager.GameOverDisplay(HighScoreListDisplay(), DataManager.NewHighScore);
  }

  private string HighScoreListDisplay() {
    List<int> highScoreList = new List<int>();
    highScoreList = DataManager.HighScoreList;
    string scoresDisp = "";

    int listLength = (highScoreList.Count > 5) ? 5 : highScoreList.Count;

    for (int i = 0; i < highScoreList.Count; i++) {
      Debug.Log(highScoreList[i] + " ");
    }

    for (int i = 0; i < listLength; i++) {
      scoresDisp += highScoreList[i] + "\n";
    }

    return scoresDisp;
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

}
