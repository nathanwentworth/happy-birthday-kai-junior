using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuFunctions : MonoBehaviour {

  private EventSystem eventSystem;


  [Header("Main Elements")]
  [SerializeField]
  private GameObject mainContainerPanel;
  [SerializeField]
  private Button firstSelectedMainButton;
  private Button lastSelectedMainButton;
  private List<GameObject> mainButtons = new List<GameObject>();

  [Header("Level Grid Elements")]
  [SerializeField]
  private GameObject levelGridPanel;
  [SerializeField]
  private GameObject levelSelectPanel;
  [SerializeField]
  private Button firstSelectedLevelButton;
  [SerializeField]
  private GameObject[] levelSelectButtons;
  [SerializeField]
  private Image levelPreviewImage;
  [SerializeField]
  private Sprite[] levelImages;

  [Header("Options Elements")]
  [SerializeField]
  private GameObject optionsPanel;

  private bool levelHighlightChanged;
  private bool buttonHighlightChanged;
  private GameObject currentlySelectedButton;
  private GameObject lastSelectedGameObject;

  private Controls controls;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Start() {
    Screen.lockCursor = false;
    eventSystem = FindObjectOfType (typeof (EventSystem)) as EventSystem;

    mainButtons = GetButtons(mainContainerPanel);

    lastSelectedMainButton = firstSelectedMainButton;

    ToggleActiveMainContainer(true);
    ToggleDisplayLevelSelect(false);
    ResizeLevelGrid();
  }

  private void Update() {

    if (controls.Cancel.WasPressed) {
      ToggleDisplayLevelSelect(false);
      ToggleDisplayOptions(false);
      ToggleActiveMainContainer(true);
    }

    buttonHighlightChanged = (lastSelectedGameObject != eventSystem.currentSelectedGameObject);

    if (buttonHighlightChanged) {
      currentlySelectedButton = eventSystem.currentSelectedGameObject;
      for (int i = 0; i < levelSelectButtons.Length; i++) {
        if (currentlySelectedButton == levelSelectButtons[i]) {
          levelPreviewImage.sprite = levelImages[i];
          break;
        }
      }

      for (int i = 0; i < mainButtons.Count; i++) {
        if (currentlySelectedButton == mainButtons[i]) {
          lastSelectedMainButton = currentlySelectedButton.GetComponent<Button>();
          Debug.Log(currentlySelectedButton.gameObject.name);
          break;
        }
      }

    }

    lastSelectedGameObject = eventSystem.currentSelectedGameObject;
  }

  public void ToggleDisplayLevelSelect(bool toggle) {
    Animator anim = null;
    if ((anim = levelGridPanel.GetComponent<Animator>()) != null) {
      anim.SetBool("open", toggle);
    } else {
      Debug.LogWarning("No animator attached to " + levelGridPanel.name);
    }

    CanvasGroup canvasGroup = null;
    if ((canvasGroup = levelGridPanel.GetComponent<CanvasGroup>()) != null) {
      // canvasGroup.alpha = (toggle) ? 1 : 0;
      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;
    } else {
      Debug.LogWarning("No canvasGroup attached to " + levelGridPanel.name);
    }

    if (toggle && firstSelectedLevelButton != null) {
      firstSelectedLevelButton.Select();
    }
  }

  public void ToggleDisplayOptions(bool toggle) {
    Animator anim = null;
    if ((anim = optionsPanel.GetComponent<Animator>()) != null) {
      anim.SetBool("open", toggle);
    } else {
      Debug.LogWarning("No animator attached to " + optionsPanel.name);
    }

    CanvasGroup canvasGroup = null;
    if ((canvasGroup = optionsPanel.GetComponent<CanvasGroup>()) != null) {
      // canvasGroup.alpha = (toggle) ? 1 : 0;
      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;
    } else {
      Debug.LogWarning("No canvasGroup attached to " + optionsPanel.name);
    }

    if (toggle && firstSelectedLevelButton != null) {
      firstSelectedLevelButton.Select();
    }
  }

  public void ToggleActiveMainContainer(bool toggle) {
    CanvasGroup canvasGroup = null;
    if ((canvasGroup = mainContainerPanel.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;
    } else {
      Debug.LogWarning("No canvasGroup attached to " + mainContainerPanel.name);
    }
    if (toggle) {
      lastSelectedMainButton.Select();
    }
  }

  private void ResizeLevelGrid() {
    RectTransform levelGridContainerRect = null;
    GridLayoutGroup gridLayout = null;

    if ((levelGridContainerRect = levelGridPanel.GetComponent<RectTransform>()) != null
      && (gridLayout = levelGridPanel.GetComponent<GridLayoutGroup>()) != null) {

      int rowColMulti = gridLayout.constraintCount;
      float spacing = gridLayout.spacing.x;
      float elemWidth = ((levelGridContainerRect.rect.height - ((rowColMulti + 1) * spacing)) / rowColMulti);
      gridLayout.cellSize = new Vector2(elemWidth, elemWidth);
    } else {
      if (levelGridContainerRect == null) {
        Debug.LogWarning("No RectTransform attached to " + levelGridPanel.name);
      } else if (gridLayout == null) {
        Debug.LogWarning("No GridLayoutGroup attached to " + levelGridPanel.name);
      }
    }

  }

  private List<GameObject> GetButtons(GameObject container) {

    List<GameObject> list = new List<GameObject>();

    Transform child = null;
    int count = container.transform.childCount;
    for (int i = 0; i < count; i++) {
      child = container.transform.GetChild(i);
      if ((child.GetComponent<Button>()) != null) {
        list.Add(child.gameObject);
      }
      
    }

    return list;
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
