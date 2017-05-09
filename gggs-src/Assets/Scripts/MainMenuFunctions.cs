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
  private Button closeButton;
  [SerializeField]
  private Button firstSelectedMainButton;
  private Button lastSelectedMainButton;
  private List<GameObject> mainButtons = new List<GameObject>();

  [Header("Level Grid Elements")]
  [SerializeField]
  private GameObject levelParentPanel;
  [SerializeField]
  private GameObject levelGridPanel;
  [SerializeField]
  private Button firstSelectedLevelButton;

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

    ToggleDisplayPanelCallback(true, mainContainerPanel);
    ToggleDisplayPanelCallback(false, levelParentPanel);
    ToggleDisplayPanelCallback(false, optionsPanel);
    ResizeLevelGrid();
  }

  private void Update() {

    if (controls.Cancel.WasPressed) {
      ToggleDisplayPanelCallback(true, mainContainerPanel);
      ToggleDisplayPanelCallback(false, levelParentPanel);
      ToggleDisplayPanelCallback(false, optionsPanel);
    }

    buttonHighlightChanged = (lastSelectedGameObject != eventSystem.currentSelectedGameObject);

    if (buttonHighlightChanged) {
      currentlySelectedButton = eventSystem.currentSelectedGameObject;

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

  public void ToggleDisplayPanel(bool toggle) {
    Debug.Log(currentlySelectedButton.name);

    // start
    if (currentlySelectedButton == mainButtons[0]) {
      ToggleDisplayPanelCallback(toggle, levelParentPanel);
    } else if (currentlySelectedButton == mainButtons[1]) {
      ToggleDisplayPanelCallback(toggle, optionsPanel);
    } else {
      Debug.Log("you clicked a button that doesn't exist! somehow!");
    }
  }

  private void ToggleDisplayPanelCallback(bool toggle, GameObject panel) {
    Animator anim = null;
    if ((anim = panel.GetComponent<Animator>()) != null) {
      anim.SetBool("open", toggle);
    } else {
      Debug.Log("No animator attached to " + panel.name);
    }

    CanvasGroup canvasGroup = null;
    if ((canvasGroup = panel.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;
    } else {
      Debug.LogWarning("No canvasGroup attached to " + panel.name);
    }

    if (toggle) {
      if (panel == levelParentPanel) {
        firstSelectedLevelButton.Select();
      } else if (panel == mainContainerPanel) {
        lastSelectedMainButton.Select();
      }
    }

    StartCoroutine(FadeCloseButton(toggle));

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
      ToggleDisplayPanelCallback(false, levelParentPanel);
      ToggleDisplayPanelCallback(false, optionsPanel);

      lastSelectedMainButton.Select();
    }
  }

  private IEnumerator FadeCloseButton(bool toggle) {
    CanvasGroup canvasGroup = null;
    if ((canvasGroup = closeButton.GetComponent<CanvasGroup>()) != null) {
      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;
      float t = 0;
      float start = (toggle) ? 0 : 1;
      float end = (toggle) ? 1 : 0;

      while (t < 1) {
        canvasGroup.alpha = Mathf.Lerp(start, end, t);

        t += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      canvasGroup.interactable = toggle;
      canvasGroup.blocksRaycasts = toggle;

    }

    yield return null;
  }

  private void ResizeLevelGrid() {
    RectTransform levelGridContainerRect = null;
    GridLayoutGroup gridLayout = null;

    if ((levelGridContainerRect = levelGridPanel.GetComponent<RectTransform>()) != null
      && (gridLayout = levelGridPanel.GetComponent<GridLayoutGroup>()) != null) {

      int rowColMulti = gridLayout.constraintCount;
      float spacing = gridLayout.spacing.x;
      float padding = gridLayout.padding.top + gridLayout.padding.bottom;
      float elemWidth = ((levelGridContainerRect.rect.height - ((rowColMulti - 1) * spacing) - padding) / rowColMulti);
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
