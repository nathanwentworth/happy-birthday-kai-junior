using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuFunctions : MonoBehaviour {

  private EventSystem eventSystem;

  [SerializeField]
  private GameObject mainContainerPanel;


  [SerializeField]
  private GameObject levelGridPanel;
  [SerializeField]
  private GameObject levelSelectPanel;
  [SerializeField]
  private GameObject[] levelSelectButtons;
  [SerializeField]
  private Image levelPreviewImage;
  [SerializeField]
  private Sprite[] levelImages;

  private bool levelHighlightChanged;
  private bool buttonHighlightChanged;
  private GameObject lastSelectedGameObject;

  private void Start() {
    Screen.lockCursor = false;
    eventSystem = FindObjectOfType (typeof (EventSystem)) as EventSystem;
    ToggleActiveMainContainer(true);
    ToggleDisplayLevelSelect(false);
    ResizeLevelGrid();
  }

  private void Update() {

    buttonHighlightChanged = (lastSelectedGameObject != eventSystem.currentSelectedGameObject);

    if (buttonHighlightChanged) {
      GameObject selectedLevelButton = eventSystem.currentSelectedGameObject;
      for (int i = 0; i < levelSelectButtons.Length; i++) {
        if (selectedLevelButton == levelSelectButtons[i]) {
          levelPreviewImage.sprite = levelImages[i];
          break;
        }
      }
    }

    lastSelectedGameObject = eventSystem.currentSelectedGameObject;
  }

  public void ToggleDisplayLevelSelect(bool toggle) {
    CanvasGroup canvasGroup = levelSelectPanel.GetComponent<CanvasGroup>();
    canvasGroup.alpha = (toggle) ? 1 : 0;
    canvasGroup.interactable = toggle;
    canvasGroup.blocksRaycasts = toggle;
  }

  public void ToggleActiveMainContainer(bool toggle) {
    CanvasGroup canvasGroup = mainContainerPanel.GetComponent<CanvasGroup>();
    canvasGroup.interactable = toggle;
    canvasGroup.blocksRaycasts = toggle;    
  }

  public void ResizeLevelGrid() {
    RectTransform levelGridContainerRect = levelGridPanel.GetComponent<RectTransform>();
    float elemWidth = ((levelGridContainerRect.rect.width - 160) / 3);
    GridLayoutGroup gridLayout = levelGridPanel.GetComponent<GridLayoutGroup>();
    gridLayout.cellSize = new Vector2(elemWidth, elemWidth);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}
