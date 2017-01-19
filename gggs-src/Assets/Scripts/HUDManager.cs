using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

  [SerializeField]
  private Text scoreText;
  [SerializeField]
  private Text highScoreText;
  [SerializeField]
  private Text timerText;

  private void Start() {
    ScoreChange();
    HighScoreChange();
  }

  public void ScoreChange() {
    scoreText.text = "Score: " + DataManager.Score;
  }

  public void HighScoreChange() {
    highScoreText.text = "High Score: " + DataManager.HighScore;
  }

  public void TimerChange(float t) {
    timerText.text = "Time Left: " + t;
  }
}
