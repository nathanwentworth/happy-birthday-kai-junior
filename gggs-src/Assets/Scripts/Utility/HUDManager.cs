using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour {

  [Header("HUD Elements")]

  [SerializeField]
  private TextMeshProUGUI scoreText;
  [SerializeField]
  private TextMeshProUGUI highScoreText;
  [SerializeField]
  private TextMeshProUGUI speedText;
  // [SerializeField]
  // private Text cumulativeScoreText;

  [SerializeField]
  private TextMeshProUGUI timerText;
  [SerializeField]
  private Image timerImage;
  [SerializeField]
  private TextMeshProUGUI overlayText;

  [SerializeField]
  private Image scoreBarFill;
  // [SerializeField]
  // private Button startButton;

  [Header("Game Over Display")]

  [SerializeField]
  private TextMeshProUGUI gameOverText;
  [SerializeField]
  private TextMeshProUGUI highScoreListText;
  [SerializeField]
  private TextMeshProUGUI nameEntryText;
  [SerializeField]
  private TextMeshProUGUI objectsScoredListText;

  [SerializeField]
  private GameObject newHighScoreText;
  [SerializeField]
  private GameObject newHighScoreHeaderText;
  [SerializeField]
  private GameObject nameEntryHeader;

  [SerializeField]
  private float gameOverScrollRate;
  private float gameOverScrollRateMultiplier = 1;

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
  [SerializeField]
  private Button pauseRestartButton;

  private bool acceptTextEntry = false;
  private int currentScoreGoal;
  private int defaultScoreGoal;
  private int bonusScoreGoal;
  private int highScoreGoal;
  private int getHighScoreChecks;

  private LevelDataContainer levelDataContainer;

  List<LevelData> levelDataList = new List<LevelData>();
  List<HighScoreData> highScoreList = new List<HighScoreData>();
  private Controls controls;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {

    levelDataContainer = FindObjectOfType (typeof (LevelDataContainer)) as LevelDataContainer;

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

    defaultScoreGoal = levelDataContainer.ScoreGoalInitial;
    bonusScoreGoal = levelDataContainer.ScoreGoalBonus;
    StartCoroutine(GetHighScore());

    currentScoreGoal = defaultScoreGoal;
    Debug.Log("currentScoreGoal" + currentScoreGoal + "defaultScoreGoal" + defaultScoreGoal);

  }

  private void Start() {
    ScoreChange();
    HighScoreChange();


    nameEntryText.text = "";
    if (DataManager.LastEnteredHighScoreName != null && DataManager.LastEnteredHighScoreName != "") {
      nameEntryText.text = DataManager.LastEnteredHighScoreName;
    }
  }

  public void ScoreChange() {
    int score = DataManager.Score;
    scoreText.text = "Score: " + score;
    if (defaultScoreGoal == 0) {
      defaultScoreGoal = DataManager.ScoreGoal;
    }

    if (score >= currentScoreGoal) {
      if (currentScoreGoal == defaultScoreGoal) { currentScoreGoal = bonusScoreGoal; }
      else if (currentScoreGoal == bonusScoreGoal) { currentScoreGoal = highScoreGoal; }
      else { currentScoreGoal = score; }

      if (highScoreGoal < score) {
        highScoreGoal = score;
      }

      HighScoreChange();
    }


    scoreBarFill.fillAmount = ((float)score / (float)defaultScoreGoal);
  }

  public void HighScoreChange() {
    string goalText = "Goal: ";

    if (currentScoreGoal == defaultScoreGoal) { goalText = "Goal: "; }
    else if (currentScoreGoal == bonusScoreGoal) { goalText = "Bonus: "; }
    else { goalText = "Best: "; }

    highScoreText.text = goalText + currentScoreGoal;
  }

  public void TimerChange(float t, float gameTime) {
    timerText.text = "" + t;
    timerImage.fillAmount = t / gameTime;

    if (t < 10) {
      timerImage.color = Color.red;
    } else {
      timerImage.color = Color.white;
    }
  }

  public void SpeedometerDisplay(float speed) {
    speedText.text = Mathf.Round(speed) + "m/s\n" + Mathf.Round((speed * 2.23694f)) + "mph";
  }

  public void OverlayText(string text) {
    if (overlayPanel == null) {
      overlayPanel = GameObject.Find("PausePanel").gameObject;
    }

  	if (overlayText == null) {
			overlayText = GameObject.Find("OverlayText").GetComponent<TextMeshProUGUI>();
  	}

    CanvasGroup canvasGroup = null;
    if ((canvasGroup = overlayPanel.GetComponent<CanvasGroup>()) != null) {
      if (canvasGroup.alpha == 0) {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
      }
    } else {
      Debug.Log("no canvasGroup attached to overlayPanel");
    }

    overlayText.text = text;
  }

  public void GameOverDisplay() {
    CanvasGroup canvasGroup = null;
    if ((canvasGroup = gameOverPanel.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.alpha = 1;
      canvasGroup.interactable = true;
      canvasGroup.blocksRaycasts = true;
    }

    HighScoreEntry("player");

    if (DataManager.Score > highScoreGoal && highScoreGoal > DataManager.BonusScoreGoal) {
      gameOverText.text = "A new personal best!! Heck yeah!";
    } else if (DataManager.Score > DataManager.BonusScoreGoal) {
      gameOverText.text = "Whoa!\nYou hatched that egg real well! Congrats!";
    } else if (DataManager.Score > DataManager.ScoreGoal) {
      gameOverText.text = "Nice!\nYou hatched the egg!";
    } else {
      gameOverText.text = "You didn't hatch the egg\nBetter luck next time!";
    }

    newHighScoreText.SetActive(DataManager.NewHighScore);

    DataManager.LastEnteredHighScoreName = nameEntryText.text;
    nameEntryText.text = "";
    nameEntryHeader.SetActive(false);
    newHighScoreHeaderText.GetComponent<TextMeshProUGUI>().text = "Score: " + DataManager.Score;
    newHighScoreHeaderText.SetActive(true);

    restartButton.interactable = true;
    changeLevelButton.interactable = true;

    restartButton.Select();

    StartCoroutine(ObjectsScoredDisplay());
  }

  public void HighScoreEntry(string name) {
    int _score = DataManager.Score;

    Scene scene = SceneManager.GetActiveScene();
    string sceneName = scene.name;

    HighScoreData hs = new HighScoreData(name, _score);

    // List<LevelData> levelDataList = new List<LevelData>();

    if ((levelDataList = DataManager.LevelDataList) != null) {
      for (int i = 0; i < levelDataList.Count; i++) {
        Debug.Log(levelDataList[i].levelName);

        if (sceneName == levelDataList[i].levelName) {
          highScoreList = levelDataList[i].highScores;

          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          int _highScore = levelDataList[i].highScore;

          if (_score > levelDataList[i].highScore) {
            _highScore = _score;
          }

          levelDataList[i].highScore = _highScore;

          Debug.Log("high score n level data list " + levelDataList[i].highScore);

          break;
        } else if (i == levelDataList.Count - 1) {
          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          int _highScore = levelDataList[i].highScore;

          if (_score > levelDataList[i].highScore) {
            _highScore = _score;
          }

          LevelData data = new LevelData(sceneName, highScoreList, _highScore);
          levelDataList.Add(data);
          break;
        }
      }
    } else {
      highScoreList.Add(hs);
      highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
      highScoreList.Reverse();

      LevelData data = new LevelData(sceneName, highScoreList, _score);
      levelDataList.Add(data);
    }

    DataManager.LevelDataList = levelDataList;

  }

  private IEnumerator GetHighScore() {
    yield return new WaitForEndOfFrame();
    int _highScore = 0;

    Scene scene = SceneManager.GetActiveScene();
    string sceneName = scene.name;

    if ((levelDataList = DataManager.LevelDataList) != null) {
      for (int i = 0; i < levelDataList.Count; i++) {
        Debug.Log("Level names: " + levelDataList[i].levelName);
        if (sceneName == levelDataList[i].levelName) {
          _highScore = levelDataList[i].highScore;
          break;
        }
      }
    } else if (getHighScoreChecks < 10) {
      getHighScoreChecks++;
    }

    Debug.Log("level high score: " + _highScore);

    highScoreGoal = _highScore;
    yield return null;
  }

  private IEnumerator ObjectsScoredDisplay() {
    objectsScoredListText.text = "";
    List<string> ObjectsScoredList = DataManager.ObjectsScoredList;
    if (ObjectsScoredList != null && ObjectsScoredList.Count > 0) {
      for (int i = 0; i < ObjectsScoredList.Count; i++) {
        objectsScoredListText.text += (ObjectsScoredList[i] + "\n");
      }
    } else {
      objectsScoredListText.text = "You didn't hit any items! Try again next time!";
    }

    RectTransform gameOverPanelRect = gameOverPanel.GetComponent<RectTransform>();

    float totalHeight = gameOverPanelRect.rect.height;

    Debug.Log("totalHeight " + totalHeight);


    RectTransform rectTransform = objectsScoredListText.GetComponent<RectTransform>();
    rectTransform.position = new Vector3(rectTransform.position.x, 0, rectTransform.position.z);
    float height = objectsScoredListText.preferredHeight;
    Vector3 startPos = rectTransform.position;

    float scrollPos = startPos.y;

    float t = 0;
    // float time = 0;

    while (scrollPos <= (height + totalHeight)) {
      rectTransform.position = new Vector3(startPos.x, scrollPos, startPos.z);
      scrollPos += gameOverScrollRate * gameOverScrollRateMultiplier;

      t += Time.deltaTime;

      yield return new WaitForEndOfFrame();
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
    CanvasGroup canvasGroup = null;
    if ((canvasGroup = overlayPanel.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.alpha = 0;
      canvasGroup.interactable = false;
      canvasGroup.blocksRaycasts = false;
    }
  }

  public void PausePanelDisplay(bool paused) {
    if (pausePanel == null) {
      pausePanel = GameObject.Find("PausePanel").gameObject;
    }
    if (pauseRestartButton == null) {
      pauseRestartButton = pausePanel.transform.Find("ButtonRestart").GetComponent<Button>();
    }
    LockMouse.Lock(!paused);

    CanvasGroup canvasGroup = null;
    if ((canvasGroup = pausePanel.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.alpha = (paused) ? 1f : 0f;
      canvasGroup.interactable = paused;
      canvasGroup.blocksRaycasts = paused;
    }

    if (paused) {
      pauseRestartButton.Select();
    }


  }

  public void Restart() {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
