using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleepCheck : MonoBehaviour {

  private Rigidbody rb;
  private bool knockedOver;
  private int points;
  private HUDManager hudManager;
  private ScoreTextPopup scoreTextPopup;
  private float threshold;
  private string objName;
  private Renderer rend;
  private readonly Color hitColor = new Color(0.8F, 0.8F, 0.8F, 1F);

  private void Start() {
    objName = gameObject.name;

    if (objName.LastIndexOf("(") > 0) {
        objName = objName.Substring(0, objName.LastIndexOf("("));
    }
    if (objName.Contains("-")) {
        objName = objName.Replace("-", " ");
    }
    if (objName.Contains("_")) {
        objName = objName.Replace("_", " ");
    }

    string input = objName;
    string pattern = "\\d";
    string replacement = "";
    System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
    objName = rgx.Replace(input, replacement);

    if (objName.Contains(" ")) {
      System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
      objName = textInfo.ToTitleCase(objName);
    }

    knockedOver = false;
    rb = GetComponent<Rigidbody>();
    rend = GetComponent<Renderer>();
    hudManager = FindObjectOfType(typeof(HUDManager)) as HUDManager;
    scoreTextPopup = FindObjectOfType(typeof(ScoreTextPopup)) as ScoreTextPopup;

    StartCoroutine(GetObjectProperties());
  }

  private void FixedUpdate() {
    if (!knockedOver && rb != null && !DataManager.GameOver && rb.velocity.magnitude > 2) {
      knockedOver = true;

      if (points == 0) {
        StartCoroutine(LateScore());
      } else {
        Score();
      }

      StartCoroutine(DestroyObject());
    }
  }

  private IEnumerator DestroyObject() {
    float t = 1f;
    float _mass = rb.mass;
    while (t > 0) {
      rb.mass = Mathf.Lerp(_mass, 1, t);

      t -= Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

  }

  private IEnumerator GetObjectProperties() {
    int _points = 0;
    int i = 0;

    List<ObjectData> ObjectProperties = null;

    while (ObjectProperties == null) {
      ObjectProperties = DataManager.ObjectProperties;
      yield return new WaitForEndOfFrame();
    }

    while (_points == 0 && i < ObjectProperties.Count) {
      if (ObjectProperties[i].name.ToLower() == objName.ToLower()) {
        _points = ObjectProperties[i].points;
      }
      i++;

      Debug.Log("index i is " + i);
    }

    points = (_points != 0) ? _points : 1;

    Debug.Log("points on " + gameObject.name + " is " + points);


  }

  private void Score() {
    if (rend != null) {
      rend.material.color = hitColor;
    }

    DataManager.Score += points;
    DataManager.CumulativeScore += points;

    if (hudManager == null) {
      hudManager = FindObjectOfType(typeof(HUDManager)) as HUDManager;
    }

    hudManager.ScoreChange();

    List<string> ObjectsScoredList = (DataManager.ObjectsScoredList != null) ? DataManager.ObjectsScoredList : new List<string>();

    string ptsText = (points > 1) ? "pts" : "pt";
    ObjectsScoredList.Add(objName + " - " + points + ptsText);

    DataManager.ObjectsScoredList = ObjectsScoredList;


    if (scoreTextPopup != null) {
      scoreTextPopup.Popup(transform.position, points, transform.localScale.y);
    } else {
      Debug.Log("No scoreTextPopup in scene!");
    }

    if (DataManager.Score > DataManager.HighScore) {

      DataManager.HighScore = DataManager.Score;

      DataManager.NewHighScore = true;
      hudManager.HighScoreChange();
    }

  }

  private IEnumerator LateScore() {
    int _points = 0;
    int i = 0;

    List<ObjectData> ObjectProperties = null;

    while (ObjectProperties == null) {
      ObjectProperties = DataManager.ObjectProperties;
      yield return new WaitForEndOfFrame();
    }

    while (_points == 0 && i < ObjectProperties.Count) {
      if (ObjectProperties[i].name.ToLower() == objName.ToLower()) {
        _points = ObjectProperties[i].points;
      }
      i++;
    }

    Score();
  }

}
