using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataContainer : MonoBehaviour {

  private HUDManager hudManager;

  [SerializeField]
  private float defaultGameTime = 30f;
  [SerializeField]
  private float countDownTime = 3f;
  [SerializeField]
  private int scoreGoalInitial = 1000;
  [SerializeField]
  private int scoreGoalBonus = 1500;

  public float CountDownTime {
    get { return countDownTime; }
    set { countDownTime = value; }
  }

  public int ScoreGoalInitial {
    get { return scoreGoalInitial; }
    set { scoreGoalInitial = value; }
  }

  public int ScoreGoalBonus {
    get { return scoreGoalBonus; }
    set { scoreGoalBonus = value; }
  }

  private float gameTime;
  public bool runTimer { get; set; }
  private bool gameOverRun;

  private int _score;

  private void Awake() {
    hudManager = FindObjectOfType (typeof (HUDManager)) as HUDManager;

    DataManager.AllowControl = false;
    DataManager.ScoreGoal = scoreGoalInitial;
    DataManager.BonusScoreGoal = scoreGoalBonus;
    runTimer = false;

    gameTime = defaultGameTime;
    hudManager.TimerChange(Mathf.Ceil(gameTime), defaultGameTime);

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

    hudManager.TimerChange(Mathf.Ceil(gameTime), defaultGameTime);
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
      hudManager.OverlayText(Mathf.Ceil(time) + "");

      time -= Time.deltaTime;
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
