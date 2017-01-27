using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlacement : MonoBehaviour {

  [SerializeField]
  private GameObject bomb;
  private GameObject bombsInst;
  private List<GameObject> bombs;
  private int bombsPlaced;
  private Camera firstPersonCamera;
  [SerializeField]
  private Camera overheadCamera;
  private Timer timer;
  
  [SerializeField]
  private int numberOfBombs;

  private Controls controls;




  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {

    firstPersonCamera = GetComponent<Camera>();
    timer = FindObjectOfType (typeof (Timer)) as Timer;

    firstPersonCamera.enabled = true;
    overheadCamera.enabled = false;


    bombsPlaced = 0;

    bombs = new List<GameObject>();

    for (int i = 0; i < numberOfBombs; i++) {
      bombsInst = GameObject.Instantiate(bomb);
      bombsInst.SetActive(false);
      bombs.Add(bombsInst);
    }

  }
	
	private void Update() {
    RayPositionCheck();  
  }

  private void RayPositionCheck() {
    RaycastHit hit;

    if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f)) {

      if (controls.Interact.WasPressed) {
        Debug.Log("pressed mouse key");
        PlaceBomb(hit);
      }
    }
  
  }

  private void PlaceBomb(RaycastHit hitPos) {

    for (int i = 0; i < bombs.Count; i++) {
      if (!bombs[i].activeInHierarchy) {
        bombs[i].transform.position = hitPos.point;
        bombs[i].transform.rotation = Quaternion.identity;
        bombs[i].SetActive(true);

        Debug.Log("placed bomb!");
        bombsPlaced++;
        if (bombsPlaced == 3) {
          for (int j = 0; j < numberOfBombs; j++) {
            bombs[j].GetComponent<BombExplosion>().enabled = true;
            StartCoroutine(CameraChangeDelay());
          }
        }

        // allowFire = false;
        // StartCoroutine(AllowFireTimer());

        return;
      }
    }

  }

  private IEnumerator CameraChangeDelay() {
    yield return new WaitForSeconds(1);


    StartCoroutine(timer.GameOverDelay());
    firstPersonCamera.enabled = false;
    overheadCamera.enabled = true;

  }

}
