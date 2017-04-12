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
  private int scoreGoal;
  private int bonusScoreGoal;

  List<LevelData> levelDataList = new List<LevelData>();
  List<HighScoreData> highScoreList = new List<HighScoreData>();
  private Controls controls;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

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

    scoreGoal = DataManager.ScoreGoal;
    bonusScoreGoal = DataManager.BonusScoreGoal;

  }

  private void Start() {
    ScoreChange();
    HighScoreChange(false);
    // CumulativeScoreChange();

    nameEntryText.text = "";
    if (DataManager.LastEnteredHighScoreName != null && DataManager.LastEnteredHighScoreName != "") {
      nameEntryText.text = DataManager.LastEnteredHighScoreName;
    }
  }

  private void Update() {
    if (acceptTextEntry) {
      GetKeyboardInput();
    }

    // gameOverScrollRateMultiplier += controls.Move.X;
  }

  public void ScoreChange() {
    int score = DataManager.Score;
    scoreText.text = "Score: " + score;
    if (scoreGoal == 0) {
      scoreGoal = DataManager.ScoreGoal;
    }

    if (score > scoreGoal) {
      HighScoreChange(true);
    }

    scoreBarFill.fillAmount = ((float)score / (float)scoreGoal);
  }

  public void HighScoreChange(bool bonus) {
    if (bonusScoreGoal == 0) {
      bonusScoreGoal = DataManager.BonusScoreGoal;
    }
    int goal = (!bonus) ? scoreGoal : bonusScoreGoal;
    string goalText = (!bonus) ? "Goal: " : "Bonus: ";
    highScoreText.text = goalText + goal;
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

    // highScoreListText.text = "";

    gameOverText.text = (DataManager.Score > DataManager.ScoreGoal) ? "Level Complete!\nYou hatched the egg!" : "You didn't hatch the egg\nBetter luck next time!" ;
    newHighScoreText.SetActive(DataManager.NewHighScore);
    acceptTextEntry = true;
    StartCoroutine(ObjectsScoredDisplay());
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
    HighScoreEntry("player");
    DataManager.LastEnteredHighScoreName = nameEntryText.text;
    // highScoreListText.text = HighScoreListDisplay();
    nameEntryText.text = "";
    nameEntryHeader.SetActive(false);
    newHighScoreHeaderText.SetActive(true);

    restartButton.interactable = true;
    changeLevelButton.interactable = true;

    restartButton.Select();

    acceptTextEntry = false;

    // foreach (char c in Input.inputString) {
    //   if (c == "\b"[0]) {
    //     if (nameEntryText.text.Length != 0) {
    //       nameEntryText.text = nameEntryText.text.Substring(0, nameEntryText.text.Length - 1);
    //     }
    //   } else {
    //     if (c == "\n"[0] || c == "\r"[0]) {
    //       print("User entered their name: " + nameEntryText.text);

    //     } else if (nameEntryText.text.Length < 16) {
    //       nameEntryText.text += c.ToString();
    //     }
    //   }
    // }
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
