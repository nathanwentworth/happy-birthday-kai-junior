using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class HUDManager : MonoBehaviour {

  [Header("HUD Elements")]

  [SerializeField]
  private TextMeshProUGUI scoreText;
  [SerializeField]
  private TextMeshProUGUI highScoreText;
  // [SerializeField]
  // private Text cumulativeScoreText;

  [SerializeField]
  private TextMeshProUGUI timerText;
  [SerializeField]
  private Image timerImage;
  [SerializeField]
  private TextMeshProUGUI overlayText;
  // [SerializeField]
  // private Button startButton;

  [Header("Game Over Display")]

  [SerializeField]
  private TextMeshProUGUI highScoreListText;
  [SerializeField]
  private TextMeshProUGUI nameEntryText;

  [SerializeField]
  private GameObject newHighScoreText;
  [SerializeField]
  private GameObject newHighScoreHeaderText;
  [SerializeField]
  private GameObject nameEntryHeader;

  [Header("Panels")]

  [SerializeField]
  private GameObject overlayPanel;
  // [SerializeField]
  // private GameObject howToPanel;
  [SerializeField]
  private GameObject gameOverPanel;

  [SerializeField]
  private Button restartButton;
  [SerializeField]
  private Button changeLevelButton;

  [Header("Pause")]

  [SerializeField]
  private GameObject pausePanel;

  private GameObject eventSystem;

  private bool acceptTextEntry = false;

  List<LevelData> levelDataList = new List<LevelData>();
  List<HighScoreData> highScoreList = new List<HighScoreData>();

  private void Awake() {

    // @DEBUG
    // @REFACTOR: this shouldn't even be needed
    HideOverlay();
    PausePanelDisplay(false);

    gameOverPanel.GetComponent<CanvasGroup>().alpha = 0;
    gameOverPanel.GetComponent<CanvasGroup>().interactable = false;
    gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

    newHighScoreText.SetActive(false);
    nameEntryHeader.SetActive(true);
    newHighScoreHeaderText.SetActive(false);

    restartButton.interactable = false;
    changeLevelButton.interactable = false;

  }

  private void Start() {
    ScoreChange();
    HighScoreChange();
    // CumulativeScoreChange();

    nameEntryText.text = "";
    if (DataManager.LastEnteredHighScoreName != null && DataManager.LastEnteredHighScoreName != "") {
      nameEntryText.text = DataManager.LastEnteredHighScoreName;
    }

    eventSystem = GameObject.Find("EventSystem");
  }

  private void Update() {
    if (acceptTextEntry) {
      GetKeyboardInput();
    }
  }

  // @DEBUG
  // @REFACTOR
  // this can probably be removed?
  public void UpdateScoreDisplays() {
    ScoreChange();
    HighScoreChange();
    // CumulativeScoreChange();
  }

  public void ScoreChange() {
    scoreText.text = "Score: " + DataManager.Score;
  }

  public void HighScoreChange() {
    highScoreText.text = "High Score: " + DataManager.HighScore;
  }

  // public void CumulativeScoreChange() {
  //   cumulativeScoreText.text = "Cumulative Score: " + DataManager.CumulativeScore;
  // }

  public void TimerChange(float t, float gameTime) {
    timerText.text = "" + t;
    timerImage.fillAmount = t / gameTime;

    if (t < 10) {
      timerImage.color = Color.red;
    } else {
      timerImage.color = Color.white;
    }
  }

  public void OverlayText(string text) {
    if (overlayPanel == null) {
      overlayPanel = GameObject.Find("PausePanel").gameObject;
    }

  	if (overlayText == null) {
  			overlayText = GameObject.Find("OverlayText").GetComponent<TextMeshProUGUI>();
  	}


    if (overlayPanel.GetComponent<CanvasGroup>().alpha == 0) {
      overlayPanel.GetComponent<CanvasGroup>().alpha = 1;
      overlayPanel.GetComponent<CanvasGroup>().interactable = true;
      overlayPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    overlayText.text = text;
  }

  // public void HowToPanelHide() {
  //   howToPanel.GetComponent<CanvasGroup>().alpha = 0;
  //   howToPanel.GetComponent<CanvasGroup>().interactable = false;
  //   howToPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

  //   startButton.interactable = false;
  // }

  public void GameOverDisplay() {
    gameOverPanel.GetComponent<CanvasGroup>().alpha = 1;
    gameOverPanel.GetComponent<CanvasGroup>().interactable = true;
    gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

    highScoreListText.text = "";

    newHighScoreText.SetActive(DataManager.NewHighScore);
    acceptTextEntry = true;
  }

  public void HighScoreEntry(string name) {
    int _score = DataManager.Score;

    Scene scene = SceneManager.GetActiveScene();
    string sceneName = scene.name;

    HighScoreData hs = new HighScoreData(name, _score);

    // List<LevelData> levelDataList = new List<LevelData>();

    if (DataManager.LevelDataList != null) {
      levelDataList = DataManager.LevelDataList;

      for (int i = 0; i < levelDataList.Count; i++) {
        if (sceneName == levelDataList[i].levelName) {
          highScoreList = levelDataList[i].highScores;

          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          break;
        } else if (i == levelDataList.Count - 1) {
          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          LevelData data = new LevelData(sceneName, highScoreList);
          levelDataList.Add(data);
          break;
        } else {
          Debug.Log("can't find this level name in the data list");
        }
      }
    } else {
      highScoreList.Add(hs);
      highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
      highScoreList.Reverse();

      LevelData data = new LevelData(sceneName, highScoreList);
      levelDataList.Add(data);
    }

    DataManager.LevelDataList = levelDataList;

  }

  private void GetKeyboardInput() {
    foreach (char c in Input.inputString) {
      if (c == "\b"[0]) {
        if (nameEntryText.text.Length != 0) {
          nameEntryText.text = nameEntryText.text.Substring(0, nameEntryText.text.Length - 1);    
        }
      } else {
        if (c == "\n"[0] || c == "\r"[0]) {
          print("User entered their name: " + nameEntryText.text);
          HighScoreEntry(nameEntryText.text);
          DataManager.LastEnteredHighScoreName = nameEntryText.text;
          highScoreListText.text = HighScoreListDisplay();
          acceptTextEntry = false;
          nameEntryText.text = "";
          nameEntryHeader.SetActive(false);
          newHighScoreHeaderText.SetActive(true);

          restartButton.interactable = true;
          changeLevelButton.interactable = true;

          eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(restartButton.gameObject);

        } else if (nameEntryText.text.Length < 16) {
          nameEntryText.text += c.ToString();
        }
      }
    }
  }

  private string HighScoreListDisplay() {
    string scoresDisp = "";

    int listLength = (highScoreList.Count > 5) ? 5 : highScoreList.Count;

    for (int i = 0; i < listLength; i++) {
      scoresDisp += highScoreList[i].name + ": " + highScoreList[i].score + "\n";
    }

    return scoresDisp;
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
    LockMouse.Lock(!paused);
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
