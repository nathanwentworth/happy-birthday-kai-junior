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
  [SerializeField]
  private Sprite[] scoreBarOverlays;
  [SerializeField]
  private Sprite[] scoreBarFills;

  [SerializeField]
  private TextMeshProUGUI timerText;
  [SerializeField]
  private Image timerImage;
  [SerializeField]
  private TextMeshProUGUI overlayText;

  [SerializeField]
  private Image scoreBarFill;
  [SerializeField]
  private Image scoreBarOverlay;

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
  private float gameOverScrollRate;
  private float gameOverScrollRateMultiplier = 1;

  [Header("Panels")]

  [SerializeField]
  private GameObject overlayPanel;
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

  private List<Scores> scoreGoals;
  private int score;
  private int goalIndex = 0;
  private int getHighScoreChecks;
  private Scene scene;
  private string sceneName;
  private int sceneIndex;

  private LevelDataContainer levelDataContainer;

  List<LevelData> levelDataList;
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

    scene = SceneManager.GetActiveScene();
    sceneName = scene.name;

    CanvasGroup gameOverCanvasGroup = null;
    if ((gameOverCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>()) != null) {
      gameOverCanvasGroup.alpha = 0;
      gameOverCanvasGroup.interactable = gameOverCanvasGroup.blocksRaycasts = false;
    }

    newHighScoreText.SetActive(false);
    newHighScoreHeaderText.SetActive(false);

    restartButton.interactable = false;
    changeLevelButton.interactable = false;

    StartCoroutine(GetScoreGoals());
    sceneIndex = GetLevelNumber();

  }

  private IEnumerator GetScoreGoals() {
    yield return new WaitForEndOfFrame();

    while (scoreGoals == null) {
      int _highScore = 0;

      if ((levelDataList = DataManager.LevelDataList) != null) {
        for (int i = 0; i < levelDataList.Count; i++) {
          if (sceneName == levelDataList[i].levelName) {
            _highScore = levelDataList[i].highScore;

            Debug.Log("Load: sceneName " + sceneName + " levelDataList[i].levelName " + levelDataList[i].levelName + " _highScore " + _highScore);
            break;
          }
        }
      } else if (getHighScoreChecks < 10) {
        getHighScoreChecks++;
      }

      List<Scores> _scoreGoals = new List<Scores>();

      _scoreGoals.Add(new Scores("Goal: ", levelDataContainer.ScoreGoalInitial));
      _scoreGoals.Add(new Scores("Bonus: ", levelDataContainer.ScoreGoalBonus));
      _scoreGoals.Add(new Scores("Best: ", _highScore));

      scoreGoals = _scoreGoals;

      goalIndex = (scoreGoals[2].val > scoreGoals[0].val) ? 2 : 0;


      yield return null;
    }

    ScoreChange();
    HighScoreChange();
  }

  public void ScoreChange() {
    if (scoreGoals == null ||
      score == null) {
      return;
    }

    score = DataManager.Score;
    scoreText.text = "Score: " + score;

    if (score >= scoreGoals[goalIndex].val) {
      if (goalIndex < 2) {
        goalIndex++;
      }

      Debug.Log("goalIndex " + goalIndex);

      if (scoreGoals[2].val < score) {
        scoreGoals[2].val = score;
      }

      HighScoreChange();
    }

    scoreBarFill.fillAmount = ((float)score / (float)scoreGoals[0].val);
  }

  public void HighScoreChange() {
    string goalText = "Goal: ";
    goalText = scoreGoals[goalIndex].text;
    highScoreText.text = goalText + scoreGoals[goalIndex].val;
  }

  private int GetLevelNumber() {
    int levelNumber = -1;
    if (System.Int32.TryParse(sceneName.Substring(sceneName.Length - 1, 1), out levelNumber)) {
      Debug.Log("The level number is " + levelNumber);
      SetUIElements(levelNumber);
    } else {
      Debug.LogWarning("The last character in the scene name isn't a number!");
    }

    return levelNumber;
  }

  private void SetUIElements(int sceneIndex) {
    scoreBarOverlay.sprite = scoreBarOverlays[sceneIndex];
    scoreBarFill.sprite = scoreBarFills[sceneIndex];

    Debug.Log(sceneIndex);
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

    if (score >= scoreGoals[2].val && scoreGoals[2].val > scoreGoals[1].val) {
      gameOverText.text = "A new personal best!! Heck yeah!";
    } else if (score > scoreGoals[1].val) {
      gameOverText.text = "Whoa!\nYou hatched that egg real well! Congrats!";
    } else if (score > scoreGoals[0].val) {
      gameOverText.text = "Nice!\nYou hatched the egg!";
    } else {
      gameOverText.text = "You didn't hatch the egg\nBetter luck next time!";
    }

    Debug.Log("ending with goal index " + goalIndex);

    newHighScoreText.SetActive(DataManager.NewHighScore);

    DataManager.LastEnteredHighScoreName = nameEntryText.text;
    nameEntryText.text = "";
    newHighScoreHeaderText.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    newHighScoreHeaderText.SetActive(true);

    restartButton.interactable = true;
    changeLevelButton.interactable = true;

    restartButton.Select();

    StartCoroutine(ObjectsScoredDisplay());
  }

  public void HighScoreEntry(string name) {
    HighScoreData hs = new HighScoreData(name, score);

    if ((levelDataList = DataManager.LevelDataList) != null) {
      for (int i = 0; i < levelDataList.Count; i++) {
        if (sceneName == levelDataList[i].levelName) {
          highScoreList = levelDataList[i].highScores;

          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          if (score > levelDataList[i].highScore) {
            levelDataList[i].highScore = score;
          }

          Debug.Log("Save (old): sceneName " + sceneName + " levelDataList[i].levelName " + levelDataList[i].levelName);
          break;
        } else if (i == levelDataList.Count - 1) {
          highScoreList.Add(hs);
          highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
          highScoreList.Reverse();

          LevelData data = new LevelData(sceneName, highScoreList, score);
          Debug.Log("Save (new): sceneName " + sceneName + " _highScore " + score);

          levelDataList.Add(data);
          break;
        }
      }
    } else {
      levelDataList = new List<LevelData>();

      highScoreList.Add(hs);
      highScoreList.Sort((x, y) => x.score.CompareTo(y.score));
      highScoreList.Reverse();

      LevelData data = new LevelData(sceneName, highScoreList, score);
      levelDataList.Add(data);
    }

    DataManager.LevelDataList = levelDataList;
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

    RectTransform rectTransform = objectsScoredListText.GetComponent<RectTransform>();
    rectTransform.position = new Vector3(rectTransform.position.x, 0, rectTransform.position.z);
    float height = objectsScoredListText.preferredHeight;
    Vector3 startPos = rectTransform.position;

    float scrollPos = startPos.y;

    float t = 0;

    while (scrollPos <= (height + totalHeight)) {
      rectTransform.position = new Vector3(startPos.x, scrollPos, startPos.z);
      scrollPos += gameOverScrollRate * gameOverScrollRateMultiplier;

      t += Time.deltaTime;

      yield return new WaitForEndOfFrame();
    }

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
    SceneManager.LoadScene(sceneName);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }
}

public class Scores {
  public string text;
  public int val;

  public Scores(string _text, int _val) {
    text = _text;
    val = _val;
  }
}

// start: 420 lines
// goal:  320 lines
