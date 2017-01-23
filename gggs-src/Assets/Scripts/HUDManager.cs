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
  private Text cumulativeScoreText;
  [SerializeField]
  private Text timerText;
  [SerializeField]
  private Text overlayText;


  [SerializeField]
  private GameObject overlayPanel;
  [SerializeField]
  private GameObject pausePanel;

  private void Start() {
    ScoreChange();
    HighScoreChange();
    CumulativeScoreChange();
    PausePanelDisplay(false);
  }

  public void UpdateScoreDisplays() {
    ScoreChange();
    HighScoreChange();
    CumulativeScoreChange();
  }

  public void ScoreChange() {
    scoreText.text = "Score: " + DataManager.Score;
  }

  public void HighScoreChange() {
    highScoreText.text = "High Score: " + DataManager.HighScore;
  }

  public void CumulativeScoreChange() {
    cumulativeScoreText.text = "Cumulative Score: " + DataManager.CumulativeScore;
  }

  public void TimerChange(float t) {
    timerText.text = "Time Left: " + t;
  }

  public void OverlayText(string text) {
    if (!overlayPanel.activeSelf) overlayPanel.SetActive(true);

    overlayText.text = text;
  }

  public void HideOverlay() {
    overlayPanel.SetActive(false);
  }

  public void PausePanelDisplay(bool paused) {
    pausePanel.SetActive(paused);
  }
}
