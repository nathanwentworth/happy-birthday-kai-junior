using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

  [SerializeField]
  private Text scoreText;

  public void ScoreChange() {
    scoreText.text = "Score: " + DataManager.Points;
  }
}
