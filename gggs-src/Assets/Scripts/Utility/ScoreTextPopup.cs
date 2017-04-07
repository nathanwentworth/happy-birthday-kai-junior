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
  private GameObject cam;
  private Camera mainCamera;
  private bool okay;
  [SerializeField]
  private float floatTime;
  [SerializeField]
  private float floatHeight;

  private void Awake() {
    okay = false;
    cam = GameObject.Find("Cam");
    mainCamera = cam.GetComponent<Camera>();
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
        Vector3 camView = mainCamera.WorldToViewportPoint(textObjList[i].transform.position);
        camView.x = Mathf.Clamp(camView.x, 0.2f, 0.8f);
        camView.y = Mathf.Clamp(camView.y, 0.2f, 0.8f);
        textObjList[i].transform.position = mainCamera.ViewportToWorldPoint(camView);
        textObjList[i].transform.LookAt(cam.transform.position);
        textObjList[i].transform.rotation *= Quaternion.Euler(0, 180, 0);
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
      // t = Mathf.Sin(Mathf.Pow(t, 3) * (Mathf.PI * 0.5f));
      // t = (Mathf.Cos(Mathf.Pow(t, 3) * Mathf.PI * 0.5f) * -1f) + 1f;
      t = (2 * (Mathf.Pow((1.6f * t) - 0.8f, 3) + 1)) * 0.5f;
      o = Mathf.Sin(Mathf.Pow(o, 10) * (Mathf.PI * 0.5f));
      obj.transform.position = new Vector3(obj.transform.position.x, Mathf.Lerp(startY, startY + floatHeight, t), obj.transform.position.z);
      tmp.color = new Color32(246, 149, 35, (byte)Mathf.Lerp(255, 0, o));

      lerpTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    obj.SetActive(false);

    yield return null;
  }

}
