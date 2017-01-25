using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

  private Text scoreText;
  private Text highScoreText;
  private Text cumulativeScoreText;
  private Text timerText;
  private Text overlayText;


  private GameObject overlayPanel;
  private GameObject pausePanel;

  private void Awake() {

    scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
    cumulativeScoreText = GameObject.Find("CumulativeScoreText").GetComponent<Text>();
    timerText = GameObject.Find("TimerText").GetComponent<Text>();
    overlayText = GameObject.Find("OverlayText").GetComponent<Text>();

    overlayPanel = GameObject.Find("OverlayPanel").gameObject;
    pausePanel = GameObject.Find("PausePanel").gameObject;

  }

  private void Start() {
    ScoreChange();
    HighScoreChange();
    CumulativeScoreChange();
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
    if (overlayPanel.GetComponent<CanvasGroup>().alpha == 0) overlayPanel.GetComponent<CanvasGroup>().alpha = 1;

    overlayText.text = text;
  }

  public void HideOverlay() {
    overlayPanel.SetActive(false);
  }

  public void PausePanelDisplay(bool paused) {
    if (pausePanel == null) {
      pausePanel = GameObject.Find("PausePanel").gameObject;
    }
    Debug.Log("object called in pause display: " + pausePanel);
    pausePanel.GetComponent<CanvasGroup>().alpha = (paused) ? 1f : 0f;
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
