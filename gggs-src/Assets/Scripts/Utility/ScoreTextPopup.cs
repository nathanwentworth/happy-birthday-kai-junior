using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextPopup : MonoBehaviour {

  [SerializeField]
  private GameObject textObj;
  [SerializeField]
  private int initNum = 10;
  private List<GameObject> textObjList;
  private Transform cam;
  private bool okay;
  [SerializeField]
  private float floatTime;
  [SerializeField]
  private float floatHeight;

  private void Awake() {
    okay = false;
    cam = GameObject.Find("Cam").GetComponent<Transform>();
    if (textObj != null) {
      if (initNum > 0) {
        ObjectPopulate(initNum);
      } else {
        Debug.LogWarning("you have to specify how many objects you want to be spawned!");
      }
    } else {
      Debug.LogWarning("there's no prefab to spawn!");
    }
  }

  public void Popup(Vector3 pos, int points, float height) {
    if (!okay) return;

    for (int i = 0; i < textObjList.Count; i++) {
      if (!textObjList[i].activeInHierarchy) {
        textObjList[i].transform.position = new Vector3(pos.x, pos.y + (height / 2), pos.z);
        textObjList[i].transform.LookAt(cam.position);
        textObjList[i].GetComponent<TextMeshPro>().text = points + "";
        textObjList[i].SetActive(true);
        StartCoroutine(FloatUp(textObjList[i]));
        break;
      }
    }

  }

  private void ObjectPopulate(int num) {

    List<GameObject> list = new List<GameObject>();

    for (int i = 0; i < num; i++) {
      GameObject obj = GameObject.Instantiate(textObj) as GameObject;
      obj.name = "ScorePopupText";
      obj.SetActive(false);
      list.Add(obj);
    }

    textObjList = list;
    okay = true;
  }

  private IEnumerator FloatUp(GameObject obj) {
    float startTime = floatTime;
    float lerpTime = 0;
    float t = 0;
    float o = 0;
    float startY = obj.transform.position.y;

    TextMeshPro tmp = obj.GetComponent<TextMeshPro>();

    while (lerpTime < startTime) {
      o = t = (lerpTime / startTime);
      // t = Mathf.Sin(t * Mathf.PI * 0.5f);
      t = Mathf.Sin(Mathf.Pow(t, 3) * (Mathf.PI * 0.5f));
      o = Mathf.Sin(Mathf.Pow(t, 10) * (Mathf.PI * 0.5f));
      obj.transform.position = new Vector3(obj.transform.position.x, Mathf.Lerp(startY, startY + floatHeight, t), obj.transform.position.z);
      tmp.color = new Color32(255, 255, 255, (byte)Mathf.Lerp(255, 0, o));

      lerpTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    obj.SetActive(false);

    yield return null;
  }

}
