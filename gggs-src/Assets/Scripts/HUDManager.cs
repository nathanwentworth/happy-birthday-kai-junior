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
  private Text carTrickText;
  private Text comboCounterText;

  private Image comboCounterImage;


  private GameObject overlayPanel;
  private GameObject pausePanel;

  private void Awake() {

    scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
    cumulativeScoreText = GameObject.Find("CumulativeScoreText").GetComponent<Text>();
    timerText = GameObject.Find("TimerText").GetComponent<Text>();
    overlayText = GameObject.Find("OverlayText").GetComponent<Text>();

    carTrickText = GameObject.Find("CarTrickText").GetComponent<Text>();
    comboCounterText = GameObject.Find("ComboCounterText").GetComponent<Text>();

    comboCounterImage = GameObject.Find("ComboCounterImage").GetComponent<Image>();

    overlayPanel = GameObject.Find("OverlayPanel").gameObject;
    pausePanel = GameObject.Find("PausePanel").gameObject;


    // @DEBUG
    // @REFACTOR: this shouldn't even be needed
    HideOverlay();
    PausePanelDisplay(false);

  }

  private void Start() {
    ScoreChange();
    HighScoreChange();
    CumulativeScoreChange();
  }

  // @DEBUG
  // @REFACTOR
  // this can probably be removed?
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

  public void ComboCounterImageChange(float time) {
    comboCounterImage.fillAmount = (time / 6f);
  }

  public void OverlayText(string text) {
    if (overlayPanel == null) {
      overlayPanel = GameObject.Find("PausePanel").gameObject;
    }

  	if (overlayText == null) {
  			overlayText = GameObject.Find("OverlayText").GetComponent<Text>();
  	}


    if (overlayPanel.GetComponent<CanvasGroup>().alpha == 0) {
      overlayPanel.GetComponent<CanvasGroup>().alpha = 1;
      overlayPanel.GetComponent<CanvasGroup>().interactable = true;
      overlayPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    overlayText.text = text;
  }

  public void CarTrickTextChange(string text) {
    carTrickText.text = "" + text;
  }

  public void ComboCounterTextChange(string text) {
    comboCounterText.text = "" + text;
  }

  public void HideOverlay() {
    overlayPanel.GetComponent<CanvasGroup>().alpha = 0;
    overlayPanel.GetComponent<CanvasGroup>().interactable = false;
    overlayPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
  }

  public void PausePanelDisplay(bool paused) {
    if (pausePanel == null) {
      pausePanel = GameObject.Find("PausePanel").gameObject;
    }
    Debug.Log("object called in pause display: " + pausePanel);
    pausePanel.GetComponent<CanvasGroup>().alpha = (paused) ? 1f : 0f;
    pausePanel.GetComponent<CanvasGroup>().interactable = paused;
    pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = paused;
  }

  public void Restart() {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
